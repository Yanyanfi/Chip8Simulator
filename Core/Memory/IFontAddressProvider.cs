using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Memory;

public interface IFontAddressProvider
{
    ushort GetAddress(byte number);
}
