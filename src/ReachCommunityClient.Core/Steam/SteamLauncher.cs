using System.ComponentModel;
using System.Diagnostics;

namespace ReachCommunityClient.Core.Steam;

/// <summary>
/// Requests that the locally installed Steam client launch MCC. This does not
/// patch, modify, or redistribute any game files.
/// </summary>
public sealed class SteamLauncher
{
    public const string MccSteamUri = "steam://rungameid/976730";

    public void LaunchMcc()
    {
        if (OperatingSystem.IsWindows())
        {
            Start(new ProcessStartInfo(MccSteamUri) { UseShellExecute = true });
            return;
        }

        var opener = OperatingSystem.IsMacOS() ? "open" : "xdg-open";
        var startInfo = new ProcessStartInfo(opener)
        {
            UseShellExecute = false
        };
        startInfo.ArgumentList.Add(MccSteamUri);

        try
        {
            Start(startInfo);
        }
        catch (Win32Exception) when (OperatingSystem.IsLinux())
        {
            // Some minimal Linux desktops do not include xdg-open. Try Steam's CLI.
            var steamStartInfo = new ProcessStartInfo("steam")
            {
                UseShellExecute = false
            };
            steamStartInfo.ArgumentList.Add("-applaunch");
            steamStartInfo.ArgumentList.Add(MccInstallation.SteamAppId.ToString());
            Start(steamStartInfo);
        }
    }

    private static void Start(ProcessStartInfo startInfo)
    {
        var process = Process.Start(startInfo);
        if (process is null)
        {
            throw new InvalidOperationException("The operating system did not start the Steam launch request.");
        }

        process.Dispose();
    }
}
