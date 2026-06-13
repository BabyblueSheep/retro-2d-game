using System;
using System.Collections.Generic;
using System.Text;

namespace Retro2DGame.Core.Game;

internal sealed class Logger
{
    public enum LogPriorityLevel
    {
        Debug,
        Info,
        Warn,
        Error
    }

    private bool _logToConsole;
    private bool _logToFile;
    private string _logFilePath;

    public LogPriorityLevel LogPriority { get; set; } = LogPriorityLevel.Info;

    public Logger()
    {
        _logFilePath = "";
    }

    public void EnableConsoleLogging()
    {
        _logToConsole = true;
    }

    public void DisableConsoleLogging()
    {
        _logToConsole = false;
    }

    public void EnableFileLogging(string filePath)
    {
        _logToFile = true;
        _logFilePath = filePath;
    }

    public void DisableFileLogging()
    {
        _logToFile = false;
        _logFilePath = "";
    }

    private void Log(string text, ConsoleColor color)
    {
        if (_logToConsole)
        {
            var previousBackgroundColor = Console.BackgroundColor;
            var previousForegroundColor = Console.ForegroundColor;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            Console.WriteLine(text);

            Console.BackgroundColor = previousBackgroundColor;
            Console.ForegroundColor = previousForegroundColor;
        }

        if (_logToFile)
        {
            File.AppendAllText(_logFilePath, $"{text}{Environment.NewLine}");
        }
    }

    public void LogDebug(string text)
    {
        if (LogPriority > LogPriorityLevel.Debug)
            return;
        Log($"[{DateTime.Now}] [DEBUG] {text}", ConsoleColor.DarkGray);
    }

    public void LogInfo(string text)
    {
        if (LogPriority > LogPriorityLevel.Info)
            return;
        Log($"[{DateTime.Now}] [INFO] {text}", ConsoleColor.White);
    }

    public void LogWarn(string text)
    {
        if (LogPriority > LogPriorityLevel.Warn)
            return;
        Log($"[{DateTime.Now}] [WARN] {text}", ConsoleColor.Yellow);
    }

    public void LogError(string text)
    {
        if (LogPriority > LogPriorityLevel.Error)
            return;
        Log($"[{DateTime.Now}] [ERROR] {text}", ConsoleColor.Red);
    }
}
