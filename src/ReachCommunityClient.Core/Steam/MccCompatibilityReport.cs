namespace ReachCommunityClient.Core.Steam;

public enum DiagnosticSeverity
{
    Information,
    Warning,
    Error
}

public sealed record CompatibilityDiagnostic(
    string Code,
    DiagnosticSeverity Severity,
    string Title,
    string Details,
    string? Path = null,
    string? SuggestedAction = null);

/// <summary>Evidence-based report for the local Steam MCC and Halo: Reach files.</summary>
public sealed record MccCompatibilityReport(
    string InstallDirectory,
    string? GameBuildId,
    string? ReachDirectory,
    string? ReachEngineModule,
    string? ReachEngineFileVersion,
    string? MapsDirectory,
    string? MainMenuMap,
    int MapFileCount,
    bool MccLauncherPresent,
    bool MccShippingExecutablePresent,
    bool ReachEngineModulePresent,
    bool MainMenuMapPresent,
    IReadOnlyList<CompatibilityDiagnostic> Diagnostics)
{
    public bool MccLaunchReady => MccLauncherPresent || MccShippingExecutablePresent;
    public bool ReachContentPresent => ReachEngineModulePresent && MapFileCount > 0;
    public bool MainMenuAssetsReady => ReachEngineModulePresent && MainMenuMapPresent;
}
