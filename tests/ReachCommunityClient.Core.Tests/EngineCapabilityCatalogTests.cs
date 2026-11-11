using ReachCommunityClient.Core.Engine;
using Xunit;

namespace ReachCommunityClient.Core.Tests;

public sealed class EngineCapabilityCatalogTests
{
    [Fact]
    public void GetCurrent_DoesNotClaimMainMenuOrMultiplayerAreReady()
    {
        var mainMenu = EngineCapabilityCatalog.GetById("reach-main-menu");
        var multiplayer = EngineCapabilityCatalog.GetById("multiplayer-sessions");

        Assert.Equal(EngineCapabilityState.RequiresRccEngineRuntime, mainMenu.State);
        Assert.Equal(EngineCapabilityState.RequiresRccEngineRuntime, multiplayer.State);
    }

    [Fact]
    public void GetCurrent_ContainsServerBrowserAsRccServiceDependent()
    {
        var serverBrowser = EngineCapabilityCatalog.GetById("server-browser");

        Assert.Equal(EngineCapabilityState.RequiresRccCommunityService, serverBrowser.State);
    }

    [Fact]
    public void GetById_ThrowsForUnknownCapability()
    {
        Assert.Throws<KeyNotFoundException>(() => EngineCapabilityCatalog.GetById("unknown"));
    }
}
