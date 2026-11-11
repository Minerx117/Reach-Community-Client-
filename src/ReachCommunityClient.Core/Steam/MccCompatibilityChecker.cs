using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ReachCommunityClient.Core.Steam;

/// <summary>
/// Checks the local MCC/Reach file layout without modifying or loading any game files.
/// The presence checks are diagnostics, not a guarantee that a specific game build
/// is compatible with RCC's future native runtime.
/// </summary>
public sealed class MccCompatibilityChecker
{
    private static readonly Regex BuildIdPattern = new(
        "\\\"buildid\\\"\\s*\\\"(?<build>[0-9]+)\\\"",
        RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

    public MccCompatibilityReport Check(MccInstallation installation)
    {
        ArgumentNullException.ThrowIfNull(installation);

        var root = installation.InstallDirectory;
        var reachDirectory = Path.Combine(root, "haloreach");
        var engineModule = Path.Combine(reachDirectory, "haloreach.dll");
        var mapsDirectory = Path.Combine(reachDirectory, "maps");
        var mainMenuMap = Path.Combine(mapsDirectory, "mainmenu.map");
        var mccLauncher = Path.Combine(root, "mcclauncher.exe");
        var shippingExecutable = Path.Combine(root, "mcc", "binaries", "win64", "mcc-win64-shipping.exe");

        var modulePresent = File.Exists(engineModule);
        var mapsPresent = Directory.Exists(mapsDirectory);
        var mapCount = CountMapFiles(mapsDirectory);
        var mainMenuPresent = File.Exists(mainMenuMap);
        var launcherPresent = File.Exists(mccLauncher);
        var shippingPresent = File.Exists(shippingExecutable);
        var buildId = ReadBuildId(installation.AppManifestPath);
        var engineFileVersion = modulePresent ? ReadFileVersion(engineModule) : null;
        var diagnostics = new List<CompatibilityDiagnostic>
        {
            new("MCC_ROOT", DiagnosticSeverity.Information, "MCC installation found",
                "Steam's MCC app manifest points to this installation.", root),
            launcherPresent || shippingPresent
                ? new("MCC_EXECUTABLE", DiagnosticSeverity.Information, "MCC executable found",
                    "At least one known MCC launch executable is present.", launcherPresent ? mccLauncher : shippingExecutable)
                : new("MCC_EXECUTABLE", DiagnosticSeverity.Warning, "MCC executable not found",
                    "The app manifest exists, but neither known launch executable was found.", root,
                    "Verify the MCC installation in Steam."),
            modulePresent
                ? new("REACH_ENGINE_MODULE", DiagnosticSeverity.Information, "Reach engine module found",
                    $"The user's Reach engine module is present. File version: {engineFileVersion ?? "not reported"}.", engineModule)
                : new("REACH_ENGINE_MODULE", DiagnosticSeverity.Error, "Reach engine module missing",
                    "haloreach.dll was not found in the expected Reach content folder.", reachDirectory,
                    "In Steam, open MCC Properties → DLC and install Halo: Reach content, then verify the game files."),
            mapsPresent && mapCount > 0
                ? new("REACH_MAPS", DiagnosticSeverity.Information, $"Reach map files found ({mapCount})",
                    "Map files were found in the standard Reach maps directory.", mapsDirectory)
                : new("REACH_MAPS", DiagnosticSeverity.Error, "Reach maps missing",
                    "No .map files were found in the expected Reach maps directory.", mapsDirectory,
                    "Install the Halo: Reach content in MCC's Steam DLC settings."),
            mainMenuPresent
                ? new("REACH_MAIN_MENU_MAP", DiagnosticSeverity.Information, "Original main-menu map found",
                    "mainmenu.map is present. This is a prerequisite only; the engine still needs a compatible host.", mainMenuMap)
                : new("REACH_MAIN_MENU_MAP", DiagnosticSeverity.Warning, "Original main-menu map not found",
                    "mainmenu.map was not found in the standard maps directory.", mapsDirectory,
                    "Verify the Halo: Reach content in Steam and check that the game files are complete.")
        };

        if (!string.IsNullOrWhiteSpace(buildId))
        {
            diagnostics.Insert(1, new CompatibilityDiagnostic(
                "MCC_BUILD_ID", DiagnosticSeverity.Information, $"MCC build ID: {buildId}",
                "Read from Steam's local MCC app manifest.", installation.AppManifestPath));
        }

        return new MccCompatibilityReport(
            Path.GetFullPath(root), buildId,
            Directory.Exists(reachDirectory) ? Path.GetFullPath(reachDirectory) : null,
            modulePresent ? Path.GetFullPath(engineModule) : null, engineFileVersion,
            mapsPresent ? Path.GetFullPath(mapsDirectory) : null,
            mainMenuPresent ? Path.GetFullPath(mainMenuMap) : null,
            mapCount, launcherPresent, shippingPresent, modulePresent, mainMenuPresent, diagnostics);
    }

    private static int CountMapFiles(string mapsDirectory)
    {
        if (!Directory.Exists(mapsDirectory)) return 0;
        try { return Directory.EnumerateFiles(mapsDirectory, "*.map", SearchOption.TopDirectoryOnly).Count(); }
        catch (IOException) { return 0; }
        catch (UnauthorizedAccessException) { return 0; }
    }

    private static string? ReadFileVersion(string filePath)
    {
        try { return FileVersionInfo.GetVersionInfo(filePath).FileVersion; }
        catch (FileNotFoundException) { return null; }
        catch (UnauthorizedAccessException) { return null; }
        catch (IOException) { return null; }
        catch (ArgumentException) { return null; }
        catch (System.ComponentModel.Win32Exception) { return null; }
    }

    private static string? ReadBuildId(string manifestPath)
    {
        try
        {
            if (!File.Exists(manifestPath)) return null;
            var match = BuildIdPattern.Match(File.ReadAllText(manifestPath));
            return match.Success ? match.Groups["build"].Value : null;
        }
        catch (IOException) { return null; }
        catch (UnauthorizedAccessException) { return null; }
    }
}
