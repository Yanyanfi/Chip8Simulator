using Abstraction;
using ConsoleGame;
using ConsoleGame.Display;
using ConsoleGame.Exceptions;
using Core.Decoder;
using Core.GameLoader;
using Core.Interfaces;
using Core.Memory;
using Core.MemoryInitializer;
using Core.VirtualMachine;
using Microsoft.Extensions.DependencyInjection;
using Platform;
using System.Diagnostics;

var collection = new ServiceCollection();
collection.AddSingleton<IDecoder, Decoder>();
collection.AddSingleton<IGameLoader, GameLoader>();
collection.AddSingleton<MemoryInitializer>();
collection.AddSingleton<IFontAddressProvider>(sp => sp.GetService<MemoryInitializer>() ?? throw new ServiceNotFoundException<MemoryInitializer>());
collection.AddSingleton<IMemoryInitializer>(sp => sp.GetService<MemoryInitializer>() ?? throw new ServiceNotFoundException<MemoryInitializer>());
collection.AddSingleton<VirtualMachine>();
collection.AddSingleton<IInput, KeyboardInput>();
collection.AddSingleton<IBuzzer, Buzzer>();
collection.AddSingleton<IGameDrawer, GameDrawer>();
collection.AddSingleton<Game>();
string localPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Chip8Simulator");
if (!Path.Exists(localPath))
    Directory.CreateDirectory(localPath);
collection.AddSingleton<ILogger, Logger>(sp=>new(Path.Join(localPath,"vm.log")));

var sp = collection.BuildServiceProvider();
var game = sp.GetService<Game>()??throw new ServiceNotFoundException<Game>();
game.Initialize(60,700);
await game.LoadAsync(args[0]);
game.StartGame();