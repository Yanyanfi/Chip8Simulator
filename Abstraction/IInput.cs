using Core.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Abstraction;

public interface IInput
{
    Key GetPressedKey();
}
