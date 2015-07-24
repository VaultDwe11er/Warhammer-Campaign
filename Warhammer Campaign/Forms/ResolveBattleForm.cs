using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HexUtilities;
using Misc;
using Terrain;
using HexUtilities.Common;

namespace Warhammer_Campaign
{
    public partial class ResolveBattleForm : Form
    {
        List<String> ArmyAttacking = new List<String>();
        List<String> ArmyAttacked = new List<String>();
        TerrainMap tm = MainForm.tm;

        public ResolveBattleForm()
        {
            InitializeComponent();

            foreach (var a in tm.Armies.FindAll((val) => val.UnitAttacked != null))
            {
                String army = a.Name;
                String unit = a.UnitAttacked;

                ArmyAttacking.Add(army);
                ArmyAttacked.Add(unit);
                lbBattles.Items.Add(army + " vs " + unit);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnResolve_Click(object sender, EventArgs e)
        {
            if (lbBattles.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a battle to resolve");
                return;
            }

            if (lbWinner.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select a result");
                return;
            }

            Army a1 = tm.Armies.Find((val) => val.Name == lbWinner.Items[0].ToString());
            Army a2 = tm.Armies.Find((val) => val.Name == lbWinner.Items[1].ToString());

            if (lbWinner.SelectedIndex == 0)
            {
                Hexside hs;
                List<NeighbourCoords> neighbours;
                NeighbourCoords neighbour;
                HexCoords toCoords;

                neighbours = a1.Coords.GetNeighbours().ToList();

                neighbour = neighbours.Find((val) => val.Coords == a2.Coords);

                hs = (Hexside)neighbours.IndexOf(neighbour);

                toCoords = a2.Coords.GetNeighbour(hs);

                a1.Move(a2.Coords, tm);
                a2.Move(toCoords, tm);
            }
            else if (lbWinner.SelectedIndex == 1)
            {
            }
            else if (lbWinner.SelectedIndex == 2)
            {
            }

            a1.EndAttack();

            this.Close();
            this.Dispose();
        }

        private void lbBattles_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (lbWinner.Items.Count > 0)
            {
                lbWinner.Items.RemoveAt(0);
            }

            if (lbBattles.SelectedItems.Count > 0)
            {
                lbWinner.Items.Add(ArmyAttacking[lbBattles.SelectedIndex]);
                lbWinner.Items.Add(ArmyAttacked[lbBattles.SelectedIndex]);
                lbWinner.Items.Add("Draw");
            }
        }
    }
}
