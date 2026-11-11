using System.Text.RegularExpressions;

namespace ReachCommunityClient.Core.Steam;

/// <summary>
/// Finds Steam library roots from the conventional Steam install locations and
/// the libraryfolders.vdf file in each discovered Steam installation.
/// </summary>
public sealed class SteamLibraryDiscovery
{
    private static readonly Regex ModernLibraryPath = new(
        "\\\"path\\\"\\s*\\\"(?<path>(?:\\\\.|[^\\\"])*)\\\"",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private static readonly Regex LegacyLibraryPath = new(
        "\\\"(?:0|[1-9][0-9]*)\\\"\\s*\\\"(?<path>(?:\\\\.|[^\\\"])*)\\\"",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly IEnumerable<string>? _candidateRoots;

    public SteamLibraryDiscovery(IEnumerable<string>? candidateRoots = null)
    {
        _candidateRoots = candidateRoots;
    }

    public IReadOnlyList<string> FindLibraryRoots()
    {
        var roots = _candidateRoots ?? GetDefaultSteamRoots();
        var found = new HashSet<string>(PathComparer);
        var pending = new Queue<string>(roots.Where(Directory.Exists));

        while (pending.Count > 0)
        {
            var root = NormalizePath(pending.Dequeue());
            if (!found.Add(root))
            {
                continue;
            }

            var libraryFoldersFile = Path.Combine(root, "steamapps", "libraryfolders.vdf");
            if (!File.Exists(libraryFoldersFile))
            {
                continue;
            }

            try
            {
                foreach (var libraryRoot in ParseLibraryFolderPaths(File.ReadAllText(libraryFoldersFile)))
                {
                    var normalized = NormalizePath(libraryRoot);
                    if (Directory.Exists(normalized) && !found.Contains(normalized))
                    {
                        pending.Enqueue(normalized);
                    }
                }
            }
            catch (IOException)
            {
                // An unreadable secondary library should not prevent discovery of others.
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore libraries that the current user cannot inspect.
            }
        }

        return found.ToArray();
    }

    /// <summary>Parses both current (path-key) and older numeric VDF library entries.</summary>
    public static IReadOnlyList<string> ParseLibraryFolderPaths(string contents)
    {
        ArgumentNullException.ThrowIfNull(contents);

        var paths = new HashSet<string>(PathComparer);
        AddMatches(ModernLibraryPath, contents, paths);
        AddMatches(LegacyLibraryPath, contents, paths);
        return paths.ToArray();
    }

    private static void AddMatches(Regex regex, string contents, ISet<string> paths)
    {
        foreach (Match match in regex.Matches(contents))
        {
            var value = match.Groups["path"].Value
                .Replace("\\\\", "\\", StringComparison.Ordinal)
                .Replace("\\\"", "\"", StringComparison.Ordinal);

            if (!string.IsNullOrWhiteSpace(value))
            {
                paths.Add(value);
            }
        }
    }

    private static IEnumerable<string> GetDefaultSteamRoots()
    {
        var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        if (OperatingSystem.IsWindows())
        {
            var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            return new[]
            {
                Path.Combine(programFilesX86, "Steam"),
                Path.Combine(programFiles, "Steam"),
                Path.Combine(localAppData, "Steam")
            };
        }

        if (OperatingSystem.IsMacOS())
        {
            return new[]
            {
                Path.Combine(home, "Library", "Application Support", "Steam"),
                "/Applications/Steam.app/Contents/MacOS"
            };
        }

        return new[]
        {
            Path.Combine(home, ".steam", "steam"),
            Path.Combine(home, ".steam", "root"),
            Path.Combine(home, ".local", "share", "Steam"),
            Path.Combine(home, ".var", "app", "com.valvesoftware.Steam", ".local", "share", "Steam")
        };
    }

    private static string NormalizePath(string path) => Path.GetFullPath(path.Trim());

    private static StringComparer PathComparer => OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase
        : StringComparer.Ordinal;
}
