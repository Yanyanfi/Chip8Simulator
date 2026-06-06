using System;
using System.Collections.Generic;
using System.Text;

namespace Core.GameLoader;

public interface IGameLoader
{
    ///<exception cref="GameFileNotFoundException"/>
    Task LoadAsync(byte[] memory, string filePath);
}
