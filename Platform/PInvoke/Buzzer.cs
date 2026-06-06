using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Platform.PInvoke
{
    internal static partial class Buzzer
    {
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool Beep(int frequency, int duration);
    }
}
