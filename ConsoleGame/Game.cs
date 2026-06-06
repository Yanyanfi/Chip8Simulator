using Abstraction;
using ConsoleGame.Display;
using ConsoleGame.Exceptions;
using Core.Input;
using Core.VirtualMachine;
using System.Diagnostics;
namespace ConsoleGame;

internal sealed class Game(VirtualMachine machine, IInput input, IBuzzer buzzer, IGameDrawer drawer)
{
    private readonly VirtualMachine _virtualMachine = machine;
    private readonly IInput _input = input;
    private readonly IBuzzer _buzzer = buzzer;
    private readonly IGameDrawer _drawer = drawer;
    private double _timePerFrame = 1000 / 60d;
    //private static readonly double _timerTickPerSecond = 60;
    //private double _currentFrameTime = 0d;
    //private double _currentInstructionTime = 0d;
    private void Update(double deltaTime)
    {
        _virtualMachine.SetInput(_input.GetPressedKey());
        _virtualMachine.Update(deltaTime);
        _virtualMachine.SetInput(Key.None);
        if (_virtualMachine.IsSoundActive)
            _buzzer.Beep((int)deltaTime);
        _drawer.Draw(_virtualMachine.Display);

    }
    public async Task LoadAsync(string path) => await _virtualMachine.LoadGameAsync(path);
    public void Initialize(double fps, double ips)
    {
        _virtualMachine.Initialize(ips);
        _timePerFrame = 1000 / fps;
    }
    public void StartGame()
    {
        var stopwatch = Stopwatch.StartNew();
        var currentFrameTime = 0d;
        while (true)
        {
            var lastFrameTime = currentFrameTime;
            currentFrameTime = stopwatch.Elapsed.TotalMilliseconds;
            var deltaTime = currentFrameTime - lastFrameTime;
            Update(deltaTime);
            while (stopwatch.Elapsed.TotalMilliseconds - currentFrameTime < _timePerFrame) ;
        }
    }
}
