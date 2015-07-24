namespace Warhammer_Campaign
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.hexgridPanel1 = new Warhammer_Campaign.CustomHexgridPanel(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnEndTurn = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.cmsArmy = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsArmy_DisbandArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmy_BuildBuilding = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmy_BuildRoad = new System.Windows.Forms.ToolStripMenuItem();
            this.lblSummary = new System.Windows.Forms.Label();
            this.lblResources = new System.Windows.Forms.Label();
            this.cmsArmyTown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsArmyTown_DisbandArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyTown_BuildBuilding = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyTown_BuildRoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsArmyTown_IncreaseArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyTown_UpgradeTown = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyTown_UpgradeObstacles = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyTown_UpgradeWalls = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyTown_UpgradeTowers = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTown = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsTown_MusterArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTown_UpgradeTown = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTown_UpgradeObstacles = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTown_UpgradeWalls = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTown_UpgradeTowers = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnResolve = new System.Windows.Forms.Button();
            this.cmsCastle = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsCastle_MusterArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCastle_UpgradeCastle = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCastle_UpgradeWalls = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCastle_UpgradeTowers = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsCastle_UpgradeGatehouse = new System.Windows.Forms.ToolStripMenuItem();
            this.pbMinimap = new Warhammer_Campaign.Minimap();
            this.cmsArmyCastle = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsArmyCastle_DisbandArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyCastle_BuildBuilding = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyCastle_BuildRoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsArmyCastle_IncreaseArmy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyCastle_UpgradeCastle = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyCastle_UpgradeWalls = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyCastle_UpgradeTowers = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsArmyCastle_UpgradeGatehouse = new System.Windows.Forms.ToolStripMenuItem();
            this.cbPrevious = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hexgridPanel1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.cmsArmy.SuspendLayout();
            this.cmsArmyTown.SuspendLayout();
            this.cmsTown.SuspendLayout();
            this.cmsCastle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimap)).BeginInit();
            this.cmsArmyCastle.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.hexgridPanel1);
            this.panel1.Location = new System.Drawing.Point(12, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 435);
            this.panel1.TabIndex = 1;
            this.panel1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel1_Scroll);
            // 
            // hexgridPanel1
            // 
            this.hexgridPanel1.BackColor = System.Drawing.Color.Black;
            this.hexgridPanel1.Host = null;
            this.hexgridPanel1.IsTransposed = false;
            this.hexgridPanel1.Location = new System.Drawing.Point(3, 3);
            this.hexgridPanel1.Name = "hexgridPanel1";
            this.hexgridPanel1.ScaleIndex = 0;
            this.hexgridPanel1.Scales = null;
            this.hexgridPanel1.Size = new System.Drawing.Size(655, 429);
            this.hexgridPanel1.TabIndex = 0;
            this.hexgridPanel1.HotspotHexChange += new System.EventHandler<Custom.HexgridPanel.HexEventArgs>(this.hexgridPanel1_HotspotHexChange);
            this.hexgridPanel1.MouseLeftClick += new System.EventHandler<Custom.HexgridPanel.HexEventArgs>(this.hexgridPanel1_LeftClick);
            this.hexgridPanel1.MouseRightClick += new System.EventHandler<Custom.HexgridPanel.HexEventArgs>(this.hexgridPanel1_RightClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 483);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(898, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(99, 17);
            this.toolStripStatusLabel1.Text = "No army selected";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(784, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Start";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnEndTurn
            // 
            this.btnEndTurn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEndTurn.Location = new System.Drawing.Point(783, 25);
            this.btnEndTurn.Name = "btnEndTurn";
            this.btnEndTurn.Size = new System.Drawing.Size(98, 33);
            this.btnEndTurn.TabIndex = 3;
            this.btnEndTurn.Text = "End Turn";
            this.btnEndTurn.UseVisualStyleBackColor = true;
            this.btnEndTurn.Click += new System.EventHandler(this.btnEndTurn_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.Location = new System.Drawing.Point(679, 25);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(98, 33);
            this.btnUndo.TabIndex = 4;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // cmsArmy
            // 
            this.cmsArmy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsArmy_DisbandArmy,
            this.cmsArmy_BuildBuilding,
            this.cmsArmy_BuildRoad});
            this.cmsArmy.Name = "cmsArmy";
            this.cmsArmy.Size = new System.Drawing.Size(150, 70);
            // 
            // cmsArmy_DisbandArmy
            // 
            this.cmsArmy_DisbandArmy.Name = "cmsArmy_DisbandArmy";
            this.cmsArmy_DisbandArmy.Size = new System.Drawing.Size(149, 22);
            this.cmsArmy_DisbandArmy.Text = "Disband Army";
            this.cmsArmy_DisbandArmy.Click += new System.EventHandler(this.cmsArmy_DisbandArmy_Click);
            // 
            // cmsArmy_BuildBuilding
            // 
            this.cmsArmy_BuildBuilding.Name = "cmsArmy_BuildBuilding";
            this.cmsArmy_BuildBuilding.Size = new System.Drawing.Size(149, 22);
            this.cmsArmy_BuildBuilding.Text = "Build Building";
            this.cmsArmy_BuildBuilding.Click += new System.EventHandler(this.cmsArmy_BuildBuilding_Click);
            // 
            // cmsArmy_BuildRoad
            // 
            this.cmsArmy_BuildRoad.Name = "cmsArmy_BuildRoad";
            this.cmsArmy_BuildRoad.Size = new System.Drawing.Size(149, 22);
            this.cmsArmy_BuildRoad.Text = "Build Road";
            this.cmsArmy_BuildRoad.Click += new System.EventHandler(this.cmsArmy_BuildRoad_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSummary.BackColor = System.Drawing.SystemColors.Control;
            this.lblSummary.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSummary.Location = new System.Drawing.Point(679, 142);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(197, 179);
            this.lblSummary.TabIndex = 5;
            // 
            // lblResources
            // 
            this.lblResources.AutoSize = true;
            this.lblResources.Location = new System.Drawing.Point(15, 13);
            this.lblResources.Name = "lblResources";
            this.lblResources.Size = new System.Drawing.Size(75, 13);
            this.lblResources.TabIndex = 6;
            this.lblResources.Text = "No Resources";
            // 
            // cmsArmyTown
            // 
            this.cmsArmyTown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsArmyTown_DisbandArmy,
            this.cmsArmyTown_BuildBuilding,
            this.cmsArmyTown_BuildRoad,
            this.toolStripSeparator1,
            this.cmsArmyTown_IncreaseArmy,
            this.cmsArmyTown_UpgradeTown,
            this.cmsArmyTown_UpgradeObstacles,
            this.cmsArmyTown_UpgradeWalls,
            this.cmsArmyTown_UpgradeTowers});
            this.cmsArmyTown.Name = "cmsArmy";
            this.cmsArmyTown.Size = new System.Drawing.Size(174, 186);
            // 
            // cmsArmyTown_DisbandArmy
            // 
            this.cmsArmyTown_DisbandArmy.Name = "cmsArmyTown_DisbandArmy";
            this.cmsArmyTown_DisbandArmy.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_DisbandArmy.Text = "Disband Army";
            this.cmsArmyTown_DisbandArmy.Click += new System.EventHandler(this.cmsArmy_DisbandArmy_Click);
            // 
            // cmsArmyTown_BuildBuilding
            // 
            this.cmsArmyTown_BuildBuilding.Name = "cmsArmyTown_BuildBuilding";
            this.cmsArmyTown_BuildBuilding.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_BuildBuilding.Text = "Build Building";
            this.cmsArmyTown_BuildBuilding.Click += new System.EventHandler(this.cmsArmy_BuildBuilding_Click);
            // 
            // cmsArmyTown_BuildRoad
            // 
            this.cmsArmyTown_BuildRoad.Name = "cmsArmyTown_BuildRoad";
            this.cmsArmyTown_BuildRoad.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_BuildRoad.Text = "Build Road";
            this.cmsArmyTown_BuildRoad.Click += new System.EventHandler(this.cmsArmy_BuildRoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(170, 6);
            // 
            // cmsArmyTown_IncreaseArmy
            // 
            this.cmsArmyTown_IncreaseArmy.Name = "cmsArmyTown_IncreaseArmy";
            this.cmsArmyTown_IncreaseArmy.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_IncreaseArmy.Text = "Increase Army";
            this.cmsArmyTown_IncreaseArmy.Click += new System.EventHandler(this.cmsArmyTown_IncreaseArmy_Click);
            // 
            // cmsArmyTown_UpgradeTown
            // 
            this.cmsArmyTown_UpgradeTown.Name = "cmsArmyTown_UpgradeTown";
            this.cmsArmyTown_UpgradeTown.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_UpgradeTown.Text = "Upgrade Town";
            this.cmsArmyTown_UpgradeTown.Click += new System.EventHandler(this.cmsTown_UpgradeTown_Click);
            // 
            // cmsArmyTown_UpgradeObstacles
            // 
            this.cmsArmyTown_UpgradeObstacles.Name = "cmsArmyTown_UpgradeObstacles";
            this.cmsArmyTown_UpgradeObstacles.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_UpgradeObstacles.Text = "Upgrade Obstacles";
            this.cmsArmyTown_UpgradeObstacles.Click += new System.EventHandler(this.cmsTown_UpgradeObstacles_Click);
            // 
            // cmsArmyTown_UpgradeWalls
            // 
            this.cmsArmyTown_UpgradeWalls.Name = "cmsArmyTown_UpgradeWalls";
            this.cmsArmyTown_UpgradeWalls.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_UpgradeWalls.Text = "Upgrade Walls";
            this.cmsArmyTown_UpgradeWalls.Click += new System.EventHandler(this.cmsTown_UpgradeWalls_Click);
            // 
            // cmsArmyTown_UpgradeTowers
            // 
            this.cmsArmyTown_UpgradeTowers.Name = "cmsArmyTown_UpgradeTowers";
            this.cmsArmyTown_UpgradeTowers.Size = new System.Drawing.Size(173, 22);
            this.cmsArmyTown_UpgradeTowers.Text = "Upgrade Towers";
            this.cmsArmyTown_UpgradeTowers.Click += new System.EventHandler(this.cmsTown_UpgradeTowers_Click);
            // 
            // cmsTown
            // 
            this.cmsTown.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsTown_MusterArmy,
            this.cmsTown_UpgradeTown,
            this.cmsTown_UpgradeObstacles,
            this.cmsTown_UpgradeWalls,
            this.cmsTown_UpgradeTowers});
            this.cmsTown.Name = "cmsArmy";
            this.cmsTown.Size = new System.Drawing.Size(174, 114);
            // 
            // cmsTown_MusterArmy
            // 
            this.cmsTown_MusterArmy.Name = "cmsTown_MusterArmy";
            this.cmsTown_MusterArmy.Size = new System.Drawing.Size(173, 22);
            this.cmsTown_MusterArmy.Text = "Muster Army";
            this.cmsTown_MusterArmy.Click += new System.EventHandler(this.cmsTown_MusterArmy_Click);
            // 
            // cmsTown_UpgradeTown
            // 
            this.cmsTown_UpgradeTown.Name = "cmsTown_UpgradeTown";
            this.cmsTown_UpgradeTown.Size = new System.Drawing.Size(173, 22);
            this.cmsTown_UpgradeTown.Text = "Upgrade Town";
            this.cmsTown_UpgradeTown.Click += new System.EventHandler(this.cmsTown_UpgradeTown_Click);
            // 
            // cmsTown_UpgradeObstacles
            // 
            this.cmsTown_UpgradeObstacles.Name = "cmsTown_UpgradeObstacles";
            this.cmsTown_UpgradeObstacles.Size = new System.Drawing.Size(173, 22);
            this.cmsTown_UpgradeObstacles.Text = "Upgrade Obstacles";
            this.cmsTown_UpgradeObstacles.Click += new System.EventHandler(this.cmsTown_UpgradeObstacles_Click);
            // 
            // cmsTown_UpgradeWalls
            // 
            this.cmsTown_UpgradeWalls.Name = "cmsTown_UpgradeWalls";
            this.cmsTown_UpgradeWalls.Size = new System.Drawing.Size(173, 22);
            this.cmsTown_UpgradeWalls.Text = "Upgrade Walls";
            this.cmsTown_UpgradeWalls.Click += new System.EventHandler(this.cmsTown_UpgradeWalls_Click);
            // 
            // cmsTown_UpgradeTowers
            // 
            this.cmsTown_UpgradeTowers.Name = "cmsTown_UpgradeTowers";
            this.cmsTown_UpgradeTowers.Size = new System.Drawing.Size(173, 22);
            this.cmsTown_UpgradeTowers.Text = "Upgrade Towers";
            this.cmsTown_UpgradeTowers.Click += new System.EventHandler(this.cmsTown_UpgradeTowers_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(783, 64);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(98, 33);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnResolve
            // 
            this.btnResolve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResolve.Location = new System.Drawing.Point(679, 64);
            this.btnResolve.Name = "btnResolve";
            this.btnResolve.Size = new System.Drawing.Size(98, 33);
            this.btnResolve.TabIndex = 8;
            this.btnResolve.Text = "Resolve Battle";
            this.btnResolve.UseVisualStyleBackColor = true;
            this.btnResolve.Click += new System.EventHandler(this.btnResolve_Click);
            // 
            // cmsCastle
            // 
            this.cmsCastle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsCastle_MusterArmy,
            this.cmsCastle_UpgradeCastle,
            this.cmsCastle_UpgradeWalls,
            this.cmsCastle_UpgradeTowers,
            this.cmsCastle_UpgradeGatehouse});
            this.cmsCastle.Name = "cmsArmy";
            this.cmsCastle.Size = new System.Drawing.Size(179, 114);
            // 
            // cmsCastle_MusterArmy
            // 
            this.cmsCastle_MusterArmy.Name = "cmsCastle_MusterArmy";
            this.cmsCastle_MusterArmy.Size = new System.Drawing.Size(178, 22);
            this.cmsCastle_MusterArmy.Text = "Muster Army";
            this.cmsCastle_MusterArmy.Click += new System.EventHandler(this.cmsCastle_MusterArmy_Click);
            // 
            // cmsCastle_UpgradeCastle
            // 
            this.cmsCastle_UpgradeCastle.Name = "cmsCastle_UpgradeCastle";
            this.cmsCastle_UpgradeCastle.Size = new System.Drawing.Size(178, 22);
            this.cmsCastle_UpgradeCastle.Text = "Upgrade Castle";
            this.cmsCastle_UpgradeCastle.Click += new System.EventHandler(this.cmsCastle_UpgradeCastle_Click);
            // 
            // cmsCastle_UpgradeWalls
            // 
            this.cmsCastle_UpgradeWalls.Name = "cmsCastle_UpgradeWalls";
            this.cmsCastle_UpgradeWalls.Size = new System.Drawing.Size(178, 22);
            this.cmsCastle_UpgradeWalls.Text = "Upgrade Walls";
            this.cmsCastle_UpgradeWalls.Click += new System.EventHandler(this.cmsCastle_UpgradeWalls_Click);
            // 
            // cmsCastle_UpgradeTowers
            // 
            this.cmsCastle_UpgradeTowers.Name = "cmsCastle_UpgradeTowers";
            this.cmsCastle_UpgradeTowers.Size = new System.Drawing.Size(178, 22);
            this.cmsCastle_UpgradeTowers.Text = "Upgrade Towers";
            this.cmsCastle_UpgradeTowers.Click += new System.EventHandler(this.cmsCastle_UpgradeTowers_Click);
            // 
            // cmsCastle_UpgradeGatehouse
            // 
            this.cmsCastle_UpgradeGatehouse.Name = "cmsCastle_UpgradeGatehouse";
            this.cmsCastle_UpgradeGatehouse.Size = new System.Drawing.Size(178, 22);
            this.cmsCastle_UpgradeGatehouse.Text = "Upgrade Gatehouse";
            this.cmsCastle_UpgradeGatehouse.Click += new System.EventHandler(this.cmsCastle_UpgradeGatehouse_Click);
            // 
            // pbMinimap
            // 
            this.pbMinimap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMinimap.BackColor = System.Drawing.Color.Transparent;
            this.pbMinimap.Location = new System.Drawing.Point(679, 324);
            this.pbMinimap.MinimapRect = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.pbMinimap.Name = "pbMinimap";
            this.pbMinimap.Size = new System.Drawing.Size(197, 149);
            this.pbMinimap.TabIndex = 10;
            this.pbMinimap.TabStop = false;
            this.pbMinimap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMinimap_MouseDown);
            this.pbMinimap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMinimap_MouseDown);
            // 
            // cmsArmyCastle
            // 
            this.cmsArmyCastle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsArmyCastle_DisbandArmy,
            this.cmsArmyCastle_BuildBuilding,
            this.cmsArmyCastle_BuildRoad,
            this.toolStripSeparator2,
            this.cmsArmyCastle_IncreaseArmy,
            this.cmsArmyCastle_UpgradeCastle,
            this.cmsArmyCastle_UpgradeWalls,
            this.cmsArmyCastle_UpgradeTowers,
            this.cmsArmyCastle_UpgradeGatehouse});
            this.cmsArmyCastle.Name = "cmsArmy";
            this.cmsArmyCastle.Size = new System.Drawing.Size(179, 186);
            // 
            // cmsArmyCastle_DisbandArmy
            // 
            this.cmsArmyCastle_DisbandArmy.Name = "cmsArmyCastle_DisbandArmy";
            this.cmsArmyCastle_DisbandArmy.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_DisbandArmy.Text = "Disband Army";
            this.cmsArmyCastle_DisbandArmy.Click += new System.EventHandler(this.cmsArmy_DisbandArmy_Click);
            // 
            // cmsArmyCastle_BuildBuilding
            // 
            this.cmsArmyCastle_BuildBuilding.Name = "cmsArmyCastle_BuildBuilding";
            this.cmsArmyCastle_BuildBuilding.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_BuildBuilding.Text = "Build Building";
            this.cmsArmyCastle_BuildBuilding.Click += new System.EventHandler(this.cmsArmy_BuildBuilding_Click);
            // 
            // cmsArmyCastle_BuildRoad
            // 
            this.cmsArmyCastle_BuildRoad.Name = "cmsArmyCastle_BuildRoad";
            this.cmsArmyCastle_BuildRoad.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_BuildRoad.Text = "Build Road";
            this.cmsArmyCastle_BuildRoad.Click += new System.EventHandler(this.cmsArmy_BuildRoad_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(175, 6);
            // 
            // cmsArmyCastle_IncreaseArmy
            // 
            this.cmsArmyCastle_IncreaseArmy.Name = "cmsArmyCastle_IncreaseArmy";
            this.cmsArmyCastle_IncreaseArmy.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_IncreaseArmy.Text = "Increase Army";
            this.cmsArmyCastle_IncreaseArmy.Click += new System.EventHandler(this.cmsArmyCastle_IncreaseArmy_Click);
            // 
            // cmsArmyCastle_UpgradeCastle
            // 
            this.cmsArmyCastle_UpgradeCastle.Name = "cmsArmyCastle_UpgradeCastle";
            this.cmsArmyCastle_UpgradeCastle.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_UpgradeCastle.Text = "Upgrade Castle";
            this.cmsArmyCastle_UpgradeCastle.Click += new System.EventHandler(this.cmsCastle_UpgradeCastle_Click);
            // 
            // cmsArmyCastle_UpgradeWalls
            // 
            this.cmsArmyCastle_UpgradeWalls.Name = "cmsArmyCastle_UpgradeWalls";
            this.cmsArmyCastle_UpgradeWalls.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_UpgradeWalls.Text = "Upgrade Walls";
            this.cmsArmyCastle_UpgradeWalls.Click += new System.EventHandler(this.cmsCastle_UpgradeWalls_Click);
            // 
            // cmsArmyCastle_UpgradeTowers
            // 
            this.cmsArmyCastle_UpgradeTowers.Name = "cmsArmyCastle_UpgradeTowers";
            this.cmsArmyCastle_UpgradeTowers.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_UpgradeTowers.Text = "Upgrade Towers";
            this.cmsArmyCastle_UpgradeTowers.Click += new System.EventHandler(this.cmsCastle_UpgradeTowers_Click);
            // 
            // cmsArmyCastle_UpgradeGatehouse
            // 
            this.cmsArmyCastle_UpgradeGatehouse.Name = "cmsArmyCastle_UpgradeGatehouse";
            this.cmsArmyCastle_UpgradeGatehouse.Size = new System.Drawing.Size(178, 22);
            this.cmsArmyCastle_UpgradeGatehouse.Text = "Upgrade Gatehouse";
            this.cmsArmyCastle_UpgradeGatehouse.Click += new System.EventHandler(this.cmsCastle_UpgradeGatehouse_Click);
            // 
            // cbPrevious
            // 
            this.cbPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPrevious.AutoSize = true;
            this.cbPrevious.Location = new System.Drawing.Point(680, 104);
            this.cbPrevious.Name = "cbPrevious";
            this.cbPrevious.Size = new System.Drawing.Size(117, 17);
            this.cbPrevious.TabIndex = 11;
            this.cbPrevious.Text = "Show previous turn";
            this.cbPrevious.UseVisualStyleBackColor = true;
            this.cbPrevious.CheckedChanged += new System.EventHandler(this.cbPrevious_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(898, 505);
            this.Controls.Add(this.cbPrevious);
            this.Controls.Add(this.pbMinimap);
            this.Controls.Add(this.btnResolve);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lblResources);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnEndTurn);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Warhammer Campaign";
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.hexgridPanel1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsArmy.ResumeLayout(false);
            this.cmsArmyTown.ResumeLayout(false);
            this.cmsTown.ResumeLayout(false);
            this.cmsCastle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinimap)).EndInit();
            this.cmsArmyCastle.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomHexgridPanel hexgridPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button btnEndTurn;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.ContextMenuStrip cmsArmy;
        private System.Windows.Forms.ToolStripMenuItem cmsArmy_DisbandArmy;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblResources;
        private System.Windows.Forms.ContextMenuStrip cmsArmyTown;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_DisbandArmy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_UpgradeTown;
        private System.Windows.Forms.ContextMenuStrip cmsTown;
        private System.Windows.Forms.ToolStripMenuItem cmsTown_UpgradeTown;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_BuildBuilding;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnResolve;
        private System.Windows.Forms.ToolStripMenuItem cmsArmy_BuildBuilding;
        private System.Windows.Forms.ToolStripMenuItem cmsTown_UpgradeObstacles;
        private System.Windows.Forms.ToolStripMenuItem cmsTown_UpgradeWalls;
        private System.Windows.Forms.ToolStripMenuItem cmsTown_UpgradeTowers;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_UpgradeObstacles;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_UpgradeWalls;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_UpgradeTowers;
        private System.Windows.Forms.ToolStripMenuItem cmsTown_MusterArmy;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_IncreaseArmy;
        private System.Windows.Forms.ContextMenuStrip cmsCastle;
        private System.Windows.Forms.ToolStripMenuItem cmsCastle_MusterArmy;
        private System.Windows.Forms.ToolStripMenuItem cmsCastle_UpgradeCastle;
        private System.Windows.Forms.ToolStripMenuItem cmsCastle_UpgradeWalls;
        private System.Windows.Forms.ToolStripMenuItem cmsCastle_UpgradeTowers;
        private System.Windows.Forms.ToolStripMenuItem cmsCastle_UpgradeGatehouse;
        private Minimap pbMinimap;
        private System.Windows.Forms.ContextMenuStrip cmsArmyCastle;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_DisbandArmy;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_BuildBuilding;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_IncreaseArmy;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_UpgradeCastle;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_UpgradeWalls;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_UpgradeTowers;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_UpgradeGatehouse;
        private System.Windows.Forms.ToolStripMenuItem cmsArmy_BuildRoad;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyTown_BuildRoad;
        private System.Windows.Forms.ToolStripMenuItem cmsArmyCastle_BuildRoad;
        private System.Windows.Forms.CheckBox cbPrevious;
    }
}

