using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexUtilities;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Terrain;
using Custom.HexgridPanel;
using HexUtilities.Common;

namespace Misc
{
    public class Town : MapObject
    {
        //Static definitions
        public static String ConfigPath = @"Config\town.xml";
        public static String[] CoverLevels;
        public static String[] CoverLevelCosts;
        public static String[] WallLevels;
        public static String[] WallLevelCosts;
        public static String[] TowerLevels;
        public static String[] TowerLevelCosts;

        public static String[] MiscUpgrades = { "Burning Oil", "Warmachines" };

        //Instance definitions
        private int _level;
        private bool _levelChanged;
        private String _coverLevel;
        private String _wallLevel;
        private String _towerLevel;
        //private bool[] _miscUpgrades;

        private String _imgPath;
        private Bitmap _bmp;

        private String _xPath;
        private String _xPathBase;

        public Town(String name, int x, int y, String imgPath, String player)
            : this(name, 1, x, y, imgPath, player, CoverLevels[0], WallLevels[0], TowerLevels[0], false)
        {
            Save();
        }

        public Town(String name, int level, int x, int y, String imgPath, String player, String coverLevel, String walLevel, String towerLevel, bool levelChanged)
        {
            Name = name;
            _level = level;
            _levelChanged = levelChanged;
            _coverLevel = coverLevel;
            _wallLevel = walLevel;
            _towerLevel = towerLevel;
            //_miscUpgrades = miscUpgradesArr;

            Coords = HexCoords.NewUserCoords(x, y);
            _imgPath = imgPath;
            _bmp = new Bitmap(imgPath);

            _xPath = "Players/Player[Name=\"" + player + "\"]/Buildings/Town[Name=\"" + name + "\"]";
            _xPathBase = "Players/Player[Name=\"" + player + "\"]/Buildings";

            ObjectType = "Town";
        }

        public override int LOS
        {
            get
            {
                return Math.Max(_level, 4);
            }
        }

        public int Level
        {
            get { return _level; }
        }

        public bool LevelChanged
        {
            get { return _levelChanged; }
            set { _levelChanged = value; Save(); }
        }

        public Bitmap Bmp
        {
            get { return _bmp; }
        }

        public String Cover
        {
            get { return _coverLevel; }
            set { _coverLevel = value; Save(); }
        }

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

        public int GarrisonSize
        {
            get { return _level * 100; }
        }

        public bool IsUnderSiege { get; set; }
        
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
        
        public void Upgrade(int levels)
        {
            if (_level + levels > 20)
            {
                MessageBox.Show("Town can't be upgraded higher than level 20");
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

        public override void Save()
        {
            XDocument xDoc = XDocument.Load(Player.SavePath);

            if (xDoc.XPathSelectElements(_xPath).Elements().Count() == 0)
            {
                XElement xElem = new XElement("Town");
                xElem.Add(new XElement("Name", Name));
                xElem.Add(new XElement("Level", _level));
                xElem.Add(new XElement("LevelChanged", _levelChanged));
                xElem.Add(new XElement("CoverLevel", _coverLevel));
                xElem.Add(new XElement("WallLevel", _wallLevel));
                xElem.Add(new XElement("TowerLevel", _towerLevel));
                xElem.Add(new XElement("IsInBattle", IsInBattle));
                xElem.Add(new XElement("ImagePath", _imgPath));
                xElem.Add(new XElement("X", Coords.User.X));
                xElem.Add(new XElement("Y", Coords.User.Y));

                xDoc.XPathSelectElement(_xPathBase).Add(xElem);
            }
            else
            {
                xDoc.XPathSelectElement(_xPath).Element("Level").Value = _level.ToString();
                xDoc.XPathSelectElement(_xPath).Element("LevelChanged").Value = _levelChanged.ToString();
                xDoc.XPathSelectElement(_xPath).Element("CoverLevel").Value = _coverLevel;
                xDoc.XPathSelectElement(_xPath).Element("WallLevel").Value = _wallLevel;
                xDoc.XPathSelectElement(_xPath).Element("TowerLevel").Value = _towerLevel;
                xDoc.XPathSelectElement(_xPath).Element("IsInBattle").Value = IsInBattle.ToString();
                xDoc.XPathSelectElement(_xPath).Element("ImagePath").Value = _imgPath;
                xDoc.XPathSelectElement(_xPath).Element("X").Value = Coords.User.X.ToString();
                xDoc.XPathSelectElement(_xPath).Element("Y").Value = Coords.User.Y.ToString();
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
