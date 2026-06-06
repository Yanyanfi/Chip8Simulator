using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFromGame
{
    public partial class Chip8DisplayControl : Control
    {
        public Chip8DisplayControl()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            InitializeComponent();
            _rectangle.Width = Width / 64;
            _rectangle.Height = Height / 32;
        }
        //private readonly bool[] _pixels = new bool[64 * 32];
        private ReadOnlyMemory<bool> _pixels;
        private static readonly float _targetRatio = 2f;
        private bool _isResizing = false;
        private Rectangle _rectangle;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color ForegroundColor { get; set; } = Color.White;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color BackgroundColor { get; set; } = Color.Black;
        public void SetFrame(ReadOnlyMemory<bool> pixels)
        {
            _pixels = pixels;
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (_pixels.Length < 64 * 32)
                return;
            var g = pe.Graphics;
            g.Clear(BackgroundColor);
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    if (_pixels.Span[x * 64 + y])
                    {
                        _rectangle.X = _rectangle.Width * y;
                        _rectangle.Y = _rectangle.Height * x;
                        g.FillRectangle(new SolidBrush(ForegroundColor), _rectangle);
                    }
                }
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_isResizing)
                return;
            _isResizing = true;
            Height = (int)(Width / _targetRatio);
            _rectangle.Height = Height / 32;
            _rectangle.Width = Width / 64;
            _isResizing = false;
        }
    }
}
