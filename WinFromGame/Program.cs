using Abstraction;
using Core.Decoder;
using Core.GameLoader;
using Core.Logger;
using Core.Memory;
using Core.MemoryInitializer;
using Core.VirtualMachine;
using Microsoft.Extensions.DependencyInjection;
using Platform;
using WinFromGame;

namespace WinFormGame;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(params string[] args)
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var collection = new ServiceCollection();
        collection.AddSingleton<IDecoder, Decoder>();
        collection.AddSingleton<IGameLoader, GameLoader>();
        collection.AddSingleton<MemoryInitializer>();
        collection.AddSingleton<IFontAddressProvider>(sp => sp.GetService<MemoryInitializer>() ?? throw new Exception());
        collection.AddSingleton<IMemoryInitializer>(sp => sp.GetService<MemoryInitializer>() ?? throw new Exception());
        collection.AddSingleton<VirtualMachine>();
        collection.AddSingleton<IInput, KeyboardInput>();
        collection.AddSingleton<IBuzzer, Buzzer>();
        collection.AddSingleton<Game>();
        string localPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Chip8Simulator");
        if (!Path.Exists(localPath))
            Directory.CreateDirectory(localPath);
        collection.AddSingleton<ILogger, Logger>(sp => new(Path.Join(localPath, "vm.log")));

        var path = args[0];
        collection.AddSingleton<MainForm>(sp => new MainForm(sp.GetService<VirtualMachine>()!, sp.GetService<IInput>()!, sp.GetService<IBuzzer>()!, path));

        var sp = collection.BuildServiceProvider();
        var form = sp.GetService<MainForm>() ?? throw new Exception();

        form.Initialize();

        Application.Run(form);
    }
}