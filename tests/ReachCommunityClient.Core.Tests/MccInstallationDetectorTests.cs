using Xunit;
using ReachCommunityClient.Core.Steam;

namespace ReachCommunityClient.Core.Tests;

public sealed class MccInstallationDetectorTests
{
    [Fact]
    public void Detect_FindsMccFromSteamAppManifest()
    {
        var root = Path.Combine(Path.GetTempPath(), "ReachCommunityClient.Tests", Guid.NewGuid().ToString("N"));
        var steamApps = Path.Combine(root, "steamapps");
        var install = Path.Combine(steamApps, "common", "Halo The Master Chief Collection");
        Directory.CreateDirectory(install);

        try
        {
            File.WriteAllText(
                Path.Combine(steamApps, "appmanifest_976730.acf"),
                "\"AppState\" { \"appid\" \"976730\" \"installdir\" \"Halo The Master Chief Collection\" }");

            var detector = new MccInstallationDetector(new SteamLibraryDiscovery(new[] { root }));
            var result = detector.Detect();

            var found = Assert.Single(result);
            Assert.Equal(Path.GetFullPath(install), found.InstallDirectory);
            Assert.Equal(Path.GetFullPath(root), found.SteamLibraryRoot);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Detect_IgnoresManifestWhenGameFolderIsMissing()
    {
        var root = Path.Combine(Path.GetTempPath(), "ReachCommunityClient.Tests", Guid.NewGuid().ToString("N"));
        var steamApps = Path.Combine(root, "steamapps");
        Directory.CreateDirectory(steamApps);

        try
        {
            File.WriteAllText(
                Path.Combine(steamApps, "appmanifest_976730.acf"),
                "\"AppState\" { \"appid\" \"976730\" \"installdir\" \"Halo The Master Chief Collection\" }");

            var detector = new MccInstallationDetector(new SteamLibraryDiscovery(new[] { root }));

            Assert.Empty(detector.Detect());
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
