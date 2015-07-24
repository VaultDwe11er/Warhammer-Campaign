using HexUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terrain;

namespace Misc
{
    public abstract class MapObject
    {
        public String ObjectType { get; set; }
        public HexCoords Coords { get; set; }
        public String Name { get; set; }
        public bool IsInBattle { get; set; }
        public abstract int LOS { get; }

        public abstract void Paint(Graphics g, TerrainMap tm);
        public abstract void Save();
    }
}
