using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Platform.PInvoke;

internal static partial class InputState
{
    [LibraryImport("user32.dll")]
    internal static partial ushort GetAsyncKeyState(int vKey);
}
