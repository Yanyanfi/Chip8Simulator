using Core.Decoder;
using Core.GameLoader;
using Core.Input;
using Core.MemoryInitializer;
namespace Core.VirtualMachine;

public sealed class VirtualMachine(IDecoder decoder, IMemoryInitializer initializer, IGameLoader loader)
{
    private readonly IMemoryInitializer _memoryInitializer = initializer;
    private readonly IGameLoader _gameLoader = loader;
    private readonly IDecoder _decoder = decoder;

    #region VMState
    private readonly byte[] _dataRegisters = new byte[16];
    private ushort _iRegister;
    private ushort _pc;
    private byte _sp;
    private byte _delayTimer;
    private byte _soundTimer;
    private readonly byte[] _memory = new byte[4096];
    private readonly ushort[] _stack = new ushort[16];
    private readonly bool[,] _display = new bool[32, 64];
    private readonly Keypad _keypad = new();
    private bool _isWaitingKeyPress = false;
    private bool _isWaitingKeyRelease = false;

    #endregion

    #region Settings
    private double _tpi = 1000 / 700d;
    private static readonly double _timerTickTime = 1000 / 60d;
    #endregion
    private double _currentTimerTickTime = 0d;
    private double _currentInstrTickTime = 0d;
    public bool IsDelayActive => _delayTimer > 0;
    public bool IsSoundActive => _soundTimer > 0;
    public IEnumerable<bool[]> Display => _display.Cast<bool>().Chunk(64);

    public void Initialize(double ips)
    {
        Initialize();
        _tpi = 1000 / ips;
    }
    public void Initialize()
    {
        _memoryInitializer.Initialize(_memory);
        _pc = 0x200;
        _sp = byte.MaxValue;
    }
    public async Task LoadGameAsync(string path) => await _gameLoader.LoadAsync(_memory, path);
    public void SetInput(Key key) => _keypad.Key = key;
    ///<exception cref="UnsupportedInstructionException"/>
    ///<exception cref="MemoryOverflowException"/>
    public void Update(double deltaTime)
    {
        deltaTime = Math.Min(deltaTime, 1000);
        _currentTimerTickTime += deltaTime;
        _currentInstrTickTime += deltaTime;
        var timerCount = (int)(_currentTimerTickTime / _timerTickTime);
        var instrCount = (int)(_currentInstrTickTime / _tpi);
        _currentTimerTickTime -= timerCount * _timerTickTime;
        _currentInstrTickTime -= instrCount * _tpi;
        _soundTimer = (byte)int.Max(0, _soundTimer - timerCount);
        _delayTimer = (byte)int.Max(0, _delayTimer - timerCount);
        for (int i = 0; i < instrCount; i++)
        {
            Tick();
        }
    }
    ///<exception cref="UnsupportedInstructionException"/>
    ///<exception cref="MemoryOverflowException"/>
    private void Tick()
    {
        //ProcessTimer(deltaTime);
        if (_pc >= _memory.Length)
            throw new MemoryOverflowException();
        var instruction = SplitInstruction(_memory[_pc], _memory[_pc + 1]);
        _decoder.Decode(instruction, _memory, _stack, _dataRegisters, _display, ref _iRegister, ref _pc, ref _sp, ref _delayTimer, ref _soundTimer, _keypad, ref _isWaitingKeyPress, ref _isWaitingKeyRelease);
    }
    private static (byte, byte, byte, byte) SplitInstruction(byte hi, byte lo) => ((byte)(hi >> 4), (byte)(hi & 0x0F), (byte)(lo >> 4), (byte)(lo & 0x0F));


}
