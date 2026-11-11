using ReachCommunityClient.Core.Steam;
using Xunit;

namespace ReachCommunityClient.Core.Tests;

public sealed class MccCompatibilityCheckerTests
{
    [Fact]
    public void Check_ReportsReachEngineAndMainMenuWhenFilesExist()
    {
        var root = Path.Combine(Path.GetTempPath(), "ReachCommunityClient.Tests", Guid.NewGuid().ToString("N"));
        var steamApps = Path.Combine(root, "steamapps");
        var install = Path.Combine(steamApps, "common", "Halo The Master Chief Collection");
        var reach = Path.Combine(install, "haloreach");
        var maps = Path.Combine(reach, "maps");
        Directory.CreateDirectory(maps);
        File.WriteAllText(Path.Combine(steamApps, "appmanifest_976730.acf"),
            "\"AppState\" { \"appid\" \"976730\" \"installdir\" \"Halo The Master Chief Collection\" \"buildid\" \"12345678\" }");
        File.WriteAllBytes(Path.Combine(reach, "haloreach.dll"), [0x4D, 0x5A]);
        File.WriteAllBytes(Path.Combine(maps, "mainmenu.map"), [0x01]);
        File.WriteAllBytes(Path.Combine(maps, "test.map"), [0x02]);

        try
        {
            var installation = Assert.Single(new MccInstallationDetector(new SteamLibraryDiscovery([root])).Detect());
            var report = new MccCompatibilityChecker().Check(installation);

            Assert.True(report.ReachContentPresent);
            Assert.True(report.MainMenuAssetsReady);
            Assert.Equal(2, report.MapFileCount);
            Assert.Equal("12345678", report.GameBuildId);
            Assert.Contains(report.Diagnostics, d => d.Code == "REACH_MAIN_MENU_MAP" && d.Severity == DiagnosticSeverity.Information);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Check_ReportsMissingReachContentWithoutThrowing()
    {
        var root = Path.Combine(Path.GetTempPath(), "ReachCommunityClient.Tests", Guid.NewGuid().ToString("N"));
        var steamApps = Path.Combine(root, "steamapps");
        var install = Path.Combine(steamApps, "common", "Halo The Master Chief Collection");
        Directory.CreateDirectory(install);
        var manifest = Path.Combine(steamApps, "appmanifest_976730.acf");
        File.WriteAllText(manifest, "\"AppState\" { \"appid\" \"976730\" \"installdir\" \"Halo The Master Chief Collection\" }");

        try
        {
            var report = new MccCompatibilityChecker().Check(new MccInstallation(install, root, manifest));
            Assert.False(report.ReachContentPresent);
            Assert.False(report.MainMenuAssetsReady);
            Assert.Contains(report.Diagnostics, d => d.Code == "REACH_ENGINE_MODULE" && d.Severity == DiagnosticSeverity.Error);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
