using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Misc;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Warhammer_Campaign
{
    public partial class PlayerForm : Form
    {
        XDocument xDoc;
        int _pos;

        public PlayerForm(int pos)
        {
            InitializeComponent();

            xDoc = XDocument.Load(Player.SavePath);
            _pos = pos;
        }

        private void btnAddPlayer_Click(object sender, EventArgs e)
        {
            if (tbPlayerName.Text == "")
            {
                MessageBox.Show("No player name entered");
                return;
            }

            if (tbPlayerName.Text == "Admin")
            {
                MessageBox.Show("Username is reserved for other uses");
                return;
            }

            if (tbPassword.Text == "")
            {
                MessageBox.Show("No password entered");
                return;
            }

            String xpath = "Players/Player[Name=\"" + tbPlayerName.Text + "\"]";

            if (xDoc.XPathSelectElements(xpath).Elements().Count() > 0)
            {
                MessageBox.Show("Player with that name already exists");
                return;
            }

            if (lbArmies.SelectedItems.Count == 0)
            {
                MessageBox.Show("No army selected");
                return;
            }

            Player p = new Player(tbPlayerName.Text, _pos, lbArmies.SelectedItem.ToString(), Player.EncryptPassword(tbPassword.Text), true, 0, 0);

            p.Save();

            this.Dispose();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
