using Avalonia.Controls;
using Avalonia.Interactivity;
using ReachCommunityClient.Core.Steam;

namespace ReachCommunityClient.App;

public partial class MainWindow : Window
{
    private readonly MccInstallationDetector _detector = new();
    private readonly MccCompatibilityChecker _checker = new();
    private readonly SteamLauncher _steamLauncher = new();
    private MccInstallation? _installation;
    private MccCompatibilityReport? _report;

    public MainWindow()
    {
        InitializeComponent();
        RefreshInstallation();
    }

    private void Refresh_Click(object? sender, RoutedEventArgs e) => RefreshInstallation();

    private void LaunchMcc_Click(object? sender, RoutedEventArgs e)
    {
        if (_installation is null)
        {
            AppStatusText.Text = "No MCC Steam installation was detected.";
            return;
        }

        try
        {
            _steamLauncher.LaunchMcc();
            AppStatusText.Text = "Steam was asked to launch the official MCC client.";
        }
        catch (Exception ex)
        {
            AppStatusText.Text = $"Could not start Steam: {ex.Message}";
        }
    }

    private void RefreshInstallation()
    {
        try
        {
            var installations = _detector.Detect();
            _installation = installations.FirstOrDefault();
            LaunchMccButton.IsEnabled = _installation is not null;

            if (_installation is null)
            {
                _report = null;
                InstallStatusText.Text = "MCC Steam installation not found";
                InstallPathText.Text = "Install MCC through Steam, then refresh. Secondary Steam libraries are searched.";
                ReachStatusText.Text = "Reach content not checked";
                ReachDetailsText.Text = "No MCC installation was detected.";
                DiagnosticsText.Text = string.Empty;
            }
            else
            {
                InstallStatusText.Text = installations.Count == 1
                    ? "Steam MCC installation detected"
                    : $"{installations.Count} MCC installations detected";
                InstallPathText.Text = _installation.InstallDirectory;
                _report = _checker.Check(_installation);
                ReachStatusText.Text = _report.MainMenuAssetsReady
                    ? "Reach engine module and main-menu map detected"
                    : _report.ReachContentPresent
                        ? "Reach content detected; main-menu map missing"
                        : "Reach content appears incomplete or is not installed";
                ReachDetailsText.Text = $"Steam build ID: {_report.GameBuildId ?? "not available"}\nReach maps found: {_report.MapFileCount}\nReach DLL version: {_report.ReachEngineFileVersion ?? "not reported"}\nEngine module: {(_report.ReachEngineModulePresent ? "present" : "missing")} · Main-menu map: {(_report.MainMenuMapPresent ? "present" : "missing")}";
                DiagnosticsText.Text = string.Join("\n", _report.Diagnostics.Select(d =>
                    $"{SeverityMark(d.Severity)} {d.Title}: {d.Details}" +
                    (string.IsNullOrWhiteSpace(d.SuggestedAction) ? string.Empty : $"\n   Suggested action: {d.SuggestedAction}")));
            }

            RuntimeStatusText.Text = "RCC's own engine runtime is not implemented yet. This application does not launch another client or external backend.";
            MultiplayerStatusText.Text = "RCC's live server directory, session service, and game networking are not implemented yet. No demo or fabricated servers are shown.";
            AppStatusText.Text = "Installation check complete. No game files were modified.";
        }
        catch (Exception ex)
        {
            _installation = null;
            _report = null;
            InstallStatusText.Text = "Could not inspect Steam libraries";
            InstallPathText.Text = ex.Message;
            ReachStatusText.Text = "Diagnostics unavailable";
            DiagnosticsText.Text = ex.Message;
            LaunchMccButton.IsEnabled = false;
        }
    }

    private static string SeverityMark(DiagnosticSeverity severity) => severity switch
    {
        DiagnosticSeverity.Error => "[ERROR]",
        DiagnosticSeverity.Warning => "[WARN]",
        _ => "[OK]"
    };
}
