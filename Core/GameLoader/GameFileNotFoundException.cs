using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.GameLoader;

public sealed class GameFileNotFoundException(string filePath): Exception
{
    public string Path => filePath;
    public static void ThrowIfNotExists(string filePath)
    {
        if (!File.Exists(filePath))
            throw new GameFileNotFoundException(filePath);
    }
}
