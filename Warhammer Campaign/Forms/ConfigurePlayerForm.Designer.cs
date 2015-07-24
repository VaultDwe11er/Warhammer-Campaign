namespace Warhammer_Campaign
{
    partial class ConfigurePlayerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.lbArmies = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddArmy = new System.Windows.Forms.Button();
            this.btnRemoveArmy = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbBuildings = new System.Windows.Forms.ListBox();
            this.btnAddBuilding = new System.Windows.Forms.Button();
            this.btnRemoveBuiding = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlayerName.Location = new System.Drawing.Point(12, 9);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(41, 13);
            this.lblPlayerName.TabIndex = 0;
            this.lblPlayerName.Text = "label1";
            // 
            // lbArmies
            // 
            this.lbArmies.FormattingEnabled = true;
            this.lbArmies.Location = new System.Drawing.Point(15, 54);
            this.lbArmies.Name = "lbArmies";
            this.lbArmies.Size = new System.Drawing.Size(202, 82);
            this.lbArmies.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Armies:";
            // 
            // btnAddArmy
            // 
            this.btnAddArmy.Location = new System.Drawing.Point(15, 143);
            this.btnAddArmy.Name = "btnAddArmy";
            this.btnAddArmy.Size = new System.Drawing.Size(98, 36);
            this.btnAddArmy.TabIndex = 3;
            this.btnAddArmy.Text = "Add army";
            this.btnAddArmy.UseVisualStyleBackColor = true;
            this.btnAddArmy.Click += new System.EventHandler(this.btnAddArmy_Click);
            // 
            // btnRemoveArmy
            // 
            this.btnRemoveArmy.Location = new System.Drawing.Point(119, 143);
            this.btnRemoveArmy.Name = "btnRemoveArmy";
            this.btnRemoveArmy.Size = new System.Drawing.Size(98, 36);
            this.btnRemoveArmy.TabIndex = 5;
            this.btnRemoveArmy.Text = "Remove army";
            this.btnRemoveArmy.UseVisualStyleBackColor = true;
            this.btnRemoveArmy.Click += new System.EventHandler(this.btnRemoveArmy_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Buildings:";
            // 
            // lbBuildings
            // 
            this.lbBuildings.FormattingEnabled = true;
            this.lbBuildings.Location = new System.Drawing.Point(15, 198);
            this.lbBuildings.Name = "lbBuildings";
            this.lbBuildings.Size = new System.Drawing.Size(202, 82);
            this.lbBuildings.TabIndex = 7;
            // 
            // btnAddBuilding
            // 
            this.btnAddBuilding.Location = new System.Drawing.Point(15, 286);
            this.btnAddBuilding.Name = "btnAddBuilding";
            this.btnAddBuilding.Size = new System.Drawing.Size(98, 36);
            this.btnAddBuilding.TabIndex = 8;
            this.btnAddBuilding.Text = "Add building";
            this.btnAddBuilding.UseVisualStyleBackColor = true;
            this.btnAddBuilding.Click += new System.EventHandler(this.btnAddBuilding_Click);
            // 
            // btnRemoveBuiding
            // 
            this.btnRemoveBuiding.Location = new System.Drawing.Point(119, 286);
            this.btnRemoveBuiding.Name = "btnRemoveBuiding";
            this.btnRemoveBuiding.Size = new System.Drawing.Size(98, 36);
            this.btnRemoveBuiding.TabIndex = 10;
            this.btnRemoveBuiding.Text = "Remove building";
            this.btnRemoveBuiding.UseVisualStyleBackColor = true;
            this.btnRemoveBuiding.Click += new System.EventHandler(this.btnRemoveBuiding_Click);
            // 
            // btnDone
            // 
            this.btnDone.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDone.Location = new System.Drawing.Point(58, 328);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(118, 33);
            this.btnDone.TabIndex = 11;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // ConfigurePlayerForm
            // 
            this.AcceptButton = this.btnDone;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnDone;
            this.ClientSize = new System.Drawing.Size(232, 370);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.btnRemoveBuiding);
            this.Controls.Add(this.btnAddBuilding);
            this.Controls.Add(this.lbBuildings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRemoveArmy);
            this.Controls.Add(this.btnAddArmy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbArmies);
            this.Controls.Add(this.lblPlayerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigurePlayerForm";
            this.Text = "Configure Player";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.ListBox lbArmies;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddArmy;
        private System.Windows.Forms.Button btnRemoveArmy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbBuildings;
        private System.Windows.Forms.Button btnAddBuilding;
        private System.Windows.Forms.Button btnRemoveBuiding;
        private System.Windows.Forms.Button btnDone;
    }
}