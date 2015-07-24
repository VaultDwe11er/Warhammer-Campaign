using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Custom.HexgridPanel;

namespace Warhammer_Campaign
{
    public class CustomHexgridPanel : HexgridPanel
    {
        static readonly Matrix TransposeMatrix = new Matrix(0F, 1F, 1F, 0F, 0F, 0F);
        Bitmap _mapBuffer;

        public CustomHexgridPanel(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            container.Add(this);

            InitializeComponent();
        }

        public Bitmap MapBuffer
        {
            get { return _mapBuffer ?? (_mapBuffer = PaintBuffer()); }
            //get { return PaintBuffer(); }
            set { if (_mapBuffer != null) _mapBuffer.Dispose(); _mapBuffer = value; }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.ResumeLayout(false);
        }

        Bitmap PaintBuffer()
        {
            var size = MapSizePixels;

            Bitmap buffer = null;
            Bitmap tempBuffer = null;
            try
            {
                tempBuffer = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb);
                using (var g = Graphics.FromImage(tempBuffer))
                {
                    g.Clear(Color.Black);
                    g.TranslateTransform(MapMargin.Width, MapMargin.Height);
                    Host.PaintMap(g);
                }
                buffer = tempBuffer;
                tempBuffer = null;
            }
            finally { if (tempBuffer != null) tempBuffer.Dispose(); }
            return buffer;
        }

        protected override void PaintPanel(Graphics g)
        {
            if (g == null) throw new ArgumentNullException("g");
            var scroll = Hexgrid.ScrollPosition;
            if (DesignMode) { g.FillRectangle(Brushes.Gray, ClientRectangle); return; }

            g.Clear(Color.Black);

            if (IsTransposed) { g.Transform = TransposeMatrix; }
            g.TranslateTransform(scroll.X, scroll.Y);
            g.ScaleTransform(MapScale, MapScale);

            var state = g.Save();
            g.DrawImageUnscaled(MapBuffer, Point.Empty);

            g.Restore(state); state = g.Save();
            Host.PaintUnits(g);

            g.Restore(state); state = g.Save();
            Host.PaintHighlight(g);
        }
    }
}