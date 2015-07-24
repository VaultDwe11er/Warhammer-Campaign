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
using Races;
using HexUtilities;

namespace Warhammer_Campaign
{
    public partial class NewArmyForm : Form
    {
        XDocument playerDoc, raceDoc;
        String playerName;
        Pen pen;
        //TODO: remove hardcoding
        Pen[] pens = { Pens.Red, Pens.Blue, Pens.Yellow, Pens.White, Pens.Black };
        HexCoords coords;

        public NewArmyForm(String pn, HexCoords c, object[] sizeArr)
        {
            InitializeComponent();

            playerName = pn;
            coords = c;

            playerDoc = XDocument.Load(Player.SavePath);
            String race = playerDoc.XPathSelectElement("Players/Player[Name=\"" + playerName + "\"]/Race").Value;
            raceDoc = XDocument.Load(Race.GetConfigDoc(race));
            
            foreach (var e in raceDoc.XPathSelectElements("Config/ArmyTypes/ArmyType"))
            {
                lbArmyTypes.Items.Add(e.Attribute("Type").Value);
            }

            cbSize.Items.AddRange(sizeArr);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbName.Text == "")
            {
                MessageBox.Show("Select an army name");
                return;
            }

            if (lbArmyTypes.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an army type");
                return;
            }

            if (cbSize.Text == "" && lbArmyTypes.SelectedItem.ToString() != "Builder")
            {
                MessageBox.Show("Select an army size");
                return;
            }

            if (playerDoc.XPathSelectElements("Players/Player/Armies/Army[Name=\"" + tbName.Text + "\"]").Count() > 0)
            {
                MessageBox.Show("Army with that name already exists. Select another name");
                return;
            }

            String name = tbName.Text;
            String type = lbArmyTypes.SelectedItem.ToString();
            int size;
            if (type != "Builder") size = int.Parse(cbSize.Text); else size = 0;
            int movement = int.Parse(raceDoc.XPathSelectElement("Config/ArmyTypes/ArmyType[@Type=\"" + type + "\"]").Attribute("Movement").Value);
            int x = coords.User.X;
            int y = coords.User.Y;
            bool canBuild = (type == "Builder");
            String imgPath = raceDoc.XPathSelectElement("Config/ArmyTypes/ArmyType[@Type=\"" + type + "\"]").Attribute("ImgPath").Value;
            int pos = int.Parse(playerDoc.XPathSelectElement("Players/Player[Name=\"" + playerName + "\"]").Element("Position").Value) - 1;
            pen = pens[pos];

            List<Resource> resourceList = new List<Resource>();
            String strUpkeep = raceDoc.XPathSelectElement("Config/ArmyTypes/ArmyType[@Type=\"" + type + "\"]").Attribute("UpkeepPer100pts").Value;
            String[] resArr = strUpkeep.Split(',');
            foreach (String val in resArr)
            {
                String[] tmpArr = val.Split('=');

                String _name = tmpArr[0];
                int _amount = int.Parse(tmpArr[1]) * size / 100;

                resourceList.Add(new Resource(_name, _amount));
            }

            Army a = new Army(x, y, name, type, movement, pen, size, playerName, canBuild, imgPath, resourceList);

            this.Close();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void lbArmyTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbArmyTypes.SelectedItem.ToString() == "Builder")
            {
                cbSize.Enabled = false;
            }
            else
            {
                cbSize.Enabled = true;
            }
        }
    }
}
