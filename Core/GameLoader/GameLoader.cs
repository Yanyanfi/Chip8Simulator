namespace Core.GameLoader;

public sealed class GameLoader : IGameLoader
{
    ///<exception cref="GameFileNotFoundException"/>
    public async Task LoadAsync(byte[] memory, string filePath)
    {
        GameFileNotFoundException.ThrowIfNotExists(filePath);
        var data =await File.ReadAllBytesAsync(filePath);
        data.CopyTo(memory, 0x200);
    }
}
