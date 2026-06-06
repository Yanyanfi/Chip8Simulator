
using Abstraction;
using Core.Input;
using Core.VirtualMachine;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace WinFromGame;

public partial class MainForm : Form
{
    private readonly VirtualMachine _virtualMachine;
    private readonly IInput _input;
    private readonly IBuzzer _buzzer;
    private double _frameTime = 1000 / 60d;
    private readonly string _path;
    public MainForm(VirtualMachine virtualMachine, IInput input, IBuzzer buzzer,string path)
    {
        _virtualMachine = virtualMachine;
        _input = input;
        _buzzer = buzzer;
        _path = path;
        InitializeComponent();
        DoubleBuffered = true;
    }
    private void GameLoop()
    {
        Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            var currentFrameTime = 0d;
            while (true)
            {
                var lastFrameTime = currentFrameTime;
                currentFrameTime = stopwatch.Elapsed.TotalMilliseconds;
                var deltaTime = currentFrameTime - lastFrameTime;
                Update(deltaTime);
                while (stopwatch.Elapsed.TotalMilliseconds - currentFrameTime < _frameTime)
                {
                    //await Task.Yield();
                }
            }
        });
        
    }
    private void Update(double deltaTime)
    {
        _virtualMachine.SetInput(_input.GetPressedKey());
        _virtualMachine.Update(deltaTime);
        _virtualMachine.SetInput(Key.None);
        if (_virtualMachine.IsSoundActive)
            _buzzer.Beep((int)deltaTime);
        var pixels = new bool[64*32];
        var index = 0;
        foreach(var line in _virtualMachine.Display)
        {
            line.CopyTo(pixels, index);
            index += 64;
        }
        chip8DisplayControl1.SetFrame(pixels);
        
    }
    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        await _virtualMachine.LoadGameAsync(_path);
        GameLoop();
    }
    public void Initialize(double fps, double ips)
    {
        _virtualMachine.Initialize(ips);
        _frameTime = 1000d / fps;
    }
    public void Initialize(double fps)
    {
        Initialize();
        _frameTime = 1000d / fps;
    }
    public void Initialize()
    {
        _virtualMachine.Initialize();
    }
    public async Task LoadGameAsync(string path) => await _virtualMachine.LoadGameAsync(path);
}
