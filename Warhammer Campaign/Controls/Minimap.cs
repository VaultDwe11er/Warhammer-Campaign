using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warhammer_Campaign
{
    class Minimap : PictureBox
    {
        public Rectangle MinimapRect { get; set; }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            if (this.Image != null) g.DrawImageUnscaled(this.Image, new Point(0, 0));
            if (MinimapRect != null) g.DrawRectangle(Pens.Red, MinimapRect);
        }
    }
}
