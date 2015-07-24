using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;
using Misc;

namespace Warhammer_Campaign
{
    public partial class CampaignForm : Form
    {
        XDocument playerDoc, turnDoc;
        List<Player> players;

        public CampaignForm()
        {
            InitializeComponent();

            playerDoc = XDocument.Load(Player.SavePath);
            turnDoc = XDocument.Load(Turn.SavePath);

            players = new List<Player>();

            UpdatePlayerList();
        }

        private void UpdatePlayerList()
        {
            int cnt = playerDoc.XPathSelectElements("Players/Player").Count();

            while (lbPlayers.Items.Count > 0)
            {
                lbPlayers.Items.RemoveAt(0);
            }

            for (int i = 1; i <= cnt; i++)
            {
                var node = playerDoc.XPathSelectElement("Players/Player[Position=\"" + i.ToString() + "\"]");

                lbPlayers.Items.Add(node.Element("Name").Value);
            }

            btnStart.Enabled = (cnt > 1);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (turnDoc.XPathSelectElements("Turn").Nodes().Count() == 0)
            {
                var elem = playerDoc.XPathSelectElement("Players/Player[Position=1]");
                players.Add(new Player(elem.Element("Name").Value, 1, elem.Element("Race").Value, elem.Element("Password").Value, false,
                    int.Parse(elem.Element("Horizontal").Value), int.Parse(elem.Element("Vertical").Value)));

                Turn t = new Turn(players);
            }

            MainForm mf = new MainForm();
            this.Hide();
            mf.ShowDialog();
            this.Dispose();
        }

        private void btnAddPlayer_Click(object sender, EventArgs e)
        {
            PlayerForm mf = new PlayerForm(lbPlayers.Items.Count + 1);
            mf.ShowDialog();

            playerDoc = XDocument.Load(Player.SavePath);
            UpdatePlayerList();
        }

        private void btnRemovePlayer_Click(object sender, EventArgs e)
        {
            if (lbPlayers.SelectedItems.Count > 0)
            {
                String name = lbPlayers.SelectedItem.ToString();
                String xpath = "Players/Player[Name=\"" + name + "\"]";
                int pos = lbPlayers.SelectedIndex + 1;

                playerDoc.XPathSelectElement(xpath).Remove();

                for (int i = pos + 1; i <= playerDoc.XPathSelectElements("Players/Player").Count() + 1; i++)
                {
                    xpath = "Players/Player[Position=\"" + i.ToString() + "\"]";

                    playerDoc.XPathSelectElement(xpath).Element("Position").Value = (i - 1).ToString();
                }

                playerDoc.Save(Player.SavePath);

                UpdatePlayerList();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            StartForm sf = new StartForm();
            this.Hide();
            sf.ShowDialog();
            this.Dispose();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int pos = lbPlayers.SelectedIndex + 1;
            String name1, name2;
            String xpath;

            if (pos > 1)
            {
                name1 = lbPlayers.SelectedItem.ToString();
                name2 = lbPlayers.Items[pos - 2].ToString();

                xpath = "Players/Player[Name=\"" + name1 + "\"]";
                playerDoc.XPathSelectElement(xpath).Element("Position").Value = (pos - 1).ToString();

                xpath = "Players/Player[Name=\"" + name2 + "\"]";
                playerDoc.XPathSelectElement(xpath).Element("Position").Value = (pos).ToString();

                playerDoc.Save(Player.SavePath);

                UpdatePlayerList();

                lbPlayers.SetSelected(pos - 2, true);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int pos = lbPlayers.SelectedIndex + 1;
            String name1, name2;
            String xpath;

            if (pos > 0 && pos < lbPlayers.Items.Count)
            {
                name1 = lbPlayers.SelectedItem.ToString();
                name2 = lbPlayers.Items[pos].ToString();

                xpath = "Players/Player[Name=\"" + name1 + "\"]";
                playerDoc.XPathSelectElement(xpath).Element("Position").Value = (pos + 1).ToString();

                xpath = "Players/Player[Name=\"" + name2 + "\"]";
                playerDoc.XPathSelectElement(xpath).Element("Position").Value = (pos).ToString();

                playerDoc.Save(Player.SavePath);

                UpdatePlayerList();

                lbPlayers.SetSelected(pos, true);
            }
        }

        private void btnConfigurePlayer_Click(object sender, EventArgs e)
        {
            if (lbPlayers.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a player to configure");
            }
            else
            {
                String name = lbPlayers.SelectedItem.ToString();
                ConfigurePlayerForm cpf = new ConfigurePlayerForm(name);
                this.Hide();
                cpf.ShowDialog();
                this.Dispose();
            }
        }
    }
}
