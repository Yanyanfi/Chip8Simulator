using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform;

public sealed class Logger(string path) : ILogger
{
    private readonly StreamWriter _writer = new(path);
    public void Log(string message) => LogAsync(message).Wait();

    public void LogAndSave(string message)
    {
        Log(message);
        Save();
    }

    public async Task LogAsync(string message)
    {
        await _writer.WriteLineAsync(@$"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
    }

    public void Save() => _writer.Flush();
}

