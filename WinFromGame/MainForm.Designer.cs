namespace WinFromGame;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        chip8DisplayControl1 = new Chip8DisplayControl();
        SuspendLayout();
        // 
        // chip8DisplayControl1
        // 
        chip8DisplayControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        chip8DisplayControl1.BackgroundColor = Color.Black;
        chip8DisplayControl1.ForegroundColor = Color.White;
        chip8DisplayControl1.Location = new Point(12, 12);
        chip8DisplayControl1.Name = "chip8DisplayControl1";
        chip8DisplayControl1.Size = new Size(516, 258);
        chip8DisplayControl1.TabIndex = 0;
        chip8DisplayControl1.Text = "chip8DisplayControl1";
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(540, 283);
        Controls.Add(chip8DisplayControl1);
        Name = "MainForm";
        Text = "Chip-8";
        ResumeLayout(false);
    }

    #endregion

    private Chip8DisplayControl chip8DisplayControl1;
}
