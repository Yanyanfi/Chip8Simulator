using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame.Exceptions;

internal sealed class InvalidInstructionCountException(int instrCount):Exception($"Instruction count can't be {instrCount}")
{
    public int InstrCount => instrCount;
}
