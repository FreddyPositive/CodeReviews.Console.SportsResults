namespace SportsDataReader.Logging;


public class FileLogger : IAppLogger
{
    private readonly string _logFilePath;

    public FileLogger(string logFilePath)
    {
        _logFilePath = logFilePath;

        // Ensure directory exists
        var dir = Path.GetDirectoryName(_logFilePath);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
    }

    public void Info(string message)
    {
        WriteLog("INFO", message);
    }

    public void Warning(string message)
    {
        WriteLog("WARN", message);
    }

    public void Error(string message, Exception ex = null)
    {
        var fullMessage = ex == null
            ? message
            : $"{message} | Exception: {ex.Message}\n{ex.StackTrace}";

        WriteLog("ERROR", fullMessage);
    }

    private void WriteLog(string level, string message)
    {
        var logEntry =
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

        File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
    }
}