namespace ConsoleGame.Display;

internal sealed class GameDrawer : IGameDrawer
{
    public void Draw(IEnumerable<bool[]> pixels)
    {
        Console.SetCursorPosition(0, 0);
        foreach (var line in pixels)
        {
            foreach (var pixel in line)
            {
                Console.Write(pixel ? "█" : " ");
            }
            Console.WriteLine();
        }
    }
}
