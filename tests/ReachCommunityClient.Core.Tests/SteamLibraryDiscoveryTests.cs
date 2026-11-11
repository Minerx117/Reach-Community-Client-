using Xunit;
using ReachCommunityClient.Core.Steam;

namespace ReachCommunityClient.Core.Tests;

public sealed class SteamLibraryDiscoveryTests
{
    [Fact]
    public void ParseLibraryFolderPaths_ReadsCurrentVdfFormat()
    {
        const string vdf = """
            "libraryfolders"
            {
                "0"
                {
                    "path" "C:\\Program Files (x86)\\Steam"
                    "apps" { }
                }
                "1"
                {
                    "path" "D:\\SteamLibrary"
                    "apps" { }
                }
            }
            """;

        var paths = SteamLibraryDiscovery.ParseLibraryFolderPaths(vdf);

        Assert.Contains("C:\\Program Files (x86)\\Steam", paths);
        Assert.Contains("D:\\SteamLibrary", paths);
    }

    [Fact]
    public void ParseLibraryFolderPaths_ReadsLegacyVdfFormat()
    {
        const string vdf = """
            "libraryfolders"
            {
                "0" "C:\\Steam"
                "1" "D:\\Games\\SteamLibrary"
            }
            """;

        var paths = SteamLibraryDiscovery.ParseLibraryFolderPaths(vdf);

        Assert.Contains("C:\\Steam", paths);
        Assert.Contains("D:\\Games\\SteamLibrary", paths);
    }

    [Fact]
    public void ParseLibraryFolderPaths_ReturnsNoPathsForEmptyInput()
    {
        Assert.Empty(SteamLibraryDiscovery.ParseLibraryFolderPaths(string.Empty));
    }
}
