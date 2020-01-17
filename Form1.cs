using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace anaclo
{
    public partial class Form1 : Form
    {
        private int _radius;
        private Point _origin;
        private Dictionary<string, Pen> _pens = new Dictionary<string, Pen>();
        private Rectangle _circleBounds;

        public Form1()
        {
            InitializeComponent();
            Paint += new PaintEventHandler(DrawClock);
            WinApi.MakeDraggable(this);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Lime;
            this.TransparencyKey = Color.Lime;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this._radius = ClientSize.Width / 2;
            this._origin = new Point(_radius, _radius);
            _pens.Add("t", Pens.Gray);
            _pens.Add("s", Pens.Red);
            _pens.Add("m", new Pen(Brushes.Black, 3));
            _pens.Add("h", new Pen(Brushes.Black, 6));
            var r = 5;
            _circleBounds = new Rectangle(_origin.X - 5, _origin.Y - r, r * 2, r * 2);
            Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void DrawClock(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            g.FillEllipse(Brushes.Gainsboro, this.ClientRectangle);

            for (var m = 0; m < 60; m++)
                g.DrawLine(_pens["t"], GetCoords(m, 60, m % 5 == 0 ? 0.9 : 0.98), GetCoords(m, 60));

            var now = DateTime.Now;
            g.DrawLine(_pens["h"], _origin, GetCoords(now.Hour, 12, 0.85));
            g.DrawLine(_pens["m"], _origin, GetCoords(now.Minute, 60));
            g.DrawLine(_pens["s"], _origin, GetCoords(now.Second, 60));
            g.FillEllipse(Brushes.Black, _circleBounds);
        }

        private Point GetCoords(int value, int max, double scale = 1)
        {
            var angle = value * 2 * Math.PI / max - 2 * Math.PI / 4;
            var x = (int)(Math.Cos(angle) * _radius * scale) + _radius;
            var y = (int)(Math.Sin(angle) * _radius * scale) + _radius;
            return new Point(x, y);
        }
    }
}
