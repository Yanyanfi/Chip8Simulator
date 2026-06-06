using System;
using System.Collections.Generic;
using System.Text;

namespace Core.MemoryInitializer;

public interface IMemoryInitializer
{
    void Initialize(byte[] memory);
}
