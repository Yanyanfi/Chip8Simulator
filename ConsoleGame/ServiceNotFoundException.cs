using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleGame;

internal sealed class ServiceNotFoundException<TService>() : Exception;
