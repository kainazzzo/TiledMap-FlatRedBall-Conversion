using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FlatRedBall.Glue.SaveClasses;
using TmxEditor.GraphicalDisplay.Tilesets;
using System.Reflection;
using RenderingLibrary.Content;
using RenderingLibrary;
using FlatRedBall.SpecializedXnaControls;
using TmxEditor.Controllers;
using TmxEditor.UI;
using ToolsUtilities;

namespace TmxEditor
{
    public partial class TmxEditorControl : UserControl
    {
        #region Fields

        SystemManagers mManagers;
        string mCurrentFileName;
        #endregion

        #region Events

        public event EventHandler AnyTileMapChange;
        public event EventHandler TilesetDisplayRightClick;

        #endregion

        public TmxEditorControl()
        {
            InitializeComponent();

            TilesetController.Self.Initialize(this.XnaControl, TilesetsListBox, this.StatusLabel, 
                this.TilesetTilePropertyGrid, this.HasCollisionsCheckBox, NameTextBox, EntitiesComboBox);
            TilesetController.Self.AnyTileMapChange += HandleChangeInternal;

            XnaControl.XnaInitialize += new Action(HandleXnaInitialize);
            XnaControl.XnaUpdate += new Action(HandleXnaUpdate);
            XnaControl.XnaDraw += new Action(HandleXnaDraw);


            LayersController.Self.Initialize(this.LayersListBox, LayerPropertyGrid);
            LayersController.Self.AnyTileMapChange += HandleChangeInternal;

        }

        private List<string> _entities;

        public List<string> Entities
        {
            get
            {
                return _entities;
            }
            set
            {
                _entities = value;
                EntitiesComboBox.Items.Clear();
                if (value != null)
                {
                    _entities.ForEach(e => EntitiesComboBox.Items.Add(e));
                }
            }
        }

        void HandleChangeInternal(object sender, EventArgs args)
        {
            if (AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }
        }



        public void LoadFile(string fileName)
        {

            if (!string.IsNullOrEmpty(fileName))
            {
                mCurrentFileName = fileName;

                ProjectManager.Self.LoadTiledMapSave(fileName);
                ToolComponentManager.Self.ReactToLoadedFile(fileName);
                LayersController.Self.TiledMapSave = ProjectManager.Self.TiledMapSave;
                this.LoadedTmxLabel.Text = fileName;
            }
        }

        public void SaveCurrentTileMap()
        {
            if (!string.IsNullOrEmpty(mCurrentFileName) && ProjectManager.Self.TiledMapSave != null)
            {

                const int maxTries = 5;
                int numberOfTries = 0;
                bool hasSaved = false;

                Exception lastException = null;

                while (!hasSaved && numberOfTries < maxTries)
                {
                    try
                    {
                        ProjectManager.Self.SaveTiledMapSave();
                        hasSaved = true;
                    }
                    catch(Exception exception)
                    {
                        exception = lastException;
                        numberOfTries++;
                    }
                }

                if (!hasSaved)
                {
                    throw new Exception("Error saving TMX file: " + lastException);
                }

            }
        }

        void HandleXnaInitialize()
        {
            try
            {
                CreateManagers();


                string targetFntFileName = CreateAndSaveFonts();

                LoaderManager.Self.Initialize(null, targetFntFileName, XnaControl.Services, mManagers);
                ToolComponentManager.Self.ReactToXnaInitialize(mManagers);
            }
            catch (Exception e)
            {
                throw new Exception("Error initializing XNA\n\n" + e.ToString());
            }
        }

        private void CreateManagers()
        {
            // For now we'll just use one SystemManagers but we may need to expand this if we have two windows
            mManagers = new SystemManagers();
            mManagers.Initialize(XnaControl.GraphicsDevice);
        }

        private static string CreateAndSaveFonts()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(XnaAndWinforms.GraphicsDeviceControl));

            string targetFntFileName = FileManager.UserApplicationDataForThisApplication + "Font18Arial.fnt";
            string targetPngFileName = FileManager.UserApplicationDataForThisApplication + "Font18Arial_0.png";
            FileManager.SaveEmbeddedResource(
                assembly,
                "XnaAndWinforms.Content.Font18Arial.fnt",
                targetFntFileName);

            FileManager.SaveEmbeddedResource(
                assembly,
                "XnaAndWinforms.Content.Font18Arial_0.png",
                targetPngFileName);
            return targetFntFileName;
        }


        void HandleXnaDraw()
        {
            mManagers.Renderer.Draw(mManagers);
        }


        void HandleXnaUpdate()
        {
            TimeManager.Self.Activity();
        }

        internal void LoadTilesetProperties(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string output;

                ProjectManager.Self.LoadTilesetFrom(fileName, out output);

                ToolComponentManager.Self.ReactToLoadedAndMergedProperties(fileName);
                if (!string.IsNullOrEmpty(output))
                {
                    MessageBox.Show(output);
                }

            }
        }

        private void AddLayerPropertyButton_Click(object sender, EventArgs e)
        {
            LayersController.Self.HandleAddPropertyClick();

        }

        private void RemovePropertyButton_Click(object sender, EventArgs e)
        {
            LayersController.Self.HandleRemovePropertyClick();
        }

        private void LayersListBox_MouseClick(object sender, MouseEventArgs e)
        {
            LayersController.Self.HandleListRightClick(e);
        }

        private void LayerPropertyGrid_Click(object sender, EventArgs e)
        {
        }

        private void LayerPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            LayersController.Self.UpdatePropertyGridContextMenu(e);

        }

        private void TilesetsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void AddTilesetPropertyButton_Click(object sender, EventArgs e)
        {
            TilesetController.Self.HandleAddPropertyClick();
        }

        private void RemoveTilesetPropertyButton_Click(object sender, EventArgs e)
        {
            TilesetController.Self.HandleRemovePropertyClick();
        }

        private void XnaControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && TilesetDisplayRightClick != null)
            {
                TilesetDisplayRightClick(this, e);
            }
        }

        public void UpdateTilesetDisplay()
        {

            TilesetController.Self.UpdateXnaDisplayToTileset();

            TilesetController.Self.Displayer.UpdateDisplayedProperties();
            TilesetController.Self.Displayer.PropertyGrid.Refresh();
        }

        private void HasCollisionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            TilesetController.Self.HasCollisionsCheckBoxChanged(HasCollisionsCheckBox.Checked);
        }

        private void EntitiesComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            TilesetController.Self.EntitiesComboBoxChanged(EntitiesComboBox.SelectedItem as string);
        }

    }
}
