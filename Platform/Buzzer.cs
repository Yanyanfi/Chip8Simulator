using Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platform;

public sealed class Buzzer : IBuzzer
{
    public void Beep(int miliseconds)
    {
        PInvoke.Buzzer.Beep(750, miliseconds);
    }
}
