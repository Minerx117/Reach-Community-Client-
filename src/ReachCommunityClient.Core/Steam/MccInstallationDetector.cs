using System.Text.RegularExpressions;

namespace ReachCommunityClient.Core.Steam;

/// <summary>Detects the Steam edition of Halo: The Master Chief Collection.</summary>
public sealed class MccInstallationDetector
{
    private static readonly Regex InstallDirectoryPattern = new(
        "\\\"installdir\\\"\\s*\\\"(?<name>[^\\\"]+)\\\"",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    private readonly SteamLibraryDiscovery _libraryDiscovery;

    public MccInstallationDetector(SteamLibraryDiscovery? libraryDiscovery = null)
    {
        _libraryDiscovery = libraryDiscovery ?? new SteamLibraryDiscovery();
    }

    public IReadOnlyList<MccInstallation> Detect()
    {
        var installations = new List<MccInstallation>();

        foreach (var libraryRoot in _libraryDiscovery.FindLibraryRoots())
        {
            var steamApps = Path.Combine(libraryRoot, "steamapps");
            var manifestPath = Path.Combine(steamApps, $"appmanifest_{MccInstallation.SteamAppId}.acf");
            if (!File.Exists(manifestPath))
            {
                continue;
            }

            try
            {
                var manifest = File.ReadAllText(manifestPath);
                var match = InstallDirectoryPattern.Match(manifest);
                var installFolderName = match.Success
                    ? match.Groups["name"].Value
                    : "Halo The Master Chief Collection";

                // Keep the manifest-derived value to a single folder name.
                installFolderName = Path.GetFileName(installFolderName.Trim());
                if (string.IsNullOrWhiteSpace(installFolderName))
                {
                    continue;
                }

                var installDirectory = Path.Combine(steamApps, "common", installFolderName);
                if (Directory.Exists(installDirectory))
                {
                    installations.Add(new MccInstallation(
                        Path.GetFullPath(installDirectory),
                        Path.GetFullPath(libraryRoot),
                        Path.GetFullPath(manifestPath)));
                }
            }
            catch (IOException)
            {
                // Keep scanning if one library's manifest cannot be read.
            }
            catch (UnauthorizedAccessException)
            {
                // Keep scanning libraries accessible to the current user.
            }
        }

        return installations;
    }
}
