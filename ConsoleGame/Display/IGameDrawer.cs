using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame.Display;

internal interface IGameDrawer
{
    void Draw(IEnumerable<bool[]> pixels);
}
