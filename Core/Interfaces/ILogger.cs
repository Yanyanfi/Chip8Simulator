using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces;

public interface ILogger
{
    void Log(string message);
    Task LogAsync(string message);
    void Save();
    void LogAndSave(string message);
}
