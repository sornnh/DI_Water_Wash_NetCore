using System.Diagnostics;


public static class AutoUpdateLauncher
{
    public static string AutomaticUpdaterFileName => "ProgramAutoUpdater.Automatic.exe";

    public static bool TryStart(
        string appName,
        string process,
        string assyPN,
        string machineName,
        string stationId,
        string lineName,
        string executablePath,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        var updaterPath = Path.Combine(AppContext.BaseDirectory, AutomaticUpdaterFileName);
        if (!File.Exists(updaterPath))
        {
            errorMessage = $"Không tìm thấy updater tự động: {updaterPath}";
            return false;
        }

        var callbackDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            appName);
        Directory.CreateDirectory(callbackDirectory);
        var callbackPath = Path.Combine(callbackDirectory, "auto-update-result.json");

        var arguments = string.Join(" ",
            Argument("--app-name", appName),
            Argument("--process", process),
            Argument("--assy-pn", assyPN),
            Argument("--machine", machineName),
            Argument("--station", stationId),
            Argument("--line", lineName),
            Argument("--exe", executablePath),
            Argument("--caller-pid", Environment.ProcessId.ToString()),
            Argument("--callback-file", callbackPath),
            Argument("--force", "true"));
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = updaterPath,
                Arguments = arguments,
                WorkingDirectory = Path.GetDirectoryName(updaterPath)!,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"Không thể khởi động updater: {ex.Message}";
            return false;
        }
    }

    private static string Argument(string name, string value)
        => $"{name} {Quote(value)}";

    private static string Quote(string value)
        => "\"" + (value ?? string.Empty).Replace("\"", "\\\"") + "\"";
}
