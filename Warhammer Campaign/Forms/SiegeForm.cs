using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warhammer_Campaign
{
    public partial class SiegeForm : Form
    {
        public SiegeForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lbSiegeType.SelectedItems.Count == 0)
            {
                MessageBox.Show("No option selected");
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public int CustomShowDialog()
        {
            ShowDialog();

            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                return lbSiegeType.SelectedIndex;
            }
            return -1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
