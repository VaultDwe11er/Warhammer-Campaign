using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Custom.HexgridPanel;
using HexUtilities.Pathfinding;
using Terrain;
using Misc;
using HexUtilities;
using HexUtilities.Common;

namespace Warhammer_Campaign
{
    public partial class MainForm : Form
    {
        public static TerrainMap tm;
        List<Player> players;
        Turn turn;
        XDocument playerDoc;
        XDocument tileDoc;
        XDocument turnDoc;
        HexCoords clickCoords;
        String loggedPlayerName;
        bool isPlayerTurn;
        List<Resource> resourceChanges;
        int minimapX, minimapY;
        float minimapScale;

        //New objects to be placed
        List<MapObject> newObjects;
        MapObject newObject;

        //Tooptips
        String ttDisbandArmy, ttBuildBuilding, ttBuildRoad;
        String ttMusterArmy, ttUpgradeTown, ttTownCover, ttTownWalls, ttTownTowers;
        String ttUpgradeCastle, ttCastleWalls, ttCastleTowers, ttCastleGatehouse;

        public MainForm(String lp = "")
        {
            InitializeComponent();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.StartPosition = FormStartPosition.Manual;
            this.Location = Properties.Settings.Default.Location;
            this.WindowState = Properties.Settings.Default.State;
            if (this.WindowState == FormWindowState.Normal) this.Size = Properties.Settings.Default.Size;

            TerrainMap.DefineBoard(@"Terrain\map.txt");

            tm = new TerrainMap();

            LoadMap();

            //Main panel
            hexgridPanel1.Scales = new List<float>() { 1.000F }.AsReadOnly();
            hexgridPanel1.ScaleIndex = hexgridPanel1.Scales
                              .Select((f, i) => new { value = f, index = i })
                              .Where(s => s.value == 1.0F)
                              .Select(s => s.index).FirstOrDefault();

            hexgridPanel1.Host = tm;
            hexgridPanel1.Size = hexgridPanel1.MapSizePixels;

            //Minimap
            minimapScale = Math.Min((float)pbMinimap.Height / (float)hexgridPanel1.Height, (float)pbMinimap.Width / (float)hexgridPanel1.Width);

            pbMinimap.Width = (int)(hexgridPanel1.Width * minimapScale);
            pbMinimap.Height = (int)(hexgridPanel1.Height * minimapScale);

            clickCoords = HexCoords.NewUserCoords(-1, -1);

            loggedPlayerName = lp;

            isPlayerTurn = (turn.CurrentPlayer.Name == loggedPlayerName || loggedPlayerName == "");

            btnEndTurn.Enabled = isPlayerTurn;

            if (loggedPlayerName != "") btnResolve.Enabled = false;

            btnUndo.Enabled = false;

            UpdateLos();
            UpdateSummary();
            UpdateResources();

            //this.MouseWheel += new MouseEventHandler(hexgridPanel1_MouseWheel);
        }

        private void UpdateSummary()
        {
            toolStripStatusLabel2.Text = turn.CurrentPlayer.Name + ", year " + turn.YearNo + ", month " + turn.MonthNo;

            if (tm.SelectedArmy == null)
            {
                toolStripStatusLabel1.Text = clickCoords.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = "Distance = " + tm.SelectedLandmark.HexDistance(tm.HotspotHex).ToString();
            }

            Army a = tm.GetArmy(clickCoords);
            Town t = tm.GetTown(clickCoords);
            Watchtower wt = tm.GetWatchtower(clickCoords);
            Castle c = tm.GetCastle(clickCoords);
            TerrainGridHex hex = (TerrainGridHex)tm.BoardHexes[clickCoords];

            Player loggedPlayer = (loggedPlayerName == "" ? turn.CurrentPlayer : players.Find((val) => val.Name == loggedPlayerName));

            String txt;


            //New object to be placed logic
            if (newObject != null)
            {
                txt = "Select position for " + newObject.ObjectType + " '" + newObject.Name + "'";
            }
            else
            {
                txt = loggedPlayer.Name + "\r\n";
                txt = txt + "--------------------------------------------\r\n";

                if (isPlayerTurn)
                {
                    txt = txt + "Armies with movement left:\r\n";

                    foreach (Army _a in turn.CurrentPlayer.Armies)
                    {
                        if (_a.Movement > 0) txt += _a.Name + "\r\n";
                    }
                }

                if (hex.IsOnboard())
                {
                    txt = txt + "\r\n";
                    txt = txt + "Tile:\r\n";
                    txt = txt + "--------------------------------------------\r\n";
                    txt = txt + "Type: " + hex.Type + "\r\n";
                    if(hex.SpecialType != "" )txt = txt + "Special Type: " + hex.SpecialType + "\r\n";
                    txt = txt + "Movement Cost: " + (hex.StepCost(Hexside.North) == -1 ? "Impassable" : hex.StepCost(Hexside.North).ToString()) + "\r\n";
                    foreach (var r in hex.Resources) txt = txt + r.Name + ": " + r.Value.ToString() + "\r\n";
                }

                if (a != null)
                {
                    if (loggedPlayer.Armies.Contains(a))
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected army (owned):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + a.Name + "\r\n";
                        txt = txt + "Type: " + a.ArmyType + " Army\r\n";
                        txt = txt + "Size: " + a.Size.ToString() + " points\r\n";
                        txt = txt + "Movement: " + a.Movement.ToString() + "\r\n";
                        if (a.IsInBattle) txt = txt + "In battle\r\n";
                        if (a.IsSieging) txt = txt + "Sieging\r\n";
                        txt = txt + "Upkeep:\r\n";
                        foreach (var r in a.UpkeepCost) txt = txt + r.Name + ": " + r.Value.ToString() + "\r\n";
                    }
                    else
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected army (enemy):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + a.Name + "\r\n";
                    }
                }

                if (t != null)
                {
                    if (loggedPlayer.Towns.Contains(t))
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected town (owned):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + t.Name + "\r\n";
                        txt = txt + "Level: " + t.Level.ToString() + "\r\n";
                        txt = txt + "Garrison size: " + t.GarrisonSize.ToString() + "\r\n";
                        txt = txt + "Obstacles: " + t.Cover + "\r\n";
                        txt = txt + "Walls: " + t.Walls + "\r\n";
                        txt = txt + "Towers: " + t.Towers + "\r\n";
                        if (t.IsInBattle) txt = txt + "In battle\r\n";
                        if (t.IsUnderSiege) txt = txt + "Under Siege\r\n";
                    }
                    else
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected town (enemy):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + t.Name + "\r\n";
                    }
                }

                if (wt != null)
                {
                    if (loggedPlayer.Watchtowers.Contains(wt))
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected watchtower (owned):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + wt.Name + "\r\n";
                        txt = txt + "Garrison size: " + wt.GarrisonSize.ToString() + "\r\n";
                    }
                    else
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected watchtower (enemy):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + wt.Name + "\r\n";
                    }
                }

                if (c != null)
                {
                    if (loggedPlayer.Castles.Contains(c))
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected castle (owned):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + c.Name + "\r\n";
                        txt = txt + "Level: " + c.Level.ToString() + "\r\n";
                        txt = txt + "Garrison size: " + c.GarrisonSize.ToString() + "\r\n";
                        txt = txt + "Walls: " + c.Walls + "\r\n";
                        txt = txt + "Towers: " + c.Towers + "\r\n";
                        txt = txt + "Gatehouse: " + c.Gatehouse + "\r\n";
                        if (c.IsInBattle) txt = txt + "In battle\r\n";
                        if (c.IsUnderSiege) txt = txt + "Under Siege\r\n";
                    }
                    else
                    {
                        txt = txt + "\r\n";
                        txt = txt + "Selected castle (enemy):\r\n";
                        txt = txt + "--------------------------------------------\r\n";
                        txt = txt + "Name: " + c.Name + "\r\n";
                    }
                }
            }
            lblSummary.Text = txt;
        }

        private void UpdateResources()
        {
            Player loggedPlayer = (loggedPlayerName == "" ? turn.CurrentPlayer : players.Find((val) => val.Name == loggedPlayerName));

            resourceChanges = new List<Resource>();
            loggedPlayer.Resources.ForEach(r => resourceChanges.Add(new Resource(r.Name, 0)));

            //Income
            foreach (TerrainGridHex hex in loggedPlayer.ResourceHexes(tm))
            {
                foreach (Resource r in hex.Resources)
                {
                    if (resourceChanges.Exists(val => val.Name == r.Name)) resourceChanges.Find(val => val.Name == r.Name).Add(r.Value);
                }
            }

            //Upkeep
            loggedPlayer.Armies.ForEach(a => a.UpkeepCost.ForEach(cost => resourceChanges.Find(res => cost.Name == res.Name).Add(-cost.Value)));

            //Update labels
            lblResources.Text = "";
            foreach (var r in loggedPlayer.Resources)
            {
                String diffAmt = resourceChanges.Find(val => val.Name == r.Name).Value.ToString();
                if (diffAmt[0] != '-') diffAmt = "+" + diffAmt;

                lblResources.Text = lblResources.Text + "  " + r.Name + ":" + r.Value.ToString() + " (" + diffAmt.ToString() + ")";
            }
        }

        private void UpdateTooltips()
        {
            ttDisbandArmy = "Disband the current army completely. If army is disbanded at a city or town, will increase its level";
            cmsArmy_DisbandArmy.ToolTipText = ttDisbandArmy;
            cmsArmyTown_DisbandArmy.ToolTipText = ttDisbandArmy;
            cmsArmyCastle_DisbandArmy.ToolTipText = ttDisbandArmy;

            ttBuildBuilding = "Build a city, town or watchtower at the current location";
            cmsArmy_BuildBuilding.ToolTipText = ttBuildBuilding;
            cmsArmyTown_BuildBuilding.ToolTipText = ttBuildBuilding;
            cmsArmyCastle_BuildBuilding.ToolTipText = ttBuildBuilding;

            ttBuildRoad = "Build a road at the current location. Must be connected to another road, a city or a town (" + turn.CurrentPlayer.Race.GetCost("Road") + ")";
            cmsArmy_BuildRoad.ToolTipText = ttBuildRoad;
            cmsArmyTown_BuildRoad.ToolTipText = ttBuildRoad;
            cmsArmyCastle_BuildRoad.ToolTipText = ttBuildRoad;

            ttUpgradeTown = "Increase town's level by 1 (" + turn.CurrentPlayer.Race.GetCost("UpgradeTown") + ")";
            cmsTown_UpgradeTown.ToolTipText = ttUpgradeTown;
            cmsArmyTown_UpgradeTown.ToolTipText = ttUpgradeTown;

            ttMusterArmy = "Muster a new army";
            cmsTown_MusterArmy.ToolTipText = ttMusterArmy;
            cmsCastle_MusterArmy.ToolTipText = ttMusterArmy;

            Town t = tm.GetTown(clickCoords);
            if (t != null)
            {
                if (t.Cover == Town.CoverLevels.Last())
                {
                    ttTownCover = "Upgrade the town's cover (Already at max level)";
                }
                else
                {
                    int pos = Array.IndexOf(Town.CoverLevels, t.Cover) + 1;
                    ttTownCover = "Upgrade the town's cover" + " (" + turn.CurrentPlayer.Race.GetCost(Town.CoverLevelCosts[pos]) + ")";
                }

                cmsTown_UpgradeObstacles.ToolTipText = ttTownCover;
                cmsArmyTown_UpgradeObstacles.ToolTipText = ttTownCover;

                if (t.Walls == Town.WallLevels.Last())
                {
                    ttTownWalls = "Upgrade the town's walls (Already at max level)";
                }
                else
                {
                    int pos = Array.IndexOf(Town.WallLevels, t.Walls) + 1;
                    ttTownWalls = "Upgrade the town's walls" + " (" + turn.CurrentPlayer.Race.GetCost(Town.WallLevelCosts[pos]) + ")";
                }

                cmsTown_UpgradeWalls.ToolTipText = ttTownWalls;
                cmsArmyTown_UpgradeWalls.ToolTipText = ttTownWalls;

                if (t.Towers == Town.TowerLevels.Last())
                {
                    ttTownTowers = "Upgrade the town's towers. (Already at max level)";
                }
                else
                {
                    int pos = Array.IndexOf(Town.TowerLevels, t.Towers) + 1;
                    ttTownTowers = "Upgrade the town's towers" + ". (" + turn.CurrentPlayer.Race.GetCost(Town.TowerLevelCosts[pos]) + ")";
                }

                cmsTown_UpgradeTowers.ToolTipText = ttTownTowers;
                cmsArmyTown_UpgradeTowers.ToolTipText = ttTownTowers;
            }

            ttUpgradeCastle = "Increase castle's level by 1 (" + turn.CurrentPlayer.Race.GetCost("UpgradeTown") + ")";
            cmsCastle_UpgradeCastle.ToolTipText = ttUpgradeCastle;
            cmsArmyCastle_UpgradeCastle.ToolTipText = ttUpgradeCastle;

            Castle c = tm.GetCastle(clickCoords);
            if (c != null)
            {
                if (c.Walls == Castle.WallLevels.Last())
                {
                    ttCastleWalls = "Upgrade the castle's walls (Already at max level)";
                }
                else
                {
                    int pos = Array.IndexOf(Castle.WallLevels, c.Walls) + 1;
                    ttCastleWalls = "Upgrade the castle's walls" + " (" + turn.CurrentPlayer.Race.GetCost(Castle.WallLevelCosts[pos]) + ")";
                }

                cmsCastle_UpgradeWalls.ToolTipText = ttCastleWalls;
                cmsArmyCastle_UpgradeWalls.ToolTipText = ttCastleWalls;

                if (c.Towers == Castle.TowerLevels.Last())
                {
                    ttCastleTowers = "Upgrade the castle's towers (Already at max level)";
                }
                else
                {
                    int pos = Array.IndexOf(Castle.TowerLevels, c.Towers) + 1;
                    ttCastleTowers = "Upgrade the castle's towers" + " (" + turn.CurrentPlayer.Race.GetCost(Castle.TowerLevelCosts[pos]) + ")";
                }

                cmsCastle_UpgradeTowers.ToolTipText = ttCastleTowers;
                cmsArmyCastle_UpgradeTowers.ToolTipText = ttCastleTowers;

                if (c.Gatehouse == Castle.GatehouseLevels.Last())
                {
                    ttCastleGatehouse = "Upgrade the castle's gatehouse (Already at max level)";
                }
                else
                {
                    int pos = Array.IndexOf(Castle.GatehouseLevels, c.Gatehouse) + 1;
                    ttCastleGatehouse = "Upgrade the castle's gatehouse" + " (" + turn.CurrentPlayer.Race.GetCost(Castle.GatehouseLevelCosts[pos]) + ")";
                }

                cmsCastle_UpgradeGatehouse.ToolTipText = ttCastleGatehouse;
                cmsArmyCastle_UpgradeGatehouse.ToolTipText = ttCastleGatehouse;
            }
        }

        private void LoadMap()
        {
            if (tm != null)
            {
                tm.Dispose();
            }

            tm.Objects = new List<MapObject>();
            newObjects = new List<MapObject>();

            playerDoc = XDocument.Load(Player.SavePath);
            tileDoc = XDocument.Load(TerrainGridHex.SavePath);
            turnDoc = XDocument.Load(Turn.SavePath);

            players = new List<Player>();

            int cnt = playerDoc.XPathSelectElements("Players/Player").Count();

            for (int i = 1; i <= cnt; i++)
            {
                XElement e = playerDoc.XPathSelectElement("Players/Player[Position=\"" + i.ToString() + "\"]");

                String name = e.Element("Name").Value;
                int position = int.Parse(e.Element("Position").Value);
                String race = e.Element("Race").Value;
                String password = e.Element("Password").Value;
                int hor = int.Parse(e.Element("Horizontal").Value);
                int ver = int.Parse(e.Element("Vertical").Value);

                Player p = new Player(name, position, race, password, false, hor, ver);

                players.Add(p);

                //Load towns
                foreach (var elem in e.XPathSelectElements("Buildings/Town"))
                {
                    String _name = elem.Element("Name").Value;
                    int _level = int.Parse(elem.Element("Level").Value);
                    bool _levelChanged = bool.Parse(elem.Element("LevelChanged").Value);
                    String _coverLevel = elem.Element("CoverLevel").Value;
                    String _wallLevel = elem.Element("WallLevel").Value;
                    String _towerLevel = elem.Element("TowerLevel").Value;
                    int _x = int.Parse(elem.Element("X").Value);
                    int _y = int.Parse(elem.Element("Y").Value);
                    String _imgPath = elem.Element("ImagePath").Value;

                    Town t = new Town(_name, _level, _x, _y, _imgPath, name, _coverLevel, _wallLevel, _towerLevel, _levelChanged);

                    if (_x == -1)
                    {
                        newObjects.Add(t);
                    }

                    tm.AddObject(t);
                    p.AddTown(t);
                }

                //Load watchtowers
                foreach (var elem in e.XPathSelectElements("Buildings/Watchtower"))
                {
                    String _name = elem.Element("Name").Value;
                    int _gs = int.Parse(elem.Element("GarrisonSize").Value);
                    int _x = int.Parse(elem.Element("X").Value);
                    int _y = int.Parse(elem.Element("Y").Value);
                    String _imgPath = elem.Element("ImagePath").Value;

                    Watchtower wt = new Watchtower(_name, _x, _y, name, _imgPath, _gs);

                    if (_x == -1)
                    {
                        newObjects.Add(wt);
                    }

                    tm.AddObject(wt);
                    p.AddWatchtower(wt);
                }

                //Load castles
                foreach (var elem in e.XPathSelectElements("Buildings/Castle"))
                {
                    String _name = elem.Element("Name").Value;
                    int _level = int.Parse(elem.Element("Level").Value);
                    bool _levelChanged = bool.Parse(elem.Element("LevelChanged").Value);
                    int _x = int.Parse(elem.Element("X").Value);
                    int _y = int.Parse(elem.Element("Y").Value);
                    String _imgPath = elem.Element("ImagePath").Value;
                    String _wallLevel = elem.Element("WallLevel").Value;
                    String _towerLevel = elem.Element("TowerLevel").Value;
                    String _gatehouseLevel = elem.Element("GatehouseLevel").Value;

                    Castle c = new Castle(_name, _level, _levelChanged, _x, _y, _imgPath, name, _wallLevel, _towerLevel, _gatehouseLevel);

                    if (_x == -1)
                    {
                        newObjects.Add(c);
                    }

                    tm.AddObject(c);
                    p.AddCastle(c);
                }

                //Load armies
                foreach (var elem in e.XPathSelectElements("Armies/Army"))
                {
                    String _name = elem.Element("Name").Value;
                    String _type = elem.Element("Type").Value;
                    int _x = int.Parse(elem.Element("X").Value);
                    int _y = int.Parse(elem.Element("Y").Value);
                    double _moveTotal = double.Parse(elem.Element("MoveTotal").Value);
                    double _moveLeft = double.Parse(elem.Element("MoveLeft").Value);
                    Pen _color = new Pen(Color.FromName(elem.Element("Color").Value));
                    int _size = int.Parse(elem.Element("Size").Value);
                    bool _canBuild = bool.Parse(elem.Element("CanBuild").Value);
                    String _imgPath = elem.Element("ImgPath").Value;
                    List<Resource> _upkeepCost = new List<Resource>();
                    elem.Element("Upkeep").Attributes().ToList().ForEach(val => _upkeepCost.Add(new Resource(val.Name.ToString(), int.Parse(val.Value))));

                    Army a = new Army(_x, _y, _name, _type, _moveTotal, _moveLeft, _color, _size, name, _canBuild, _imgPath, _upkeepCost);

                    if (_x == -1)
                    {
                        newObjects.Add(a);
                    }

                    tm.AddObject(a);
                    p.AddArmy(a);
                }

                //Load resources
                foreach (var elem in e.XPathSelectElements("Resources").Elements())
                {
                    String _name = elem.Name.ToString();
                    int _amt = int.Parse(elem.Value);

                    p.Race.GetResource(_name).Value = _amt;
                }
            }

            //Load battles
            foreach (var e in playerDoc.XPathSelectElements("Players/Player/Armies/Army[IsInBattle=\"true\" or IsInBattle=\"True\"]"))
            {
                Army a = tm.Armies.Find((val) => val.Name == e.Element("Name").Value);
                MapObject mo = tm.Objects.Find((val) => val.Name == e.Element("UnitAttacked").Value);

                a.Attack(mo);
            }

            //Load sieges
            foreach (var e in playerDoc.XPathSelectElements("Players/Player/Armies/Army[IsSieging=\"true\" or IsSieging=\"True\"]"))
            {
                Army a = tm.Armies.Find((val) => val.Name == e.Element("Name").Value);
                Castle c = tm.Castles.Find((val) => val.Name == e.Element("UnitAttacked").Value);
                Town t = tm.Towns.Find((val) => val.Name == e.Element("UnitAttacked").Value);

                if (c != null) a.Siege(c);
                if (t != null) a.Siege(t);
            }

            //Load turn
            String _currentPlayer = turnDoc.XPathSelectElement("Turn/CurrentPlayer").Value;
            int _yearNo = int.Parse(turnDoc.XPathSelectElement("Turn/YearNo").Value);
            int _monthNo = int.Parse(turnDoc.XPathSelectElement("Turn/MonthNo").Value);
            bool _isStartOfTurn = bool.Parse(turnDoc.XPathSelectElement("Turn/IsStartOfTurn").Value);

            turn = new Turn(players, _currentPlayer, _yearNo, _monthNo, _isStartOfTurn);

            tm.BoardHexes.ForEach((val) => ((TerrainGridHex)val).IsSummer = (_monthNo <= 8));

            //New object to be placed logic
            if (newObjects.Count > 0) newObject = newObjects.First();
        }

        private void UpdateMinimapRelated()
        {
            Rectangle r1 = panel1.ClientRectangle;
            int x = (int)(minimapX * minimapScale);
            int y = (int)(minimapY * minimapScale);
            int w = (int)(r1.Width * minimapScale);
            int h = (int)(r1.Height * minimapScale);
            Rectangle rect = new Rectangle(x, y, w, h);

            pbMinimap.MinimapRect = rect;

            pbMinimap.Refresh();
        }

        public void UpdateLos()
        {
            Player loggedPlayer = (loggedPlayerName == "" ? turn.CurrentPlayer : players.Find((val) => val.Name == loggedPlayerName));

            tm.Los = new Fov(tm);
            if (loggedPlayerName != "Admin")
            {
                loggedPlayer.Armies.ForEach(a => tm.AddFOV(a));
                loggedPlayer.Towns.ForEach(t => tm.AddFOV(t));
                loggedPlayer.Watchtowers.ForEach(wt => tm.AddFOV(wt));
                loggedPlayer.Castles.ForEach(c => tm.AddFOV(c));
            }
        }

        private void hexgridPanel1_LeftClick(object sender, HexEventArgs e)
        {
            Army a = tm.GetArmy(e.Coords);
            MapObject b = tm.GetBuilding(e.Coords);
            clickCoords = e.Coords;

            //New object logic
            if (newObject != null)
            {
                if (tm.BoardHexes[clickCoords].StepCost(Hexside.North) == -1)
                {
                    MessageBox.Show("Can't place on immovable terrain");
                }

                else if (newObject.Coords != clickCoords)
                {
                    MessageBox.Show("Other building/enemy unit already on this spot");
                }
                else
                {
                    newObject.Coords = clickCoords;
                    Refresh();
                    newObject.Save();

                    newObjects.Remove(newObject);

                    if (newObjects.Count > 0)
                    {
                        newObject = newObjects.First();
                    }
                    else
                    {
                        newObject = null;
                    }
                }
                clickCoords = HexCoords.NewUserCoords(-1, -1);
            }

            else if (isPlayerTurn)
            {
                if (a != null)
                {
                    if (tm.SelectedArmy == a)
                    {
                        tm.SelectedArmy = null;
                    }
                    else if (turn.CurrentPlayer.Armies.Contains(a))
                    {
                        tm.SelectedArmy = a;
                    }
                }
                else
                {
                    tm.SelectedArmy = null;
                }
            }
            UpdateSummary();
            Refresh();
        }

        private void hexgridPanel1_RightClick(object sender, HexEventArgs e)
        {
            Army a = tm.GetArmy(e.Coords);
            Town t = tm.GetTown(e.Coords);
            Watchtower wt = tm.GetWatchtower(e.Coords);
            Castle c = tm.GetCastle(e.Coords);
            TerrainGridHex hex = (TerrainGridHex)tm.BoardHexes[e.Coords];

            if (isPlayerTurn && newObject == null)
            {
                if (tm.SelectedArmy == null)
                {
                    clickCoords = e.Coords;
                }

                //Move
                if (tm.SelectedArmy != null && tm.InMovement(e.Coords) && tm.SelectedArmy.Coords != e.Coords)
                {
                    //Can't move, own army in the way
                    if (turn.CurrentPlayer.Armies.Contains(a))
                    {
                        MessageBox.Show("Can't move here, other army in the way");
                    }
                    //Siege
                    else if ((c != null && !turn.CurrentPlayer.Castles.Contains(c)) || (t != null && !turn.CurrentPlayer.Towns.Contains(t)))
                    {
                        

                        SiegeForm sf = new SiegeForm();
                        int result = sf.CustomShowDialog();
                        sf.Dispose();

                        HexCoords coords = tm.Path.ElementAt(tm.Path.TotalSteps - 1).StepCoords;

                        switch(result)
                        {
                            case 0:
                                playerDoc.Save(Player.UndoPath);
                                btnUndo.Enabled = true;

                                tm.SelectedArmy.Move(coords, tm);
                                if (c != null) tm.SelectedArmy.Attack(c);
                                if (t != null) tm.SelectedArmy.Attack(t);

                                playerDoc = XDocument.Load(Player.SavePath);
                                break;

                            case 1:
                                playerDoc.Save(Player.UndoPath);
                                btnUndo.Enabled = true;

                                tm.SelectedArmy.Move(coords, tm);
                                if (c != null) tm.SelectedArmy.Siege(c);
                                if (t != null) tm.SelectedArmy.Siege(t);

                                playerDoc = XDocument.Load(Player.SavePath);
                                break;
                        }

                        tm.SelectedArmy = null;
                    }
                    //Attack army
                    else if (a != null && !turn.CurrentPlayer.Armies.Contains(a))
                    {
                        playerDoc.Save(Player.UndoPath);
                        btnUndo.Enabled = true;

                        if (a.Size > 0)
                        {
                            HexCoords coords = tm.Path.ElementAt(tm.Path.TotalSteps - 1).StepCoords;

                            tm.SelectedArmy.Move(coords, tm);
                            tm.SelectedArmy.Attack(a);

                            tm.SelectedArmy = null;

                            playerDoc = XDocument.Load(Player.SavePath);
                        }
                        else
                        {
                            a.Remove();
                            tm.SelectedArmy.Move(e.Coords, tm);
                            tm.SelectedArmy = null;
                        }

                        playerDoc = XDocument.Load(Player.SavePath);
                    }
                    //Move
                    else //(a == null && t == null && c == null && wt == null)
                    {
                        DialogResult result = System.Windows.Forms.DialogResult.Yes;
                        if (tm.SelectedArmy.IsSieging) { result = MessageBox.Show("About to stop sieging. Are you sure?", "Warning", MessageBoxButtons.YesNo); }

                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            playerDoc.Save(Player.UndoPath);
                            btnUndo.Enabled = true;

                            if (tm.SelectedArmy.IsSieging) tm.SelectedArmy.EndSiege();
                            tm.SelectedArmy.Move(e.Coords, tm);
                            tm.SelectedArmy = null;

                            playerDoc = XDocument.Load(Player.SavePath);
                        }
                    }
                }

                //Show menus
                else //if (tm.SelectedArmy == null)
                {
                    if (turn.CurrentPlayer.Armies.Contains(a) && turn.CurrentPlayer.Towns.Contains(t))
                    {
                        cmsArmyTown_BuildBuilding.Enabled = true;
                        cmsArmyTown_IncreaseArmy.Enabled = true;
                        cmsArmyTown_UpgradeTown.Enabled = true;
                        cmsArmyTown_UpgradeObstacles.Enabled = true;
                        cmsArmyTown_UpgradeWalls.Enabled = true;
                        cmsArmyTown_UpgradeTowers.Enabled = true;

                        cmsArmyTown_BuildBuilding.Enabled = false;
                        cmsArmyTown_BuildRoad.Enabled = false;
                        if (t.Level <= 1 || t.IsUnderSiege || t.IsInBattle) cmsArmyTown_IncreaseArmy.Enabled = false;
                        if (t.Level == 20 || t.LevelChanged || !turn.CurrentPlayer.Race.CanPay("UpgradeTown") || t.IsUnderSiege || t.IsInBattle) cmsArmyTown_UpgradeTown.Enabled = false;
                        if (t.Cover == Town.CoverLevels.Last() || !turn.CurrentPlayer.CanUpgradeCover(t) || t.IsUnderSiege || t.IsInBattle) cmsArmyTown_UpgradeObstacles.Enabled = false;
                        if (t.Walls == Town.WallLevels.Last() || !turn.CurrentPlayer.CanUpgradeWalls(t) || t.IsUnderSiege || t.IsInBattle) cmsArmyTown_UpgradeWalls.Enabled = false;
                        if (t.Towers == Town.TowerLevels.Last() || !turn.CurrentPlayer.CanUpgradeTowers(t) || t.IsUnderSiege || t.IsInBattle) cmsArmyTown_UpgradeTowers.Enabled = false;

                        cmsArmyTown.Show(hexgridPanel1, e.X, e.Y);
                    }
                    else if (turn.CurrentPlayer.Armies.Contains(a) && turn.CurrentPlayer.Castles.Contains(c))
                    {
                        cmsArmyCastle_BuildBuilding.Enabled = true;
                        cmsArmyCastle_IncreaseArmy.Enabled = true;
                        cmsArmyCastle_UpgradeCastle.Enabled = true;
                        cmsArmyCastle_UpgradeWalls.Enabled = true;
                        cmsArmyCastle_UpgradeTowers.Enabled = true;
                        cmsArmyCastle_UpgradeGatehouse.Enabled = true;

                        cmsArmyCastle_BuildBuilding.Enabled = false;
                        cmsArmyCastle_BuildRoad.Enabled = false;
                        if (c.Level <= 1 || a != null || c.IsUnderSiege) cmsArmyCastle_IncreaseArmy.Enabled = false;
                        if (c.Level == 20 || c.LevelChanged || !turn.CurrentPlayer.Race.CanPay("UpgradeCastle") || c.IsUnderSiege || c.IsInBattle) cmsArmyCastle_UpgradeCastle.Enabled = false;
                        if (c.Walls == Castle.WallLevels.Last() || !turn.CurrentPlayer.CanUpgradeWalls(c) || c.IsUnderSiege || c.IsInBattle) cmsArmyCastle_UpgradeWalls.Enabled = false;
                        if (c.Towers == Castle.TowerLevels.Last() || !turn.CurrentPlayer.CanUpgradeTowers(c) || c.IsUnderSiege || c.IsInBattle) cmsArmyCastle_UpgradeTowers.Enabled = false;
                        if (c.Gatehouse == Castle.GatehouseLevels.Last() || !turn.CurrentPlayer.CanUpgradeGatehouse(c) || c.IsUnderSiege || c.IsInBattle) cmsArmyCastle_UpgradeGatehouse.Enabled = false;

                        cmsArmyCastle.Show(hexgridPanel1, e.X, e.Y);
                    }
                    else if (turn.CurrentPlayer.Armies.Contains(a))
                    {
                        cmsArmy_BuildBuilding.Enabled = true;
                        cmsArmy_BuildRoad.Enabled = true;

                        if (tm.GetBuilding(clickCoords) != null || turn.CurrentPlayer.BuildableBuildings.Count == 0 || !a.CanBuild || a.IsInBattle) cmsArmy_BuildBuilding.Enabled = false;
                        if (tm.GetBuilding(clickCoords) != null || !turn.CurrentPlayer.Race.CanPay("Road") || hex.SpecialType == "Road" || !a.CanBuild || a.IsInBattle || !a.CanBuildRoad(tm)) cmsArmy_BuildRoad.Enabled = false;

                        cmsArmy.Show(hexgridPanel1, e.X, e.Y);
                    }
                    else if (turn.CurrentPlayer.Towns.Contains(t))
                    {
                        cmsTown_MusterArmy.Enabled = true;
                        cmsTown_UpgradeTown.Enabled = true;
                        cmsTown_UpgradeObstacles.Enabled = true;
                        cmsTown_UpgradeWalls.Enabled = true;
                        cmsTown_UpgradeTowers.Enabled = true;

                        if (t.Level <= 5 || a != null || t.IsUnderSiege || t.IsInBattle) cmsTown_MusterArmy.Enabled = false;
                        if (t.Level == 20 || t.LevelChanged || !turn.CurrentPlayer.Race.CanPay("UpgradeTown") || t.IsUnderSiege || t.IsInBattle) cmsTown_UpgradeTown.Enabled = false;
                        if (t.Cover == Town.CoverLevels.Last() || !turn.CurrentPlayer.CanUpgradeCover(t) || t.IsUnderSiege || t.IsInBattle) cmsTown_UpgradeObstacles.Enabled = false;
                        if (t.Walls == Town.WallLevels.Last() || !turn.CurrentPlayer.CanUpgradeWalls(t) || t.IsUnderSiege || t.IsInBattle) cmsTown_UpgradeWalls.Enabled = false;
                        if (t.Towers == Town.TowerLevels.Last() || !turn.CurrentPlayer.CanUpgradeTowers(t) || t.IsUnderSiege || t.IsInBattle) cmsTown_UpgradeTowers.Enabled = false;

                        cmsTown.Show(hexgridPanel1, e.X, e.Y);
                    }
                    else if (turn.CurrentPlayer.Castles.Contains(c))
                    {
                        cmsCastle_MusterArmy.Enabled = true;
                        cmsCastle_UpgradeCastle.Enabled = true;
                        cmsCastle_UpgradeWalls.Enabled = true;
                        cmsCastle_UpgradeTowers.Enabled = true;
                        cmsCastle_UpgradeGatehouse.Enabled = true;

                        if (c.Level <= 2 || a != null || c.IsUnderSiege || c.IsInBattle) cmsCastle_MusterArmy.Enabled = false;
                        if (c.Level == 20 || c.LevelChanged || !turn.CurrentPlayer.Race.CanPay("UpgradeCastle") || c.IsUnderSiege || c.IsInBattle) cmsCastle_UpgradeCastle.Enabled = false;
                        if (c.Walls == Castle.WallLevels.Last() || !turn.CurrentPlayer.CanUpgradeWalls(c) || c.IsUnderSiege || c.IsInBattle) cmsCastle_UpgradeWalls.Enabled = false;
                        if (c.Towers == Castle.TowerLevels.Last() || !turn.CurrentPlayer.CanUpgradeTowers(c) || c.IsUnderSiege || c.IsInBattle) cmsCastle_UpgradeTowers.Enabled = false;
                        if (c.Gatehouse == Castle.GatehouseLevels.Last() || !turn.CurrentPlayer.CanUpgradeGatehouse(c) || c.IsUnderSiege || c.IsInBattle) cmsCastle_UpgradeGatehouse.Enabled = false;

                        cmsCastle.Show(hexgridPanel1, e.X, e.Y);
                    }
                }
            }

            UpdateResources();
            LoadMap();
            UpdateSummary();
            UpdateTooltips();
            Refresh();
        }

        private void btnEndTurn_Click(object sender, EventArgs e)
        {
            tm.SelectedArmy = null;

            bool armiesInBattle = tm.Armies.Exists(a => a.IsInBattle);

            //Check if net resources > 0
            if (!turn.IsStartOfTurn)
            {
                foreach (Resource r in turn.CurrentPlayer.Resources)
                {
                    if (r.Value + resourceChanges.Find(val => val.Name == r.Name).Value < 0)
                    {
                        throw new NotImplementedException("Event where net resources < 0 unhandled");
                    }
                }

                foreach (Resource r in turn.CurrentPlayer.Resources)
                {
                    r.Add(resourceChanges.Find(val => val.Name == r.Name).Value);
                }
                turn.CurrentPlayer.Save();
            }

            //Battles to resolve
            if (turn.IsStartOfTurn && armiesInBattle)
            {
                String str = "The following battles need to be resolved first:\r\n";
                foreach (var a in tm.Armies)
                {
                    if (a.UnitAttacked != null)
                    {
                        str += a.Name + " vs " + a.UnitAttacked + "\r\n";
                    }
                }

                MessageBox.Show(str);
            }
            else if (turn.IsStartOfTurn && newObjects.Count > 0)
            {
                MessageBox.Show("New objects have not been placed");
            }
            else
            {
                if(!turn.IsStartOfTurn)playerDoc.Save(@"Save\players_" + turn.CurrentPlayer.Name + ".xml");

                turn.MoveNextPlayer();
                playerDoc = XDocument.Load(Player.SavePath);
            }

            btnUndo.Enabled = false;
            isPlayerTurn = (turn.CurrentPlayer.Name == loggedPlayerName || loggedPlayerName == "");

            btnEndTurn.Enabled = isPlayerTurn;

            hexgridPanel1.MapBuffer = null;
            LoadMap();
            UpdateLos();
            UpdateResources();
            UpdateSummary();
            Refresh();
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            playerDoc = XDocument.Load(Player.UndoPath);
            playerDoc.Save(Player.SavePath);

            LoadMap();

            clickCoords = HexCoords.NewUserCoords(-1, -1);
            tm.SelectedArmy = null;
            btnUndo.Enabled = false;

            UpdateSummary();
            Refresh();
        }

        private void cbPrevious_CheckedChanged(object sender, EventArgs e)
        {
            tm.ViewPrevious = cbPrevious.Checked;
            Refresh();
        }

        private void hexgridPanel1_HotspotHexChange(object sender, HexEventArgs e)
        {

            tm.HotspotHex = e.Coords;
            tm.GoalHex = e.Coords;
            MapObject b = tm.GetBuilding(e.Coords);
            Army a = tm.GetArmy(e.Coords);

            if (newObject != null && b == null && a == null)
            {
                newObject.Coords = e.Coords;
                tm.Los = new Fov(tm);
                tm.Los[e.Coords] = true;
            }

            Refresh();
            UpdateSummary();
        }

        private void cmsTown_UpgradeTown_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Town t = tm.GetTown(clickCoords);
            turn.CurrentPlayer.UpgradeTown(t);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        public void cmsArmy_DisbandArmy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("About to disband this army. Are you sure?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                playerDoc.Save(Player.UndoPath);
                btnUndo.Enabled = true;

                Army a = tm.GetArmy(clickCoords);
                Town t = tm.GetTown(clickCoords);
                Castle c = tm.GetCastle(clickCoords);

                if (t != null)
                {
                    int l = Math.Min(a.Size/100, 20-t.Level);
                    t.Upgrade(l);
                }
                if (c != null) 
                {
                    int l = (int)(Math.Floor((double)(a.Size / 400)));
                    l = Math.Min(l, 20 - c.Level);
                    c.Upgrade(l);
                }

                a.Remove();

                playerDoc = XDocument.Load(Player.SavePath);

                LoadMap();
                UpdateSummary();
                UpdateResources();
            }
        }

        private void cmsArmy_BuildBuilding_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            if (turn.CurrentPlayer.BuildableBuildings.Count > 0)
            {
                NewBuildingForm nbf = new NewBuildingForm(turn.CurrentPlayer.Name, clickCoords, turn.CurrentPlayer.BuildableBuildings.ToArray(), turn.CurrentPlayer.Race);
                nbf.ShowDialog();
            }

            LoadMap();

            MapObject b = tm.GetBuilding(clickCoords);

            if (b != null)
            {
                switch (b.ObjectType)
                {
                    case "Town":
                        turn.CurrentPlayer.BuildTown();
                        break;
                    case "Watchtower":
                        turn.CurrentPlayer.BuildWatchtower();
                        break;
                    case "Castle":
                        turn.CurrentPlayer.BuildCastle();
                        break;
                }
            }

            playerDoc = XDocument.Load(Player.SavePath);

            UpdateSummary();
            UpdateResources();
        }
        private void cmsArmy_BuildRoad_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            tileDoc.Save(TerrainGridHex.UndoPath);
            btnUndo.Enabled = true;

            TerrainGridHex hex = (TerrainGridHex)tm.BoardHexes[clickCoords];

            hex.SpecialType = "Road";
            turn.CurrentPlayer.BuildRoad();

            hexgridPanel1.MapBuffer = null;
            LoadMap();
            
            playerDoc = XDocument.Load(Player.SavePath);
            tileDoc = XDocument.Load(TerrainGridHex.SavePath);

            UpdateSummary();
            UpdateResources();
        }

        private void cmsTown_UpgradeObstacles_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Town t = tm.GetTown(clickCoords);

            turn.CurrentPlayer.UpgradeCover(t);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsTown_UpgradeWalls_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Town t = tm.GetTown(clickCoords);

            turn.CurrentPlayer.UpgradeWalls(t);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsTown_UpgradeTowers_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Town t = tm.GetTown(clickCoords);

            turn.CurrentPlayer.UpgradeTowers(t);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsTown_MusterArmy_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;


            Town t = tm.GetTown(clickCoords);
            object[] arr = new object[t.Level - 5];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (i + 5) * 100;
            }

            NewArmyForm naf = new NewArmyForm(turn.CurrentPlayer.Name, clickCoords, arr);

            naf.ShowDialog();

            LoadMap();

            Army a = tm.GetArmy(clickCoords);

            if (a != null)
            {
                int levels = a.Size / 100;

                t.Downgrade(levels);

                LoadMap();
            }

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsCastle_MusterArmy_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Castle c = tm.GetCastle(clickCoords);
            object[] arr = new object[c.Level - 2];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (i + 2) * 400;
            }

            NewArmyForm naf = new NewArmyForm(turn.CurrentPlayer.Name, clickCoords, arr);

            naf.ShowDialog();

            LoadMap();

            Army a = tm.GetArmy(clickCoords);

            if (a != null)
            {
                int levels = a.Size / 400;

                c.Downgrade(levels);

                LoadMap();
            }

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsCastle_UpgradeCastle_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Castle c = tm.GetCastle(clickCoords);
            turn.CurrentPlayer.UpgradeCastle(c);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsCastle_UpgradeWalls_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Castle c = tm.GetCastle(clickCoords);

            turn.CurrentPlayer.UpgradeWalls(c);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsCastle_UpgradeTowers_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Castle c = tm.GetCastle(clickCoords);

            turn.CurrentPlayer.UpgradeTowers(c);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsCastle_UpgradeGatehouse_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Castle c = tm.GetCastle(clickCoords);

            turn.CurrentPlayer.UpgradeGatehouse(c);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
            UpdateResources();
        }

        private void cmsArmyTown_IncreaseArmy_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Army a = tm.GetArmy(clickCoords);
            Town t = tm.GetTown(clickCoords);

            a.Size += 100;
            t.Downgrade(1);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
        }

        private void cmsArmyCastle_IncreaseArmy_Click(object sender, EventArgs e)
        {
            playerDoc.Save(Player.UndoPath);
            btnUndo.Enabled = true;

            Army a = tm.GetArmy(clickCoords);
            Castle c = tm.GetCastle(clickCoords);

            a.Size += 400;
            c.Downgrade(1);

            playerDoc = XDocument.Load(Player.SavePath);
            UpdateSummary();
        }

        private void btnResolve_Click(object sender, EventArgs e)
        {
            ResolveBattleForm rb = new ResolveBattleForm();
            rb.ShowDialog();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tm.Objects = new List<MapObject>();
            newObject = null;
            newObjects = new List<MapObject>();

            playerDoc.XPathSelectElements("Players/Player").Remove();
            playerDoc.Save(Player.SavePath);

            tileDoc.XPathSelectElements("Tiles/Tile").Remove();
            tileDoc.Save(TerrainGridHex.SavePath);

            players = new List<Player>();

            String password = Player.EncryptPassword("a");

            players.Add(new Player("Red Player", 1, "Ogres", password, true, 0, 0));
            players.Add(new Player("Blue Player", 2, "High Elves", password, true, 0, 0));

            List<Resource> res1 = new List<Resource>() { new Resource("Food", 60), new Resource("Magic", 1) };
            List<Resource> res2 = new List<Resource>() { new Resource("Gold", 50), new Resource("Magic", 1) };

            Army a1, a2, a3, a4;

            a1 = new Army(25, 5, "Army 1", "Builder", 4, Pens.Red, 0, "Red Player", true, @"Races\Ogres\builder.png", null);
            tm.AddObject(a1);

            a2 = new Army(8, 8, "Army 2", "Flying", 10, Pens.Blue, 2000, "Blue Player", false, @"Races\HighElves\flying.png", res2);
            tm.AddObject(a2);

            a3 = new Army(14, 3, "Army 3", "Monstrous", 6, Pens.Red, 1500, "Red Player", false, @"Races\Ogres\monstrous.png", res1);
            tm.AddObject(a3);

            a4 = new Army(3, 14, "Army 4", "Builder", 5, Pens.Blue, 0, "Blue Player", true, @"Races\HighElves\builder.png", null);
            tm.AddObject(a4);

            players.ElementAt(0).AddArmy(a1);
            players.ElementAt(1).AddArmy(a2);
            players.ElementAt(0).AddArmy(a3);
            players.ElementAt(1).AddArmy(a4);

            Town t1 = new Town("Town 1", 3, 12, @"Terrain\Possible Buildings or Encampments\human-tile.png", "Red Player");
            t1.Upgrade(10);
            t1.LevelChanged = false;

            tm.AddObject(t1);
            players.ElementAt(0).AddTown(t1);

            Town t2 = new Town("Town 2", 8, 12, @"Terrain\Possible Buildings or Encampments\human-tile.png", "Blue Player");
            t2.Upgrade(5);
            t2.LevelChanged = false;

            tm.AddObject(t2);
            players.ElementAt(1).AddTown(t2);

            Watchtower wt1 = new Watchtower("Tower 1", 5, 7, "Blue Player", @"Terrain\Possible Buildings or Encampments\elven-tile.png");
            tm.AddObject(wt1);
            players.ElementAt(1).AddWatchtower(wt1);

            Castle c1 = new Castle("Castle 1", 20, 13, @"Terrain\Possible Buildings or Encampments\keep-tile.png", "Blue Player");
            c1.Upgrade(6);
            tm.AddObject(c1);
            players.ElementAt(1).AddCastle(c1);

            hexgridPanel1.MapBuffer = null;
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(5, 7)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(6, 6)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(7, 6)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(8, 5)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(9, 5)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(10, 4)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(11, 4)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(12, 3)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(13, 3)]).SpecialType = "Road";
            ((TerrainGridHex)tm.BoardHexes[HexCoords.NewUserCoords(14, 3)]).SpecialType = "Road";

            turn = new Turn(players);

            tm.SelectedArmy = null;
            clickCoords = HexCoords.NewUserCoords(-1, -1);

            Refresh();

            playerDoc = XDocument.Load(Player.SavePath);
            playerDoc.Save(Player.UndoPath);

            UpdateResources();
            UpdateSummary();
            UpdateLos();
        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {

            minimapX = panel1.HorizontalScroll.Value;
            minimapY = panel1.VerticalScroll.Value;

            UpdateMinimapRelated();

            turn.CurrentPlayer.ScrollPosHor = panel1.HorizontalScroll.Value;
            turn.CurrentPlayer.ScrollPosVer = panel1.VerticalScroll.Value;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            //Scrollbar positions don't update correctly after first set, thus the need for a second set
            panel1.VerticalScroll.Value = Math.Min(turn.CurrentPlayer.ScrollPosVer, panel1.VerticalScroll.Maximum);
            panel1.VerticalScroll.Value = Math.Min(turn.CurrentPlayer.ScrollPosVer, panel1.VerticalScroll.Maximum);
            panel1.HorizontalScroll.Value = Math.Min(turn.CurrentPlayer.ScrollPosHor, panel1.VerticalScroll.Maximum);
            panel1.HorizontalScroll.Value = Math.Min(turn.CurrentPlayer.ScrollPosHor, panel1.VerticalScroll.Maximum);

            minimapX = panel1.HorizontalScroll.Value;
            minimapY = panel1.VerticalScroll.Value;

            UpdateMinimapRelated();
        }

        private void pbMinimap_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Rectangle r1 = panel1.ClientRectangle;

                int x = Math.Max((int)(e.X / minimapScale - r1.Width / 2), 0);
                int y = Math.Max((int)(e.Y / minimapScale - r1.Height / 2), 0);

                x = Math.Min(x, (int)(pbMinimap.Width / minimapScale) - r1.Width);
                y = Math.Min(y, (int)(pbMinimap.Height / minimapScale) - r1.Height);

                minimapX = x;
                minimapY = y;

                UpdateMinimapRelated();

                panel1.HorizontalScroll.Value = Math.Max(0, x);
                panel1.HorizontalScroll.Value = Math.Max(0, x);
                panel1.VerticalScroll.Value = Math.Max(0, y);
                panel1.VerticalScroll.Value = Math.Max(0, y);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            UpdateMinimapRelated();

            Properties.Settings.Default.State = this.WindowState;
            if (this.WindowState == FormWindowState.Normal) Properties.Settings.Default.Size = this.Size;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();

            if (newObject != null)
            {
                newObject.Coords = HexCoords.NewUserCoords(-1, -1);
            }

            foreach (Player p in turn.Players)
            {
                p.Save();
            }
        }

        private void MainForm_LocationChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) Properties.Settings.Default.Location = this.Location;
        }

        public override void Refresh()
        {
            base.Refresh();

            Bitmap bmp = new Bitmap(hexgridPanel1.Width, hexgridPanel1.Height);
            hexgridPanel1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            bmp = new Bitmap(bmp, pbMinimap.Width, pbMinimap.Height);

            pbMinimap.Image = bmp;

            pbMinimap.Refresh();
        }
    }
}

