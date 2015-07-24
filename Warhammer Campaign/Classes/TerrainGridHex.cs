using System;
using System.Drawing;
using Custom.HexgridPanel;
using HexUtilities;
using Misc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Terrain
{
    public class TerrainGridHex : MapGridHex
    {
        //static definitions
        public static String ConfigPath = @"Config\tiles.xml";
        public static String SavePath = @"Save\tiles.xml";
        public static String UndoPath = @"Save\tiles_bak.xml";
        public static Bitmap _bmpRoad;
        
        public static Bitmap BmpRoad
        {
            get
            {
                if (_bmpRoad == null) _bmpRoad = new Bitmap(@"Terrain\Possible Underground Tiles\path1.png");
                return _bmpRoad;
            }
            set { _bmpRoad = value; }
        }

        //Instance definitions
        private Bitmap _bmpSummer;
        private Bitmap _bmpWinter;
        private double _stepCost;
        private int _elevation;
        private String _specialType;
        //String _imgPath;

        public TerrainGridHex(HexBoard<MapGridHex> board, HexCoords coords, String type, String imgSummer, String imgWinter, double stepCost, int elevation, List<Resource> resources)
            : base(board, coords)
        {
            Type = type;
            _specialType = "";
            _stepCost = stepCost;
            _elevation = elevation;
            IsSummer = true;
            Resources = resources;

            //TODO: improve logic
            while (_bmpSummer == null)
            {
                try
                {
                    _bmpSummer = new Bitmap(imgSummer);
                }
                catch (OutOfMemoryException)
                {
                }
            }

            while (_bmpWinter == null)
            {
                try
                {
                    _bmpWinter = new Bitmap(imgWinter);
                }
                catch (OutOfMemoryException)
                {
                }
            }
        }

        public String Type { get; private set; }
        public bool IsSummer { get; set; }
        public String SpecialType { get { return _specialType; } set { _specialType = value; Save(); } }
        public List<Resource> Resources { get; private set; }
        public Army SelectedArmy { get; set; }

        public override int Elevation { get { return _elevation; } }
        public override int ElevationASL { get { return Board.ElevationASL(Elevation); } }
        public override int HeightTerrain { get { return ElevationASL; } }

        public override double StepCost(Hexside direction)
        {
            if (SelectedArmy != null)
            {
                switch (SelectedArmy.ArmyType)
                {
                    case "Standard":
                    case "Mounstrous":
                    case "Mounted":
                        return SpecialType == "Road" ? _stepCost / 2 : _stepCost;
                    case "Scouting":
                        if (_stepCost == 1) return SpecialType == "Road" ? _stepCost / 2 : _stepCost;
                        else return SpecialType == "Road" ? (_stepCost - 1) / 2 : (_stepCost - 1);
                    case "Flying":
                        return 1;
                    default:
                        return SpecialType == "Road" ? _stepCost / 2 : _stepCost;
                }
            }

            return SpecialType == "Road" ? _stepCost / 2 : _stepCost;
        }
        
        public bool Visible
        {
            get { return ((TerrainMap)Board).Los[this.Coords]; }
        }

        public void Save()
        {
            XDocument xDoc = XDocument.Load(TerrainGridHex.SavePath);
            String xPath = "Tiles/Tile[@X=\"" + Coords.User.X + "\" and @Y=\"" + Coords.User.Y + "\"]";

            if (SpecialType != "")
            {
                if (xDoc.XPathSelectElements(xPath).Attributes().Count() == 0)
                {
                    XElement xElem = new XElement("Tile");
                    xElem.Add(new XAttribute("X", Coords.User.X));
                    xElem.Add(new XAttribute("Y", Coords.User.Y));
                    xElem.Add(new XAttribute("Type", SpecialType));

                    xDoc.XPathSelectElement("Tiles").Add(xElem);
                }
                else
                {
                    xDoc.XPathSelectElement(xPath).Attribute("Type").Value = SpecialType;
                }
            }
            else
            {
                if (xDoc.XPathSelectElements(xPath).Elements().Count() > 0)
                {
                    xDoc.XPathSelectElement(xPath).Remove();
                }
            }

            xDoc.Save(TerrainGridHex.SavePath);
        }

        public override void Paint(Graphics g)
        {
            if (SpecialType == "Road")
            {
                if (BmpRoad.Width != (int)Board.HexgridPath.GetBounds().Width || BmpRoad.Height != (int)Board.HexgridPath.GetBounds().Height)
                    BmpRoad = new Bitmap(BmpRoad, (int)Board.HexgridPath.GetBounds().Width, (int)Board.HexgridPath.GetBounds().Height);

                g.DrawImageUnscaled(BmpRoad, 0, 0);
            }
            else
            {
                if (_bmpSummer.Width != (int)Board.HexgridPath.GetBounds().Width || _bmpSummer.Height != (int)Board.HexgridPath.GetBounds().Height)
                    _bmpSummer = new Bitmap(_bmpSummer, (int)Board.HexgridPath.GetBounds().Width, (int)Board.HexgridPath.GetBounds().Height);

                if (_bmpWinter.Width != (int)Board.HexgridPath.GetBounds().Width || _bmpWinter.Height != (int)Board.HexgridPath.GetBounds().Height)
                    _bmpWinter = new Bitmap(_bmpWinter, (int)Board.HexgridPath.GetBounds().Width, (int)Board.HexgridPath.GetBounds().Height);

                if (IsSummer) g.DrawImageUnscaled(_bmpSummer, 0, 0);
                else g.DrawImageUnscaled(_bmpWinter, 0, 0);
            }
        }
    }
}
