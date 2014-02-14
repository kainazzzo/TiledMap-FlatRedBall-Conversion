﻿namespace TmxEditor
{
    partial class TmxEditorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TilesetsListBox = new System.Windows.Forms.ListBox();
            this.XnaControl = new XnaAndWinforms.GraphicsDeviceControl();
            this.TilesetXnaContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LoadedTmxLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TilesetsTab = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.HasCollisionsCheckBox = new System.Windows.Forms.CheckBox();
            this.RemoveTilesetPropertyButton = new System.Windows.Forms.Button();
            this.AddTilesetPropertyButton = new System.Windows.Forms.Button();
            this.TilesetTilePropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.LayersTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LayersListBox = new System.Windows.Forms.TreeView();
            this.LayerListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RemoveLayerPropertyButton = new System.Windows.Forms.Button();
            this.AddLayerPropertyButton = new System.Windows.Forms.Button();
            this.LayerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.LayerPropertyGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.EntitiesComboBox = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.TilesetsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.LayersTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TilesetsListBox
            // 
            this.TilesetsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TilesetsListBox.FormattingEnabled = true;
            this.TilesetsListBox.ItemHeight = 20;
            this.TilesetsListBox.Location = new System.Drawing.Point(0, 0);
            this.TilesetsListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TilesetsListBox.Name = "TilesetsListBox";
            this.TilesetsListBox.Size = new System.Drawing.Size(276, 736);
            this.TilesetsListBox.TabIndex = 6;
            this.TilesetsListBox.SelectedIndexChanged += new System.EventHandler(this.TilesetsListBox_SelectedIndexChanged);
            // 
            // XnaControl
            // 
            this.XnaControl.ContextMenuStrip = this.TilesetXnaContextMenu;
            this.XnaControl.DesiredFramesPerSecond = 30F;
            this.XnaControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XnaControl.Location = new System.Drawing.Point(0, 0);
            this.XnaControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.XnaControl.Name = "XnaControl";
            this.XnaControl.Size = new System.Drawing.Size(934, 464);
            this.XnaControl.TabIndex = 5;
            this.XnaControl.Text = "graphicsDeviceControl1";
            this.XnaControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.XnaControl_MouseClick);
            // 
            // TilesetXnaContextMenu
            // 
            this.TilesetXnaContextMenu.Name = "TilesetXnaContextMenu";
            this.TilesetXnaContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // LoadedTmxLabel
            // 
            this.LoadedTmxLabel.AutoSize = true;
            this.LoadedTmxLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.LoadedTmxLabel.Location = new System.Drawing.Point(0, 0);
            this.LoadedTmxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LoadedTmxLabel.Name = "LoadedTmxLabel";
            this.LoadedTmxLabel.Size = new System.Drawing.Size(124, 20);
            this.LoadedTmxLabel.TabIndex = 4;
            this.LoadedTmxLabel.Text = "No TMX Loaded";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusLabel.Location = new System.Drawing.Point(0, 803);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(99, 20);
            this.StatusLabel.TabIndex = 7;
            this.StatusLabel.Text = "Status Label";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TilesetsTab);
            this.tabControl1.Controls.Add(this.LayersTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 20);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1240, 783);
            this.tabControl1.TabIndex = 8;
            // 
            // TilesetsTab
            // 
            this.TilesetsTab.Controls.Add(this.splitContainer2);
            this.TilesetsTab.Location = new System.Drawing.Point(4, 29);
            this.TilesetsTab.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TilesetsTab.Name = "TilesetsTab";
            this.TilesetsTab.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TilesetsTab.Size = new System.Drawing.Size(1232, 750);
            this.TilesetsTab.TabIndex = 0;
            this.TilesetsTab.Text = "Tilesets";
            this.TilesetsTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(4, 5);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.TilesetsListBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1224, 740);
            this.splitContainer2.SplitterDistance = 280;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 7;
            this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
            // 
            // splitContainer3
            // 
            this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.XnaControl);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.EntitiesComboBox);
            this.splitContainer3.Panel2.Controls.Add(this.label1);
            this.splitContainer3.Panel2.Controls.Add(this.NameTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.NameLabel);
            this.splitContainer3.Panel2.Controls.Add(this.HasCollisionsCheckBox);
            this.splitContainer3.Panel2.Controls.Add(this.RemoveTilesetPropertyButton);
            this.splitContainer3.Panel2.Controls.Add(this.AddTilesetPropertyButton);
            this.splitContainer3.Panel2.Controls.Add(this.TilesetTilePropertyGrid);
            this.splitContainer3.Size = new System.Drawing.Size(938, 740);
            this.splitContainer3.SplitterDistance = 468;
            this.splitContainer3.SplitterWidth = 6;
            this.splitContainer3.TabIndex = 6;
            // 
            // NameTextBox
            // 
            this.NameTextBox.AcceptsReturn = true;
            this.NameTextBox.Location = new System.Drawing.Point(70, 6);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(256, 26);
            this.NameTextBox.TabIndex = 7;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(4, 11);
            this.NameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(55, 20);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name:";
            // 
            // HasCollisionsCheckBox
            // 
            this.HasCollisionsCheckBox.AutoSize = true;
            this.HasCollisionsCheckBox.Location = new System.Drawing.Point(4, 42);
            this.HasCollisionsCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.HasCollisionsCheckBox.Name = "HasCollisionsCheckBox";
            this.HasCollisionsCheckBox.Size = new System.Drawing.Size(134, 24);
            this.HasCollisionsCheckBox.TabIndex = 5;
            this.HasCollisionsCheckBox.Text = "Has Collisions";
            this.HasCollisionsCheckBox.UseVisualStyleBackColor = true;
            this.HasCollisionsCheckBox.CheckedChanged += new System.EventHandler(this.HasCollisionsCheckBox_CheckedChanged);
            // 
            // RemoveTilesetPropertyButton
            // 
            this.RemoveTilesetPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveTilesetPropertyButton.Location = new System.Drawing.Point(171, 222);
            this.RemoveTilesetPropertyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RemoveTilesetPropertyButton.Name = "RemoveTilesetPropertyButton";
            this.RemoveTilesetPropertyButton.Size = new System.Drawing.Size(158, 35);
            this.RemoveTilesetPropertyButton.TabIndex = 4;
            this.RemoveTilesetPropertyButton.Text = "Remove Property";
            this.RemoveTilesetPropertyButton.UseVisualStyleBackColor = true;
            this.RemoveTilesetPropertyButton.Click += new System.EventHandler(this.RemoveTilesetPropertyButton_Click);
            // 
            // AddTilesetPropertyButton
            // 
            this.AddTilesetPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddTilesetPropertyButton.Location = new System.Drawing.Point(4, 222);
            this.AddTilesetPropertyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AddTilesetPropertyButton.Name = "AddTilesetPropertyButton";
            this.AddTilesetPropertyButton.Size = new System.Drawing.Size(158, 35);
            this.AddTilesetPropertyButton.TabIndex = 3;
            this.AddTilesetPropertyButton.Text = "Add Property";
            this.AddTilesetPropertyButton.UseVisualStyleBackColor = true;
            this.AddTilesetPropertyButton.Click += new System.EventHandler(this.AddTilesetPropertyButton_Click);
            // 
            // TilesetTilePropertyGrid
            // 
            this.TilesetTilePropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TilesetTilePropertyGrid.HelpVisible = false;
            this.TilesetTilePropertyGrid.Location = new System.Drawing.Point(0, 77);
            this.TilesetTilePropertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TilesetTilePropertyGrid.Name = "TilesetTilePropertyGrid";
            this.TilesetTilePropertyGrid.Size = new System.Drawing.Size(934, 136);
            this.TilesetTilePropertyGrid.TabIndex = 0;
            this.TilesetTilePropertyGrid.ToolbarVisible = false;
            // 
            // LayersTab
            // 
            this.LayersTab.Controls.Add(this.splitContainer1);
            this.LayersTab.Location = new System.Drawing.Point(4, 29);
            this.LayersTab.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LayersTab.Name = "LayersTab";
            this.LayersTab.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LayersTab.Size = new System.Drawing.Size(1232, 750);
            this.LayersTab.TabIndex = 1;
            this.LayersTab.Text = "Layers";
            this.LayersTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(4, 5);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LayersListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RemoveLayerPropertyButton);
            this.splitContainer1.Panel2.Controls.Add(this.AddLayerPropertyButton);
            this.splitContainer1.Panel2.Controls.Add(this.LayerPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(1224, 740);
            this.splitContainer1.SplitterDistance = 317;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 8;
            // 
            // LayersListBox
            // 
            this.LayersListBox.ContextMenuStrip = this.LayerListContextMenu;
            this.LayersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayersListBox.HideSelection = false;
            this.LayersListBox.Location = new System.Drawing.Point(0, 0);
            this.LayersListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LayersListBox.Name = "LayersListBox";
            this.LayersListBox.ShowRootLines = false;
            this.LayersListBox.Size = new System.Drawing.Size(313, 736);
            this.LayersListBox.TabIndex = 7;
            this.LayersListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LayersListBox_MouseClick);
            // 
            // LayerListContextMenu
            // 
            this.LayerListContextMenu.Name = "LayerListContextMenu";
            this.LayerListContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // RemoveLayerPropertyButton
            // 
            this.RemoveLayerPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemoveLayerPropertyButton.Location = new System.Drawing.Point(171, 696);
            this.RemoveLayerPropertyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RemoveLayerPropertyButton.Name = "RemoveLayerPropertyButton";
            this.RemoveLayerPropertyButton.Size = new System.Drawing.Size(158, 35);
            this.RemoveLayerPropertyButton.TabIndex = 2;
            this.RemoveLayerPropertyButton.Text = "Remove Property";
            this.RemoveLayerPropertyButton.UseVisualStyleBackColor = true;
            this.RemoveLayerPropertyButton.Click += new System.EventHandler(this.RemovePropertyButton_Click);
            // 
            // AddLayerPropertyButton
            // 
            this.AddLayerPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddLayerPropertyButton.Location = new System.Drawing.Point(4, 696);
            this.AddLayerPropertyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AddLayerPropertyButton.Name = "AddLayerPropertyButton";
            this.AddLayerPropertyButton.Size = new System.Drawing.Size(158, 35);
            this.AddLayerPropertyButton.TabIndex = 1;
            this.AddLayerPropertyButton.Text = "Add Property";
            this.AddLayerPropertyButton.UseVisualStyleBackColor = true;
            this.AddLayerPropertyButton.Click += new System.EventHandler(this.AddLayerPropertyButton_Click);
            // 
            // LayerPropertyGrid
            // 
            this.LayerPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LayerPropertyGrid.ContextMenuStrip = this.LayerPropertyGridContextMenu;
            this.LayerPropertyGrid.HelpVisible = false;
            this.LayerPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.LayerPropertyGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LayerPropertyGrid.Name = "LayerPropertyGrid";
            this.LayerPropertyGrid.Size = new System.Drawing.Size(892, 693);
            this.LayerPropertyGrid.TabIndex = 0;
            this.LayerPropertyGrid.ToolbarVisible = false;
            this.LayerPropertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.LayerPropertyGrid_SelectedGridItemChanged);
            this.LayerPropertyGrid.Click += new System.EventHandler(this.LayerPropertyGrid_Click);
            // 
            // LayerPropertyGridContextMenu
            // 
            this.LayerPropertyGridContextMenu.Name = "LayerListContextMenu";
            this.LayerPropertyGridContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Entity Type:";
            // 
            // EntitiesComboBox
            // 
            this.EntitiesComboBox.FormattingEnabled = true;
            this.EntitiesComboBox.Location = new System.Drawing.Point(442, 8);
            this.EntitiesComboBox.Name = "EntitiesComboBox";
            this.EntitiesComboBox.Size = new System.Drawing.Size(240, 28);
            this.EntitiesComboBox.TabIndex = 9;
            // 
            // TmxEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.LoadedTmxLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TmxEditorControl";
            this.Size = new System.Drawing.Size(1240, 823);
            this.tabControl1.ResumeLayout(false);
            this.TilesetsTab.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.LayersTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox TilesetsListBox;
        private XnaAndWinforms.GraphicsDeviceControl XnaControl;
        private System.Windows.Forms.Label LoadedTmxLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TilesetsTab;
        private System.Windows.Forms.TabPage LayersTab;
        private System.Windows.Forms.TreeView LayersListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid LayerPropertyGrid;
        private System.Windows.Forms.Button RemoveLayerPropertyButton;
        private System.Windows.Forms.Button AddLayerPropertyButton;
        private System.Windows.Forms.ContextMenuStrip LayerListContextMenu;
        private System.Windows.Forms.ContextMenuStrip LayerPropertyGridContextMenu;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.PropertyGrid TilesetTilePropertyGrid;
        private System.Windows.Forms.Button RemoveTilesetPropertyButton;
        private System.Windows.Forms.Button AddTilesetPropertyButton;
        public System.Windows.Forms.ContextMenuStrip TilesetXnaContextMenu;
        private System.Windows.Forms.CheckBox HasCollisionsCheckBox;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.ComboBox EntitiesComboBox;
        private System.Windows.Forms.Label label1;
    }
}
