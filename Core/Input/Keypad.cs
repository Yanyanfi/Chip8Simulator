namespace Core.Input;

public sealed class Keypad
{
    public Key Key 
    { 
        get;
        set
        {
            if (value == field)
                return;
            if (value == Key.None)
            {
                PreviousKey = Key;
                field = value;
                Released?.Invoke(this, PreviousKey);
                Released = null;
                return;
            }
            PreviousKey = Key.None;
            field = value;
            Pressed?.Invoke(this, value);
            Pressed = null;
        } 
    } = Key.None;
    public Key PreviousKey { get; private set; } = Key.None;
    //public bool IsWaitingForKey => (Pressed, Released) is not (null, null);
    public event EventHandler<Key>? Pressed;
    public event EventHandler<Key>? Released;
    public void Clear() => Key = Key.None;
    public bool IsPressed(Key key) => Key.HasFlag(key);
    public bool IsPressed(byte code) => code switch
    {
        0 => Key.HasFlag(Key.Zero),
        1 => Key.HasFlag(Key.One),
        2 => Key.HasFlag(Key.Two),
        3 => Key.HasFlag(Key.Three),
        4 => Key.HasFlag(Key.Four),
        5 => Key.HasFlag(Key.Five),
        6 => Key.HasFlag(Key.Six),
        7 => Key.HasFlag(Key.Seven),
        8 => Key.HasFlag(Key.Eight),
        9 => Key.HasFlag(Key.Nine),
        10 => Key.HasFlag(Key.Ten),
        11 => Key.HasFlag(Key.Elevent),
        12 => Key.HasFlag(Key.Twelve),
        13 => Key.HasFlag(Key.Thirteen),
        14 => Key.HasFlag(Key.Fourteen),
        15 => Key.HasFlag(Key.Fifteen),
        _ => IsPressed((byte)(code % 16))
    };
    public byte GetKeyCode() => (byte)(Math.Log2((int)Key));
    public byte GetPreviousKeyCode() => (byte)(Math.Log2((int)PreviousKey));
}
[Flags]
public enum Key { None = 0, Zero = 1, One = 2, Two = 4, Three = 8, Four = 16, Five = 32, Six = 64, Seven = 128, Eight = 256, Nine = 512, Ten = 1024, Elevent = 2048, Twelve = 4096, Thirteen = 8192, Fourteen = 16384, Fifteen = 32768 }
