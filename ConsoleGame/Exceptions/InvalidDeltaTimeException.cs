using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame.Exceptions;

internal sealed class InvalidDeltaTimeException(double deltaTime):Exception($"Delta time can't be {deltaTime}")
{
    public double DeltaTime => deltaTime;
}

