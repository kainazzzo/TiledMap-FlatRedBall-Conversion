namespace TmxEditor
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
            this.LoadedTmxLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TilesetsTab = new System.Windows.Forms.TabPage();
            this.LayersTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.LayersListBox = new System.Windows.Forms.TreeView();
            this.RemovePropertyButton = new System.Windows.Forms.Button();
            this.AddLayerPropertyButton = new System.Windows.Forms.Button();
            this.LayerPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.LayerListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.LayerPropertyGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tabControl1.SuspendLayout();
            this.TilesetsTab.SuspendLayout();
            this.LayersTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TilesetsListBox
            // 
            this.TilesetsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TilesetsListBox.FormattingEnabled = true;
            this.TilesetsListBox.Location = new System.Drawing.Point(0, 0);
            this.TilesetsListBox.Name = "TilesetsListBox";
            this.TilesetsListBox.Size = new System.Drawing.Size(120, 472);
            this.TilesetsListBox.TabIndex = 6;
            // 
            // XnaControl
            // 
            this.XnaControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.XnaControl.DesiredFramesPerSecond = 30F;
            this.XnaControl.Location = new System.Drawing.Point(126, 0);
            this.XnaControl.Name = "XnaControl";
            this.XnaControl.Size = new System.Drawing.Size(693, 483);
            this.XnaControl.TabIndex = 5;
            this.XnaControl.Text = "graphicsDeviceControl1";
            // 
            // LoadedTmxLabel
            // 
            this.LoadedTmxLabel.AutoSize = true;
            this.LoadedTmxLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.LoadedTmxLabel.Location = new System.Drawing.Point(0, 0);
            this.LoadedTmxLabel.Name = "LoadedTmxLabel";
            this.LoadedTmxLabel.Size = new System.Drawing.Size(86, 13);
            this.LoadedTmxLabel.TabIndex = 4;
            this.LoadedTmxLabel.Text = "No TMX Loaded";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusLabel.Location = new System.Drawing.Point(0, 522);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(66, 13);
            this.StatusLabel.TabIndex = 7;
            this.StatusLabel.Text = "Status Label";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TilesetsTab);
            this.tabControl1.Controls.Add(this.LayersTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(827, 509);
            this.tabControl1.TabIndex = 8;
            // 
            // TilesetsTab
            // 
            this.TilesetsTab.Controls.Add(this.TilesetsListBox);
            this.TilesetsTab.Controls.Add(this.XnaControl);
            this.TilesetsTab.Location = new System.Drawing.Point(4, 22);
            this.TilesetsTab.Name = "TilesetsTab";
            this.TilesetsTab.Padding = new System.Windows.Forms.Padding(3);
            this.TilesetsTab.Size = new System.Drawing.Size(819, 483);
            this.TilesetsTab.TabIndex = 0;
            this.TilesetsTab.Text = "Tilesets";
            this.TilesetsTab.UseVisualStyleBackColor = true;
            // 
            // LayersTab
            // 
            this.LayersTab.Controls.Add(this.splitContainer1);
            this.LayersTab.Location = new System.Drawing.Point(4, 22);
            this.LayersTab.Name = "LayersTab";
            this.LayersTab.Padding = new System.Windows.Forms.Padding(3);
            this.LayersTab.Size = new System.Drawing.Size(819, 483);
            this.LayersTab.TabIndex = 1;
            this.LayersTab.Text = "Layers";
            this.LayersTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.LayersListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RemovePropertyButton);
            this.splitContainer1.Panel2.Controls.Add(this.AddLayerPropertyButton);
            this.splitContainer1.Panel2.Controls.Add(this.LayerPropertyGrid);
            this.splitContainer1.Size = new System.Drawing.Size(813, 477);
            this.splitContainer1.SplitterDistance = 271;
            this.splitContainer1.TabIndex = 8;
            // 
            // LayersListBox
            // 
            this.LayersListBox.ContextMenuStrip = this.LayerListContextMenu;
            this.LayersListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayersListBox.HideSelection = false;
            this.LayersListBox.Location = new System.Drawing.Point(0, 0);
            this.LayersListBox.Name = "LayersListBox";
            this.LayersListBox.ShowRootLines = false;
            this.LayersListBox.Size = new System.Drawing.Size(271, 477);
            this.LayersListBox.TabIndex = 7;
            this.LayersListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LayersListBox_MouseClick);
            // 
            // RemovePropertyButton
            // 
            this.RemovePropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RemovePropertyButton.Location = new System.Drawing.Point(114, 451);
            this.RemovePropertyButton.Name = "RemovePropertyButton";
            this.RemovePropertyButton.Size = new System.Drawing.Size(105, 23);
            this.RemovePropertyButton.TabIndex = 2;
            this.RemovePropertyButton.Text = "Remove Property";
            this.RemovePropertyButton.UseVisualStyleBackColor = true;
            this.RemovePropertyButton.Click += new System.EventHandler(this.RemovePropertyButton_Click);
            // 
            // AddLayerPropertyButton
            // 
            this.AddLayerPropertyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AddLayerPropertyButton.Location = new System.Drawing.Point(3, 451);
            this.AddLayerPropertyButton.Name = "AddLayerPropertyButton";
            this.AddLayerPropertyButton.Size = new System.Drawing.Size(105, 23);
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
            this.LayerPropertyGrid.Name = "LayerPropertyGrid";
            this.LayerPropertyGrid.Size = new System.Drawing.Size(535, 449);
            this.LayerPropertyGrid.TabIndex = 0;
            this.LayerPropertyGrid.ToolbarVisible = false;
            this.LayerPropertyGrid.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.LayerPropertyGrid_SelectedGridItemChanged);
            this.LayerPropertyGrid.Click += new System.EventHandler(this.LayerPropertyGrid_Click);
            // 
            // LayerListContextMenu
            // 
            this.LayerListContextMenu.Name = "LayerListContextMenu";
            this.LayerListContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // LayerPropertyGridContextMenu
            // 
            this.LayerPropertyGridContextMenu.Name = "LayerListContextMenu";
            this.LayerPropertyGridContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // TmxEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.LoadedTmxLabel);
            this.Name = "TmxEditorControl";
            this.Size = new System.Drawing.Size(827, 535);
            this.tabControl1.ResumeLayout(false);
            this.TilesetsTab.ResumeLayout(false);
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
        private System.Windows.Forms.Button RemovePropertyButton;
        private System.Windows.Forms.Button AddLayerPropertyButton;
        private System.Windows.Forms.ContextMenuStrip LayerListContextMenu;
        private System.Windows.Forms.ContextMenuStrip LayerPropertyGridContextMenu;
    }
}
