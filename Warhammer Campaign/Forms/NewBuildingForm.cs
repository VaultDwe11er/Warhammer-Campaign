using Misc;
using HexUtilities;
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
using Races;

namespace Warhammer_Campaign
{
    public partial class NewBuildingForm : Form
    {
        String player;
        HexCoords coords;
        XDocument playerDoc;
        Race race;

        public NewBuildingForm(String p, HexCoords c, object[] arr, Race r)
        {
            InitializeComponent();

            cbBuildingType.Items.AddRange(arr);

            playerDoc = XDocument.Load(Player.SavePath);
            
            player = p;
            coords = c;
            race = r;

            lblCost.Text = "";
        }

        private void btnAddBuilding_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "")
            {
                MessageBox.Show("Select a name");
                return;
            }

            if (cbBuildingType.Text == "")
            {
                MessageBox.Show("Select a building type");
                return;
            }

            if (playerDoc.XPathSelectElements("Players/Player/Buildings/*[Name=\"" + tbName.Text + "\"]").Count() > 0)
            {
                MessageBox.Show("Building with that name already exists. Select another name");
                return;
            }

            String _name = tbName.Text;
            String _type = cbBuildingType.Text;
            int _x = coords.User.X;
            int _y = coords.User.Y;

            switch (_type)
            {
                case "Town":
                    Town t = new Town(_name, _x, _y, @"Terrain\Possible Buildings or Encampments\human-tile.png", player);
                    break;
                case "Watchtower":
                    Watchtower wt = new Watchtower(_name, _x, _y, player, @"Terrain\Possible Buildings or Encampments\elven-tile.png");
                    break;
                case "Castle":
                    Castle c = new Castle(_name, _x, _y, @"Terrain\Possible Buildings or Encampments\keep-tile.png", player);
                    break;
                default:
                    MessageBox.Show("Building type not defined yet");
                    break;
            }

            this.Close();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void cbBuildingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (race != null)
            {
                String costType = "Build" + cbBuildingType.SelectedItem.ToString();
                lblCost.Text = "Cost: " + race.GetCost(costType);
            }
        }
    }
}
