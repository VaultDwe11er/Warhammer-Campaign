using Misc;
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

namespace Warhammer_Campaign
{
    public partial class StartForm : Form
    {

        public StartForm()
        {
            InitializeComponent();

            LoadConfig();

            //Find current campaign
            XDocument xDoc = XDocument.Load(Turn.SavePath);

            if (xDoc.XPathSelectElements("Turn").Nodes().Count() == 0)
            {
                btnResume.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void LoadConfig()
        {
            int i;
            IEnumerable<XElement> elem;

            //Town
            XDocument townDoc = XDocument.Load(Town.ConfigPath);

            elem = townDoc.XPathSelectElements("Town/Obstacles/Level");
            Town.CoverLevels = new String[elem.Count()];
            Town.CoverLevelCosts = new String[elem.Count()];
            i = 0;
            
            foreach (var e in elem)
            {
                Town.CoverLevels[i] = e.Attribute("Name").Value;
                if (i > 0)
                {
                    Town.CoverLevelCosts[i] = e.Attribute("CostType").Value;
                }
                else Town.CoverLevelCosts[i] = "";
                i++;
            }
            
            elem = townDoc.XPathSelectElements("Town/Walls/Level");
            Town.WallLevels = new String[elem.Count()];
            Town.WallLevelCosts = new String[elem.Count()];
            i = 0;
            foreach (var e in elem)
            {
                Town.WallLevels[i] = e.Attribute("Name").Value;
                if (i > 0)
                {
                    Town.WallLevelCosts[i] = e.Attribute("CostType").Value;
                }
                else Town.WallLevelCosts[i] = "";
                i++;
            }

            elem = townDoc.XPathSelectElements("Town/Towers/Level");
            Town.TowerLevels = new String[elem.Count()];
            Town.TowerLevelCosts = new String[elem.Count()];
            i = 0;
            foreach (var e in elem)
            {
                Town.TowerLevels[i] = e.Attribute("Name").Value;
                if (i > 0)
                {
                    Town.TowerLevelCosts[i] = e.Attribute("CostType").Value;
                }
                else Town.TowerLevelCosts[i] = "";
                i++;
            }

            //Castle
            XDocument castleDoc = XDocument.Load(Castle.ConfigPath);

            elem = castleDoc.XPathSelectElements("Castle/Walls/Level");
            Castle.WallLevels = new String[elem.Count()];
            Castle.WallLevelCosts = new String[elem.Count()];
            i = 0;
            foreach (var e in elem)
            {
                Castle.WallLevels[i] = e.Attribute("Name").Value;
                if (i > 0)
                {
                    Castle.WallLevelCosts[i] = e.Attribute("CostType").Value;
                }
                else Castle.WallLevelCosts[i] = "";
                i++;
            }

            elem = castleDoc.XPathSelectElements("Castle/Towers/Level");
            Castle.TowerLevels = new String[elem.Count()];
            Castle.TowerLevelCosts = new String[elem.Count()];
            i = 0;
            foreach (var e in elem)
            {
                Castle.TowerLevels[i] = e.Attribute("Name").Value;
                if (i > 0)
                {
                    Castle.TowerLevelCosts[i] = e.Attribute("CostType").Value;
                }
                else Castle.TowerLevelCosts[i] = "";
                i++;
            }

            elem = castleDoc.XPathSelectElements("Castle/Gatehouse/Level");
            Castle.GatehouseLevels = new String[elem.Count()];
            Castle.GatehouseLevelCosts = new String[elem.Count()];
            i = 0;
            foreach (var e in elem)
            {
                Castle.GatehouseLevels[i] = e.Attribute("Name").Value;
                if (i > 0)
                {
                    Castle.GatehouseLevelCosts[i] = e.Attribute("CostType").Value;
                }
                else Castle.GatehouseLevelCosts[i] = "";
                i++;
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            if (tbUserName.Text == "")
            {
                MessageBox.Show("Please supply a user name");
                return;
            }
            if (tbPassword.Text == "" && tbUserName.Text != "Admin")
            {
                MessageBox.Show("Please supply a password");
                return;
            }

            XDocument xDoc = XDocument.Load(Player.SavePath);

            String password = Player.EncryptPassword(tbPassword.Text);

            String xPath = "Players/Player[Name= \"" + tbUserName.Text + "\" and Password = \"" + password + "\"]";
            int cnt = xDoc.XPathSelectElements(xPath).Count();

            if(tbUserName.Text == "Admin")
            {
                MainForm mf = new MainForm();
                this.Hide();
                mf.ShowDialog();
                this.Dispose();
            }
            else if (cnt == 0)
            {
                MessageBox.Show("User name or password incorrect");
            }
            else
            {
                MainForm mf = new MainForm(tbUserName.Text);
                this.Hide();
                mf.ShowDialog();
                this.Dispose();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            XDocument playerDoc = XDocument.Load(Player.SavePath);
            XDocument turnDoc = XDocument.Load(Turn.SavePath);

            if (turnDoc.XPathSelectElements("Turn").Nodes().Count() > 0)
            {
                if (MessageBox.Show("About to delete any existing campaign. Are you sure?", "Warning", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            playerDoc.XPathSelectElements("Players").Nodes().Remove();
            playerDoc.Save(Player.SavePath);

            turnDoc.XPathSelectElements("Turn").Nodes().Remove();
            turnDoc.Save(Turn.SavePath);

            CampaignForm mf = new CampaignForm();
            this.Hide();
            mf.ShowDialog();
            this.Dispose();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            XDocument xDoc = XDocument.Load(Turn.SavePath);

            if (bool.Parse(xDoc.XPathSelectElement("Turn/IsStartOfTurn").Value))
            {
                CampaignForm cf = new CampaignForm();
                this.Hide();
                cf.ShowDialog();
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Can't edit campaign in the middle of a turn");
            }
        }
    }
}
