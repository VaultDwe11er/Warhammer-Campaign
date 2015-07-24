using HexUtilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Terrain;

namespace Misc
{
    public class Watchtower : MapObject
    {
        private int _garrisonSize;

        private String _imgPath;
        private Bitmap _bmp;

        private String _xPath;
        private String _xPathBase;

        public Watchtower(String name, int x, int y, String player, String imgPath)
            : this(name, x, y, player, imgPath, 500)
        {
            Save();
        }

        public Watchtower(String name, int x, int y, String player, String imgPath, int gs)
        {
            _garrisonSize = gs;

            Name = name;
            ObjectType = "Watchtower";
            Coords = HexCoords.NewUserCoords(x, y);

            _xPath = "Players/Player[Name=\"" + player + "\"]/Buildings/Watchtower[Name=\"" + name + "\"]";
            _xPathBase = "Players/Player[Name=\"" + player + "\"]/Buildings";

            _imgPath = imgPath;
            _bmp = new Bitmap(imgPath);
        }

        public override int LOS
        {
            get
            {
                return 8;
            }
        }

        public int GarrisonSize
        {
            get { return _garrisonSize; }
        }

        public override void Save()
        {
            XDocument xDoc = XDocument.Load(Player.SavePath);

            if (xDoc.XPathSelectElements(_xPath).Elements().Count() == 0)
            {
                XElement xElem = new XElement("Watchtower");
                xElem.Add(new XElement("Name", Name));
                xElem.Add(new XElement("GarrisonSize", _garrisonSize));
                xElem.Add(new XElement("ImagePath", _imgPath));
                xElem.Add(new XElement("X", Coords.User.X));
                xElem.Add(new XElement("Y", Coords.User.Y));

                xDoc.XPathSelectElement(_xPathBase).Add(xElem);
            }
            else
            {
                xDoc.XPathSelectElement(_xPath).Element("GarrisonSize").Value = _garrisonSize.ToString();
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
