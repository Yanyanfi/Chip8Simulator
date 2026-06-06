namespace Shit;

public partial class Form1 : Form
{
    private int _field;
    public Form1()
    {
        InitializeComponent();
    }
    public async Task SetFieldAsync(int value)
    {
        await Task.Delay(100);
        _field = value;
    }
}
