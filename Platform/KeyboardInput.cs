using Abstraction;
using Core.Input;
using static Platform.PInvoke.InputState;

namespace Platform;

public sealed class KeyboardInput : IInput
{
    private readonly static (int VKey, Key Key)[] _keyCodes = Enumerable
        .Range(0x30, 10)
        .Concat(Enumerable.Range(0x41, 6))
        .Select((e, i) =>(e, (Key)Math.Pow(2, i)))
        .ToArray();
    public Key GetPressedKey()
    {
        return _keyCodes
            .Where(k => IsKeyPressed(k.VKey))
            .Select(k => k.Key)
            .Aggregate(Key.None, (a, b) => a | b);
    }
    private static bool IsKeyPressed(int vKey) => GetAsyncKeyState(vKey) != 0;
}
