using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Linq;
using Races;
using Terrain;
using System.Net.Mail;
using System.Net;

namespace Misc
{
    public class Player
    {
        public static String SavePath = @"Save\players.xml";
        public static String UndoPath = @"Save\players_bak.xml";

        private String _name;
        private List<Army> _armies;
        private List<Town> _towns;
        private List<Watchtower> _watchtowers;
        private List<Castle> _castles;
        private int _pos;
        private Race _race;
        private String _password;
        private int _scrollPosVer, _scrollPosHor;
        private String _emailAddress;

        public Player(String name, int pos, String race, String encryptedPassword, bool isNewPlayer, int hor, int ver)
        {
            _name = name;
            _pos = pos;
            _armies = new List<Army>();
            _towns = new List<Town>();
            _watchtowers = new List<Watchtower>();
            _castles = new List<Castle>();
            _password = encryptedPassword;
            _scrollPosVer = ver;
            _scrollPosHor = hor;

            switch(race)
            {
                case "Ogres":
                    _race = new OgresRace();
                    break;
                case "High Elves":
                    _race = new HighElvesRace();
                    break;
                default:
                    _race = new Race();
                    break;
            }

            if (isNewPlayer) Save();
        }

        public String Name
        {
            get { return _name; }
        }

        public int Position
        {
            get { return _pos; }
        }

        public int ScrollPosVer
        {
            get { return _scrollPosVer; }
            set { _scrollPosVer = value; }
        }

        public int ScrollPosHor
        {
            get { return _scrollPosHor; }
            set { _scrollPosHor = value; }
        }

        public String RaceType
        {
            get { return _race.RaceType; }
        }

        public Race Race
        {
            get { return _race; }
        }

        public List<Resource> Resources
        {
            get { return _race.Resources; }
        }

        public List<Army> Armies
        {
            get { return _armies; }
        }

        public List<Town> Towns
        {
            get { return _towns; }
        }

        public List<Watchtower> Watchtowers
        {
            get { return _watchtowers; }
        }

        public List<Castle> Castles
        {
            get { return _castles; }
        }

        public static String EncryptPassword(String password)
        {
            String val = "";

            foreach (char c in password)
            {
                int i = (int)c;
                String s;

                i = i * 3 - 2;
                s = i.ToString();
                s = s.PadLeft(3, '0');

                val += s;
            }

            val = String.Join(String.Empty,val.Reverse().ToArray());

            return val;
        }
        /*
        public String DecryptPassword(String encryptedPassword)
        {
            if (encryptedPassword.Length % 3 != 0) throw new Exception();

            String val = "";
            int n = encryptedPassword.Length / 3;

            for(int i = 0; i<n;i++)
            {
                String s = encryptedPassword.Substring(i * 3, 3);
                int j = int.Parse(s);

                val += (char)j;
            }

            return val;
        }
        
        public bool IsValidEncryption(String password)
        {
            bool val = true;

            try
            {
                DecryptPassword(password);
            }
            catch
            {
                val = false;
            }

            return val;
        }
        */
        public void AddArmy(Army a)
        {
            _armies.Add(a);
        }

        public void AddTown(Town t)
        {
            _towns.Add(t);
        }

        public void AddWatchtower(Watchtower wt)
        {
            _watchtowers.Add(wt);
        }

        public void AddCastle(Castle c)
        {
            _castles.Add(c);
        }

        public void UpgradeTown(Town t)
        {
            t.Upgrade(1);
            _race.Pay("UpgradeTown");

            Save();
        }
        public void UpgradeCastle(Castle c)
        {
            c.Upgrade(1);
            _race.Pay("UpgradeCastle");

            Save();
        }


        public void UpgradeCover(Town t)
        {
            int pos = Array.IndexOf(Town.CoverLevels, t.Cover) + 1;

            t.Cover = Town.CoverLevels[pos];
            _race.Pay(Town.CoverLevelCosts[pos]);

            Save();
        }

        public void UpgradeWalls(Town t)
        {
            int pos = Array.IndexOf(Town.WallLevels, t.Walls) + 1;

            t.Walls = Town.WallLevels[pos];
            _race.Pay(Town.WallLevelCosts[pos]);

            Save();
        }

        public void UpgradeTowers(Town t)
        {
            int pos = Array.IndexOf(Town.TowerLevels, t.Towers) + 1;

            t.Towers = Town.TowerLevels[pos];
            _race.Pay(Town.TowerLevelCosts[pos]);

            Save();
        }

        public void UpgradeWalls(Castle c)
        {
            int pos = Array.IndexOf(Castle.WallLevels, c.Walls) + 1;

            c.Walls = Castle.WallLevels[pos];
            _race.Pay(Castle.WallLevelCosts[pos]);

            Save();
        }

        public void UpgradeTowers(Castle c)
        {
            int pos = Array.IndexOf(Castle.TowerLevels, c.Towers) + 1;

            c.Towers = Castle.TowerLevels[pos];
            _race.Pay(Castle.TowerLevelCosts[pos]);

            Save();
        }
        public void UpgradeGatehouse(Castle c)
        {
            int pos = Array.IndexOf(Castle.GatehouseLevels, c.Gatehouse) + 1;

            c.Gatehouse = Castle.GatehouseLevels[pos];
            _race.Pay(Castle.GatehouseLevelCosts[pos]);

            Save();
        }


        public void BuildTown()
        {
            _race.Pay("BuildTown");

            Save();
        }

        public void BuildWatchtower()
        {
            _race.Pay("BuildWatchtower");

            Save();
        }

        public void BuildCastle()
        {
            _race.Pay("BuildCastle");

            Save();
        }

        public void BuildRoad()
        {
            _race.Pay("Road");

            Save();
        }

        public List<object> BuildableBuildings
        {
            get
            {
                List<object> retVal = new List<object>();

                if (this.Race.CanPay("BuildTown")) retVal.Add("Town");
                if (this.Race.CanPay("BuildWatchtower")) retVal.Add("Watchtower");
                if (this.Race.CanPay("BuildCastle")) retVal.Add("Castle");

                return retVal;
            }
        }

        public List<TerrainGridHex> ResourceHexes(TerrainMap tm)
        {
            List<TerrainGridHex> retVal = new List<TerrainGridHex>();

            foreach (Town t in Towns.Where(val => !val.IsUnderSiege))
            {
                foreach (TerrainGridHex hex in t.ResourceHexes(tm))
                {
                    if (!retVal.Contains(hex)) retVal.Add(hex);
                }
            }

            foreach (Castle c in Castles.Where(val => !val.IsUnderSiege))
            {
                foreach (TerrainGridHex hex in c.ResourceHexes(tm))
                {
                    if (!retVal.Contains(hex)) retVal.Add(hex);
                }
            }

            return retVal;
        }

        public bool CanUpgradeCover(Town t)
        {
            int i = Array.IndexOf(Town.CoverLevels, t.Cover) + 1;
            return this.Race.CanPay(Town.CoverLevelCosts[i]);
        }

        public bool CanUpgradeWalls(Town t)
        {
            int i = Array.IndexOf(Town.WallLevels, t.Walls) + 1;
            return this.Race.CanPay(Town.WallLevelCosts[i]);
        }

        public bool CanUpgradeTowers(Town t)
        {
            int i = Array.IndexOf(Town.TowerLevels, t.Towers) + 1;
            return this.Race.CanPay(Town.TowerLevelCosts[i]);
        }

        public bool CanUpgradeWalls(Castle c)
        {
            int i = Array.IndexOf(Castle.WallLevels, c.Walls) + 1;
            return this.Race.CanPay(Castle.WallLevelCosts[i]);
        }

        public bool CanUpgradeTowers(Castle c)
        {
            int i = Array.IndexOf(Castle.TowerLevels, c.Towers) + 1;
            return this.Race.CanPay(Castle.TowerLevelCosts[i]);
        }

        public bool CanUpgradeGatehouse(Castle c)
        {
            int i = Array.IndexOf(Castle.GatehouseLevels, c.Gatehouse) + 1;
            return this.Race.CanPay(Castle.GatehouseLevelCosts[i]);
        }

        public void ResetTurn()
        {
            foreach (Army a in _armies)
            {
                a.ResetMovement();
            }

            foreach (Town t in _towns)
            {
                t.LevelChanged = false;
            }

            Save();
        }

        public void SendEmail(String head, String body)
        {
            if (_emailAddress != null)
            {
                MailMessage mail = new MailMessage("qtotto@gmail.com", _emailAddress);
                SmtpClient client = new SmtpClient();
                mail.Subject = head;
                mail.Body = body;
                client.Send(mail);
            }
        }

        public void Save()
        {
            XDocument xDoc = XDocument.Load(Player.SavePath);
            String xpath = "Players/Player[Name=\"" + _name + "\"]";

            if (xDoc.XPathSelectElements(xpath).Elements().Count() == 0)
            {
                XElement xElem = new XElement("Player");
                xElem.Add(new XElement("Name", _name));
                xElem.Add(new XElement("Position", _pos));
                xElem.Add(new XElement("Race", this.RaceType));
                xElem.Add(new XElement("Password", _password));
                xElem.Add(new XElement("Horizontal", _scrollPosHor));
                xElem.Add(new XElement("Vertical", _scrollPosVer));
                xElem.Add(new XElement("Armies"));
                xElem.Add(new XElement("Buildings"));
                xElem.Add(new XElement("Resources"));

                xDoc.XPathSelectElement("Players").Add(xElem);
            }
            else
            {
                xDoc.XPathSelectElement(xpath).Element("Position").Value = _pos.ToString();
                xDoc.XPathSelectElement(xpath).Element("Race").Value = this.RaceType;
                xDoc.XPathSelectElement(xpath).Element("Password").Value = _password;
                xDoc.XPathSelectElement(xpath).Element("Horizontal").Value = _scrollPosHor.ToString();
                xDoc.XPathSelectElement(xpath).Element("Vertical").Value = _scrollPosVer.ToString();
            }

            _race.Save(xDoc, xpath + "/Resources");

            xDoc.Save(Player.SavePath);

            foreach (Army a in _armies)
            {
                a.Save();
            }

            foreach (Town t in _towns)
            {
                t.Save();
            }
        }
    }

    public class PlayerComp : Comparer<Player>
    {
        public override int Compare(Player x, Player y)
        {
            return x.Position.CompareTo(y.Position);
        }
    }
}
