using Core.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Platform;

public sealed class Logger(string path) : ILogger
{
    private readonly StreamWriter _writer = new(path);
    public void Log(string message) => LogAsync(message).Wait();

    public void LogAndSave(string message)
    {
#if DEBUG
        Log(message);
        Save();
#endif
    }
    
    public async Task LogAsync(string message)
    {
#if DEBUG
        await _writer.WriteLineAsync(@$"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}");
#endif
    }

    public void Save() => _writer.Flush();
}

