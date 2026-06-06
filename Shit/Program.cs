namespace Shit;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static async Task Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        var s0 = SynchronizationContext.Current;
        var form = new Form1();
        var s = SynchronizationContext.Current?.ToString();
        await Task.Delay(1).ConfigureAwait(false);
        var s1 = SynchronizationContext.Current?.ToString();
        await form.SetFieldAsync(111);
        Application.Run(form);
    }    
}