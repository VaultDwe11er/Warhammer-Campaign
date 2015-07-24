using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using HexUtilities;
using Terrain;

namespace Misc
{
    public class Castle : MapObject
    {        
        //Static members
        public static String ConfigPath = @"Config\castle.xml";
        public static String[] WallLevels;
        public static String[] WallLevelCosts;
        public static String[] TowerLevels;
        public static String[] TowerLevelCosts;
        public static String[] GatehouseLevels;
        public static String[] GatehouseLevelCosts;

        //Instance members
        private int _level;
        private bool _levelChanged;
        private String _wallLevel;
        private String _towerLevel;
        private String _gatehouseLevel;

        private String _imgPath;
        private Bitmap _bmp;

        private String _xPath;
        private String _xPathBase;

        public Castle(String name, int x, int y, String imgPath, String player)
            : this(name, 1, false, x, y, imgPath, player, WallLevels[0], TowerLevels[0], GatehouseLevels[0])
        {
            Save();
        }

        public Castle(String name, int level, bool levelChanged, int x, int y, String imgPath, String player, String wallLevel, String towerLevel, String gatehouseLevel)
        {
            Name = name;
            _level = level;
            _wallLevel = wallLevel;
            _towerLevel = towerLevel;
            _gatehouseLevel = gatehouseLevel;

            Coords = HexCoords.NewUserCoords(x, y);
            _imgPath = imgPath;
            _bmp = new Bitmap(imgPath);

            _xPath = "Players/Player[Name=\"" + player + "\"]/Buildings/Castle[Name=\"" + name + "\"]";
            _xPathBase = "Players/Player[Name=\"" + player + "\"]/Buildings";

            ObjectType = "Castle";
        }

        public override int LOS
        {
            get
            {
                return Math.Max(_level, 6);
            }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; Save(); }
        }

        public bool LevelChanged
        {
            get { return _levelChanged; }
            set { _levelChanged = value; Save(); }
        }

        public int GarrisonSize
        {
            get { return _level * 200; }
        }

        public bool IsUnderSiege { get; set; }

        public String Walls
        {
            get { return _wallLevel; }
            set { _wallLevel = value; Save(); }
        }

        public String Towers
        {
            get { return _towerLevel; }
            set { _towerLevel = value; Save(); }
        }

        public String Gatehouse
        {
            get { return _gatehouseLevel; }
            set { _gatehouseLevel = value; Save(); }
        }

        public void Upgrade(int levels)
        {
            if (_level + levels > 20)
            {
                MessageBox.Show("Castle can't be upgraded higher than level 20");
            }
            else
            {
                _level += levels;
                _levelChanged = true;
                Save();
            }
        }

        public void Downgrade(int levels)
        {
            if (_level - levels <= 0)
            {
                MessageBox.Show("Town with level " + _level.ToString() + " can not be downgraded by " + levels.ToString() + " levels");
            }
            else
            {
                _level -= levels;
                Save();
            }
        }

        public List<TerrainGridHex> ResourceHexes(TerrainMap tm)
        {
            int range = _level;

            List<TerrainGridHex> retVal = new List<TerrainGridHex>();
            List<TerrainGridHex> hexes;

            retVal.Add((TerrainGridHex)tm.BoardHexes[Coords]);

            for (int i = 1; i <= range; i++)
            {
                hexes = new List<TerrainGridHex>();
                hexes.AddRange(retVal);
                foreach (TerrainGridHex hex in hexes)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        Hexside hs = (Hexside)j;
                        TerrainGridHex nhex = (TerrainGridHex)hex.Neighbour(hs);

                        if (!retVal.Contains(nhex) && nhex.IsOnboard()) retVal.Add(nhex);
                    }
                }
            }

            return retVal;
        }

        public override void Save()
        {
            XDocument xDoc = XDocument.Load(Player.SavePath);

            if (xDoc.XPathSelectElements(_xPath).Elements().Count() == 0)
            {
                XElement xElem = new XElement("Castle");
                xElem.Add(new XElement("Name", Name));
                xElem.Add(new XElement("Level", _level));
                xElem.Add(new XElement("LevelChanged", _levelChanged));
                xElem.Add(new XElement("ImagePath", _imgPath));
                xElem.Add(new XElement("X", Coords.User.X));
                xElem.Add(new XElement("Y", Coords.User.Y));
                xElem.Add(new XElement("WallLevel", _wallLevel));
                xElem.Add(new XElement("TowerLevel", _towerLevel));
                xElem.Add(new XElement("GatehouseLevel", _gatehouseLevel));
                xElem.Add(new XElement("IsInBattle", IsInBattle));
                xElem.Add(new XElement("IsUnderSiege", IsUnderSiege));

                xDoc.XPathSelectElement(_xPathBase).Add(xElem);
            }
            else
            {
                xDoc.XPathSelectElement(_xPath).Element("Level").Value = _level.ToString();
                xDoc.XPathSelectElement(_xPath).Element("LevelChanged").Value = _levelChanged.ToString();
                xDoc.XPathSelectElement(_xPath).Element("ImagePath").Value = _imgPath;
                xDoc.XPathSelectElement(_xPath).Element("X").Value = Coords.User.X.ToString();
                xDoc.XPathSelectElement(_xPath).Element("Y").Value = Coords.User.Y.ToString();
                xDoc.XPathSelectElement(_xPath).Element("WallLevel").Value = _wallLevel;
                xDoc.XPathSelectElement(_xPath).Element("TowerLevel").Value = _towerLevel;
                xDoc.XPathSelectElement(_xPath).Element("GatehouseLevel").Value = _gatehouseLevel;
                xDoc.XPathSelectElement(_xPath).Element("IsInBattle").Value = IsInBattle.ToString();
                xDoc.XPathSelectElement(_xPath).Element("IsUnderSiege").Value = IsUnderSiege.ToString();
            }

            xDoc.Save(Player.SavePath);
        }

        public override void Paint(Graphics g, TerrainMap tm)
        {
            if (tm.IsOnboard(Coords) && tm.Los[Coords])
            {
                var container = g.BeginContainer();

                tm.TranslateGraphicsToHex(g, Coords);
                g.DrawImage(_bmp, tm.BoardHexes[Coords].Board.HexgridPath.GetBounds());
                if (tm.ShowHexgrid) g.DrawPath(Pens.Black, tm.HexgridPath);

                g.EndContainer(container);
            }
        }
    }
}
