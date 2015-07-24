using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Misc
{
    public class Turn
    {
        public static String SavePath = @"Save\turn.xml";
        
        private List<Player> _players;
        private Player _currentPlayer;
        private int _monthNo;
        private int _yearNo;
        private bool _isStartOfTurn;
        private String _xPath;

        public Turn(List<Player> players)
        {
            _players = players;
            _isStartOfTurn = true;
            _yearNo = 1;
            _monthNo = 1;
            
            _players.Sort(new PlayerComp());
            _currentPlayer = null;

            _xPath = "Turn";

            Save();
        }

        public Turn(List<Player> players, String currentPlayer, int yearNo, int monthNo, bool isStartOfTurn)
        {
            _players = players;
            _isStartOfTurn = isStartOfTurn;
            _yearNo = yearNo;
            _monthNo = monthNo;

            _players.Sort(new PlayerComp());
            _currentPlayer = (currentPlayer == "" ? null : _players.Find((val) => (val.Name == currentPlayer)));

            _xPath = "Turn";
        }

        public Player CurrentPlayer
        {
            get { return _currentPlayer ?? new Player("Admin", 0, "", "", false, 0, 0); }
        }

        public int YearNo
        {
            get { return _yearNo; }
        }

        public int MonthNo
        {
            get { return _monthNo; }
        }

        public bool IsStartOfTurn
        {
            get { return _isStartOfTurn; }
        }

        public List<Player> Players
        {
            get { return _players; }
        }

        public void MoveNextPlayer()
        {
            if (_currentPlayer == null)
            {
                _currentPlayer = _players.First();
                _isStartOfTurn = false;

                _currentPlayer.ResetTurn();
            }
            else if (_currentPlayer == _players.Last())
            {
                _currentPlayer = null;
                _isStartOfTurn = true;

                if (_monthNo < 12)
                {
                    _monthNo++;
                }
                else
                {
                    _yearNo++;
                    _monthNo = 1;
                }
            }
            else
            {
                _currentPlayer = _players.ElementAt(_currentPlayer.Position);

                _currentPlayer.ResetTurn();
            }
            //Start of turn events
            if (_currentPlayer != null)
            {
                _currentPlayer.Towns.Where(t => t.IsUnderSiege).ToList().ForEach(t => t.Downgrade(1));

                _currentPlayer.SendEmail("Warhammer Campaign", "It is your turn");
            }

            Save();
        }

        public void Save()
        {
            XDocument xDoc = XDocument.Load(Turn.SavePath);

            if (xDoc.XPathSelectElements(_xPath).Elements().Count() == 0)
            {
                XElement xElem = new XElement("Turn");
                xElem.Add(new XElement("CurrentPlayer", (_currentPlayer == null ? "" : _currentPlayer.Name)));
                xElem.Add(new XElement("YearNo", _yearNo));
                xElem.Add(new XElement("MonthNo", _monthNo));
                xElem.Add(new XElement("IsStartOfTurn", _isStartOfTurn));

                xDoc = new XDocument(xElem);
            }
            else
            {
                xDoc.XPathSelectElement(_xPath).Elements("CurrentPlayer").ElementAtOrDefault(0).Value = (_currentPlayer == null ? "" : _currentPlayer.Name);
                xDoc.XPathSelectElement(_xPath).Elements("YearNo").ElementAtOrDefault(0).Value = _yearNo.ToString();
                xDoc.XPathSelectElement(_xPath).Elements("MonthNo").ElementAtOrDefault(0).Value = _monthNo.ToString();
                xDoc.XPathSelectElement(_xPath).Elements("IsStartOfTurn").ElementAtOrDefault(0).Value = _isStartOfTurn.ToString();
            }

            xDoc.Save(Turn.SavePath);
        }
    }
}
