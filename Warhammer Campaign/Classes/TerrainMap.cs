using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Custom.HexgridPanel;
using HexUtilities;
using HexUtilities.Pathfinding;
using Misc;
using System.Drawing.Drawing2D;
using HexUtilities.Common;
using HexUtilities.ShadowCasting;
using System.Drawing.Imaging;

namespace Terrain
{
    public class TerrainMap : MapDisplay<MapGridHex>
    {
        private List<MapObject> _objects = new List<MapObject>();
        private Army _selectedArmy;
        private Landmark _lm;
        private Fov _los;
        private XDocument tileSaveDoc;

        public TerrainMap()
            : base(_sizeHexes, (map, coords) => InitializeHex(map, coords))
        {
            _los = new Fov(this);
            tileSaveDoc = XDocument.Load(TerrainGridHex.SavePath);

            foreach(var e in tileSaveDoc.XPathSelectElements("Tiles/Tile"))
            {
                int x = int.Parse(e.Attribute("X").Value);
                int y = int.Parse(e.Attribute("Y").Value);

                ((TerrainGridHex)this.BoardHexes[HexCoords.NewUserCoords(x, y)]).SpecialType = e.Attribute("Type").Value;
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
          "CA2233:OperationsShouldNotOverflow", MessageId = "10*elevationLevel")]
        public override int ElevationASL(int elevationLevel) { return 10 * elevationLevel; }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
          "CA2233:OperationsShouldNotOverflow", MessageId = "2*range")]
        public override int Heuristic(int range) { return 2 * range; }

        #region static Board definition
        static List<string> _board = new List<string>() { "." };
        static Size _sizeHexes = new Size(_board[0].Length, _board.Count);
        static XDocument tileDoc = XDocument.Load(TerrainGridHex.ConfigPath);

        #endregion

        private static MapGridHex InitializeHex(HexBoard<MapGridHex> board, HexCoords coords)
        {
            
            char value = _board[coords.User.Y][coords.User.X];

            String xPath = "Tiles/Tile[@Char=\"" + value + "\"]";

            XElement e = tileDoc.XPathSelectElement(xPath);

            String Type = e.Attribute("Type").Value;
            String imgSummer = e.Attribute("SummerImage").Value;
            String imgWinter = e.Attribute("WinterImage").Value;
            int moveCost = int.Parse(e.Attribute("MoveCost").Value);
            int elevation = int.Parse(e.Attribute("Elevation").Value);
            List<Resource> resourceList = new List<Resource>();

            //Resources
            if (e.Attribute("Resources") != null)
            {
                String resList = e.Attribute("Resources").Value;

                String[] resArr = resList.Split(',');

                foreach (String val in resArr)
                {
                    String[] tmpArr = val.Split('=');

                    String _name = tmpArr[0];
                    int _amount = int.Parse(tmpArr[1]);

                    resourceList.Add(new Resource(_name, _amount));
                }
            }

            return new TerrainGridHex(board, coords, Type, imgSummer, imgWinter, moveCost, elevation, resourceList);
        }
        
        public List<Army> Armies
        {
            get
            {
                List<Army> tmp = (_objects.FindAll((val) => val.ObjectType == "Army")).Select((val) => (Army)val).ToList();
                return tmp;
            }
        }

        public List<Town> Towns
        {
            get
            {
                List<Town> tmp = (_objects.FindAll((val) => val.ObjectType == "Town")).Select((val) => (Town)val).ToList();
                return tmp;
            }
        }

        public List<Watchtower> Watchtowers
        {
            get
            {
                List<Watchtower> tmp = (_objects.FindAll((val) => val.ObjectType == "Watchtower")).Select((val) => (Watchtower)val).ToList();
                return tmp;
            }
        }

        public List<Castle> Castles
        {
            get
            {
                List<Castle> tmp = (_objects.FindAll((val) => val.ObjectType == "Castle")).Select((val) => (Castle)val).ToList();
                return tmp;
            }
        }

        public List<MapObject> Objects
        {
            get { return _objects; }
            set { _objects = value; }
        }

        public Fov Los
        {
            get { return _los; }
            set { _los = value;}
        }

        public bool ViewPrevious { get; set; }

        public void AddObject(MapObject o)
        {
            _objects.Add(o);
        }

        public Army SelectedArmy
        {
            get { return _selectedArmy; }
            set
            {
                _selectedArmy = value;
                if (value != null)
                {
                    this.BoardHexes.ForEach(hex => ((TerrainGridHex)hex).SelectedArmy = _selectedArmy);
                    _lm = new Landmark(_selectedArmy.Coords, this);
                    StartHex = _lm.Coords;
                }
                else
                {
                    _lm = null;
                    StartHex = HexCoords.EmptyUser;
                }
            }
        }

        public Landmark SelectedLandmark
        {
            get { return _lm; }
        }
        
        public bool InMovement(HexCoords coords)
        {
            return _lm.HexDistance(coords) <= SelectedArmy.Movement && _lm.HexDistance(coords) > 0;
        }

        public Army GetArmy(HexCoords coords)
        {
            return (Army)_objects.Find((val) => val.Coords == coords && val.ObjectType == "Army" && Los[coords]);
        }
        
        public Town GetTown(HexCoords coords)
        {
            return (Town)_objects.Find((val) => val.Coords == coords && val.ObjectType == "Town" && Los[coords]);
        }

        public Watchtower GetWatchtower(HexCoords coords)
        {
            return (Watchtower)_objects.Find((val) => val.Coords == coords && val.ObjectType == "Watchtower" && Los[coords]);
        }
        public Castle GetCastle(HexCoords coords)
        {
            return (Castle)_objects.Find((val) => val.Coords == coords && val.ObjectType == "Castle" && Los[coords]);
        }
        
        public MapObject GetBuilding(HexCoords coords)
        {
            return _objects.Find((val) => val.Coords == coords && new List<String>() { "Castle", "Town", "Watchtower" }.Contains(val.ObjectType) && Los[coords]);
        }

        public void AddFOV(MapObject o)
        {
            FovRadius = o.LOS;
            IFov f = this.GetFieldOfView(o.Coords);

            BoardHexes.ForEach(a => AddHexToFov(f, a));
        }

        private void AddHexToFov(IFov f, MapGridHex hex)
        {
            if (f[hex.Coords]) _los[hex.Coords] = true;
        }

        public static void DefineBoard(String mapPath)
        {
            StreamReader sr = new StreamReader(mapPath);
            _board = new List<string>();

            while (!sr.EndOfStream)
            {
                _board.Add(sr.ReadLine());
            }

            _sizeHexes = new Size(_board[0].Length, _board.Count);
        }

        //Paint the top layer of the display, graphics that changes frequently between refreshes.
        public override void PaintHighlight(Graphics g)
        {
            var container = g.BeginContainer();


            if (StartHex != HexCoords.EmptyUser && this.IsOnboard(GoalHex) && _selectedArmy.Movement > 0)
            {
                TranslateGraphicsToHex(g, StartHex);
                g.DrawPath(Pens.Red, HexgridPath);
                g.EndContainer(container); g.BeginContainer();

                if (_lm.HexDistance(GoalHex) <= _selectedArmy.Movement && StartHex != GoalHex)
                {
                    container = g.BeginContainer();
                    PaintPath(g, Path);
                    g.EndContainer(container); g.BeginContainer();
                }
            }

            g.EndContainer(container);
            container = g.BeginContainer();

            _objects.ForEach((val) => val.Paint(g, this));

            if (_selectedArmy != null)
            {
                var clipHexes = GetClipInHexes(g.VisibleClipBounds);

                using (var shadeBrush = new SolidBrush(Color.FromArgb(ShadeBrushAlpha, ShadeBrushColor)))
                {
                    base.PaintForEachHex(g, clipHexes, coords =>
                    {
                        if (_lm.HexDistance(coords) > _selectedArmy.Movement || _lm.HexDistance(coords) == -1) { g.FillPath(shadeBrush, HexgridPath); }
                    });
                }
            }

            else
            {
                var clipHexes = GetClipInHexes(g.VisibleClipBounds);

                using (var shadeBrush = new SolidBrush(Color.FromArgb(ShadeBrushAlpha, ShadeBrushColor)))
                {
                    base.PaintForEachHex(g, clipHexes, coords =>
                    {
                        if (!_los[coords]) { g.FillPath(shadeBrush, HexgridPath); }
                    });
                }
            }
            g.EndContainer(container);

            
            //Position previous turn
            if (ViewPrevious)
            {
                XDocument turnDoc = XDocument.Load(Turn.SavePath);
                String pName = turnDoc.XPathSelectElement("Turn/CurrentPlayer").Value;
                XDocument tmpDoc = XDocument.Load(@"Save\players_" + pName + ".xml");

                foreach (XElement elem in tmpDoc.XPathSelectElements("Players/Player[Name!=\"" + pName + "\"]/Armies/Army"))
                {
                    int x = int.Parse(elem.Element("X").Value);
                    int y = int.Parse(elem.Element("Y").Value);
                    Image _bmp = Bitmap.FromFile(elem.Element("ImgPath").Value);
                    HexCoords coords = HexCoords.NewUserCoords(x, y);
                    Rectangle rect = Rectangle.Round(BoardHexes[coords].Board.HexgridPath.GetBounds());
                    if (Los[coords])
                    {
                        container = g.BeginContainer();

                        var target = CentreOfHex(coords);

                        ColorMatrix colormatrix = new ColorMatrix();
                        colormatrix.Matrix33 = 0.5F;
                        ImageAttributes imgAttribute = new ImageAttributes();
                        imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        TranslateGraphicsToHex(g, coords);
                        g.DrawImage(_bmp, rect, 0, 0, _bmp.Width, _bmp.Height, GraphicsUnit.Pixel, imgAttribute);
                        if (ShowHexgrid) g.DrawPath(Pens.Black, HexgridPath);

                        g.EndContainer(container);
                    }
                }
            }
        }

        //Paint the intermediate layer of the display, graphics that changes infrequently between refreshes.

        public override void PaintUnits(Graphics g)
        {
        }

        //Paint the base layer of the display, graphics that changes rarely between refreshes.

        public override void PaintMap(Graphics g) { PaintMap(g, (h) => h.Paint(g)); }

        void PaintMap(Graphics g, Action<MapGridHex> paintAction)
        {
            if (g == null) throw new ArgumentNullException("g");
            if (paintAction == null) throw new ArgumentNullException("paintAction");
            var clipHexes = GetClipInHexes(g.VisibleClipBounds);
            var location = new Point(GridSize.Width * 2 / 3, GridSize.Height / 2);

            using (var font = new Font("ArialNarrow", 8))
            using (var format = new StringFormat())
            {
                format.Alignment = format.LineAlignment = StringAlignment.Center;

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                PaintForEachHex(g, clipHexes, coords =>
                {
                    paintAction(this[coords]);
                    if (ShowHexgrid) g.DrawPath(Pens.Black, HexgridPath);
                });
            }
        }
        
        protected override void PaintPath(Graphics g, IDirectedPath path)
        {
            if (g == null) throw new ArgumentNullException("g");

            using (var brush = new SolidBrush(Color.FromArgb(78, Color.PaleGoldenrod)))
            {
                while (path != null)
                {
                    var coords = path.PathStep.Hex.Coords;
                    TranslateGraphicsToHex(g, coords);
                    
                    if (ShowPathArrow) PaintPathArrow(g, path);
                    path = path.PathSoFar;
                }
            }
        }

        new public Point CentreOfHex(HexCoords coords)
        {
            return base.CentreOfHex(coords);
        }

        new public void TranslateGraphicsToHex(Graphics g, HexCoords coords)
        {
            base.TranslateGraphicsToHex(g, coords);
        }
    }
}
