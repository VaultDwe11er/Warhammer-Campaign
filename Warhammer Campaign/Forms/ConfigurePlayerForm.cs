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
using HexUtilities;

namespace Warhammer_Campaign
{
    public partial class ConfigurePlayerForm : Form
    {
        String playerName;
        XDocument xDoc;
        String xPath;

        public ConfigurePlayerForm(String pn)
        {
            InitializeComponent();

            playerName = pn;
            xDoc = XDocument.Load(Player.SavePath);
            xPath = "Players/Player[Name=\"" + playerName + "\"]";

            lblPlayerName.Text = playerName;

            PopulateListBoxes();
        }

        public void PopulateListBoxes()
        {
            while (lbArmies.Items.Count > 0)
            {
                lbArmies.Items.RemoveAt(0);
            }

            while (lbBuildings.Items.Count > 0)
            {
                lbBuildings.Items.RemoveAt(0);
            }

            foreach (var e in xDoc.XPathSelectElements(xPath + "/Armies/Army"))
            {
                lbArmies.Items.Add(e.Element("Name").Value);
            }

            foreach (var e in xDoc.XPathSelectElements(xPath + "/Buildings/*"))
            {
                lbBuildings.Items.Add(e.Element("Name").Value);
            }
        }

        private void btnAddArmy_Click(object sender, EventArgs e)
        {
            object[] arr = new object[76];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (i + 5) * 100;
            }

            NewArmyForm naf = new NewArmyForm(playerName, HexCoords.NewUserCoords(-1, -1), arr);
            naf.ShowDialog();

            xDoc = XDocument.Load(Player.SavePath);
            PopulateListBoxes();
        }

        private void btnRemoveArmy_Click(object sender, EventArgs e)
        {
            if (lbArmies.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an army to remove");
                return;
            }

            String name = lbArmies.SelectedItem.ToString();
            String xp = xPath + "/Armies/Army[Name=\"" + name + "\"]";
            int pos = lbArmies.SelectedIndex;

            xDoc.XPathSelectElement(xp).Remove();

            xDoc.Save(Player.SavePath);

            lbArmies.Items.RemoveAt(pos);
        }

        private void btnAddBuilding_Click(object sender, EventArgs e)
        {
            NewBuildingForm nbf = new NewBuildingForm(playerName, HexCoords.NewUserCoords(-1, -1), new object[] { "Town", "Watchtower", "Castle" }, null);
            nbf.ShowDialog();

            xDoc = XDocument.Load(Player.SavePath);
            PopulateListBoxes();
        }

        private void btnRemoveBuiding_Click(object sender, EventArgs e)
        {
            if (lbBuildings.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a building to remove");
                return;
            }

            String name = lbBuildings.SelectedItem.ToString();
            String xp = xPath + "/Buildings/Town[Name=\"" + name + "\"]";
            int pos = lbBuildings.SelectedIndex;

            xDoc.XPathSelectElement(xp).Remove();

            xDoc.Save(Player.SavePath);

            lbBuildings.Items.RemoveAt(pos);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            CampaignForm cf = new CampaignForm();
            this.Hide();
            cf.ShowDialog();
            this.Dispose();
        }
    }
}
