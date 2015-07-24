using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using HexUtilities;
using HexUtilities.Pathfinding;
using Terrain;

namespace Misc
{
    public class Army : MapObject
    {
        private String _armyType;
        private double _moveTotal;
        private double _moveLeft;
        private Pen _color;
        private int _size;
        private MapObject _unitAttacked;
        private bool _canBuild;
        private List<Resource> _upkeepCost;

        private String _imgPath;
        private Bitmap _bmp;

        private String _xPath;
        private String _xPathBase;

        //New army created
        public Army(int x, int y, String name, String type, double movement, Pen color, int size, String player, bool canBuild, String imgPath, List<Resource> upkeepCost)
            : this(x, y, name, type, movement, 0, color, size, player, canBuild, imgPath, upkeepCost)
        {
            Save();
        }

        //Load army
        public Army(int x, int y, String name, String type, double moveTotal, double moveLeft, Pen color, int size, String player, bool canBuild, String imgPath, List<Resource> upkeepCost)
        {
            Coords = HexCoords.NewUserCoords(x, y);
            Name = name;
            _armyType = type;
            _moveTotal = moveTotal;
            _color = color;
            _moveLeft = moveLeft;
            _size = size;
            IsInBattle = false;
            _canBuild = canBuild;
            _upkeepCost = upkeepCost;

            _imgPath = imgPath;
            _bmp = new Bitmap(imgPath);

            _xPath = "Players/Player[Name=\"" + player + "\"]/Armies/Army[Name=\"" + name + "\"]";
            _xPathBase = "Players/Player[Name=\"" + player + "\"]/Armies";

            ObjectType = "Army";
        }

        public override int LOS
        {
            get
            {
                return (int)Math.Floor(_moveTotal);
            }
        }

        public String ArmyType
        {
            get { return _armyType; }
        }

        public int Size
        {
            get { return _size; }
            set
            {
                _upkeepCost.ForEach(r => r.Value = r.Value * value / _size);
                _size = value;
            }
        }

        public double Movement
        {
            get { return _moveLeft; }
        }
        
        public Pen Color
        {
            get { return _color; }
        }

        public String UnitAttacked
        {
            get { return _unitAttacked == null ? null : _unitAttacked.Name; }
        }

        public bool CanBuild
        {
            get { return _canBuild; }
        }

        public bool CanBuildRoad(TerrainMap tm)
        {
            for (int i = 0; i <= 5; i++)
            {
                Hexside hs = (Hexside)i;
                TerrainGridHex nhex = (TerrainGridHex)tm.BoardHexes[Coords].Neighbour(hs);

                Town t = tm.GetTown(nhex.Coords);
                Castle c = tm.GetCastle(nhex.Coords);

                if (nhex.SpecialType == "Road" || t != null || c != null) return true;
            }

            return false;
        }

        public bool IsSieging { get; set; }
        
        public List<Resource> UpkeepCost
        {
            //get { return _upkeepCost.Select(r => new Resource(r.Name, r.Value * _size / 100)).ToList(); }
            get { return _upkeepCost; }
        }

        public void Move(HexCoords toCoords, TerrainMap tm)
        {
            Landmark lm = new Landmark(Coords, tm);
            double distance = lm.HexDistance(toCoords);
            Coords = toCoords;
            _moveLeft -= distance;

            Save();
        }

        public void ResetMovement()
        {
            if (!IsInBattle)
            {
                _moveLeft = _moveTotal;
            }

            Save();
        }

        public void Attack(MapObject mo)
        {
            IsInBattle = true;
            mo.IsInBattle = true;
            _unitAttacked = mo;
            _moveLeft = 0;

            Save();
            mo.Save();
        }

        public void EndAttack()
        {
            IsInBattle = false;
            _unitAttacked.IsInBattle = false;

            _unitAttacked.Save();
            _unitAttacked = null;
            Save();
        }

        public void Siege(Town t)
        {
            IsSieging = true;
            t.IsUnderSiege = true;
            _unitAttacked = t;

            Save();
            t.Save();
        }

        public void Siege(Castle c)
        {
            IsSieging = true;
            c.IsUnderSiege = true;
            _unitAttacked = c;

            Save();
            c.Save();
        }

        public void EndSiege()
        {
            IsSieging = false;
            try { ((Castle)_unitAttacked).IsUnderSiege = false; }
            catch { }
            try { ((Town)_unitAttacked).IsUnderSiege = false; }
            catch { }

            _unitAttacked.Save();
            _unitAttacked = null;
            Save();
        }

        public override void Save()
        {
            XDocument xDoc = XDocument.Load(Player.SavePath);

            if (xDoc.XPathSelectElements(_xPath).Elements().Count() == 0)
            {
                XElement xElem = new XElement("Army");
                xElem.Add(new XElement("Name", Name));
                xElem.Add(new XElement("Type", ArmyType));
                xElem.Add(new XElement("MoveTotal", _moveTotal));
                xElem.Add(new XElement("MoveLeft", _moveLeft));
                xElem.Add(new XElement("Color", _color.Color.Name));
                xElem.Add(new XElement("Size", _size));
                xElem.Add(new XElement("IsInBattle", IsInBattle));
                xElem.Add(new XElement("IsSieging", IsSieging));
                xElem.Add(new XElement("UnitAttacked", UnitAttacked == null ? "" : UnitAttacked));
                xElem.Add(new XElement("CanBuild", _canBuild));
                xElem.Add(new XElement("X", Coords.User.X));
                xElem.Add(new XElement("Y", Coords.User.Y));
                xElem.Add(new XElement("ImgPath", _imgPath));
                XElement e = new XElement("Upkeep");
                if (_upkeepCost != null) _upkeepCost.ForEach(val => e.Add(new XAttribute(val.Name, val.Value)));
                xElem.Add(e);

                xDoc.XPathSelectElement(_xPathBase).Add(xElem);
            }
            else
            {
                xDoc.XPathSelectElement(_xPath).Element("Type").Value = _armyType;
                xDoc.XPathSelectElement(_xPath).Element("MoveTotal").Value = _moveTotal.ToString();
                xDoc.XPathSelectElement(_xPath).Element("MoveLeft").Value = _moveLeft.ToString();
                xDoc.XPathSelectElement(_xPath).Element("Color").Value = _color.Color.Name;
                xDoc.XPathSelectElement(_xPath).Element("Size").Value = _size.ToString();
                xDoc.XPathSelectElement(_xPath).Element("IsInBattle").Value = IsInBattle.ToString();
                xDoc.XPathSelectElement(_xPath).Element("IsSieging").Value = IsSieging.ToString();
                xDoc.XPathSelectElement(_xPath).Element("UnitAttacked").Value = (UnitAttacked == null ? "" : UnitAttacked);
                xDoc.XPathSelectElement(_xPath).Element("CanBuild").Value = _canBuild.ToString();
                xDoc.XPathSelectElement(_xPath).Element("X").Value = Coords.User.X.ToString();
                xDoc.XPathSelectElement(_xPath).Element("Y").Value = Coords.User.Y.ToString();
                xDoc.XPathSelectElement(_xPath).Element("ImgPath").Value = _imgPath;
            }

            xDoc.Save(Player.SavePath);
        }

        public void Remove()
        {
            XDocument xDoc = XDocument.Load(Player.SavePath);

            xDoc.XPathSelectElement(_xPath).Remove();

            xDoc.Save(Player.SavePath);
        }

        public override void Paint(Graphics g, TerrainMap tm)
        {
            if (tm.IsOnboard(Coords) && tm.Los[Coords])
            {
                var container = g.BeginContainer();
                var target = tm.CentreOfHex(Coords);

                tm.TranslateGraphicsToHex(g, Coords);
                g.DrawImage(_bmp, tm.BoardHexes[Coords].Board.HexgridPath.GetBounds());
                if (tm.ShowHexgrid) g.DrawPath(Pens.Black, tm.HexgridPath);

                g.EndContainer(container);
            }
        }
    }
}
