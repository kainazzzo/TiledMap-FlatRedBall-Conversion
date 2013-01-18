using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TmxEditor.GraphicalDisplay.Tilesets;
using System.Reflection;
using RenderingLibrary.Content;
using RenderingLibrary;
using FlatRedBall.SpecializedXnaControls;
using TmxEditor.Controllers;
using TmxEditor.UI;

namespace TmxEditor
{
    public partial class TmxEditorControl : UserControl
    {
        #region Fields

        SystemManagers mManagers;

        #endregion

        #region Events

        public event EventHandler AnyTileMapChange;

        #endregion

        public TmxEditorControl()
        {
            InitializeComponent();

            TilesetDisplayManager.Self.Initialize(this.XnaControl, TilesetsListBox, this.StatusLabel);
            XnaControl.XnaInitialize += new Action(HandleXnaInitialize);
            XnaControl.XnaUpdate += new Action(HandleXnaUpdate);
            XnaControl.XnaDraw += new Action(HandleXnaDraw);

            LayersController.Self.Initialize(this.LayersListBox, LayerPropertyGrid);
            LayersController.Self.AnyTileMapChange += HandleChangeInternal;

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

                ProjectManager.Self.LoadTiledMapSave(fileName);
                ToolComponentManager.Self.ReactToLoadedFile(fileName);
                LayersController.Self.TiledMapSave = ProjectManager.Self.TiledMapSave;
                this.LoadedTmxLabel.Text = fileName;
            }


        }


        void HandleXnaInitialize()
        {
            try
            {
                // For now we'll just use one SystemManagers but we may need to expand this if we have two windows
                mManagers = new SystemManagers();
                mManagers.Initialize(XnaControl.GraphicsDevice);



                Assembly assembly = Assembly.GetAssembly(typeof(XnaAndWinforms.GraphicsDeviceControl));

                string targetFntFileName = WahooToolsUtilities.FileManager.UserApplicationDataForThisApplication + "Font18Arial.fnt";
                string targetPngFileName = WahooToolsUtilities.FileManager.UserApplicationDataForThisApplication + "Font18Arial_0.png";
                WahooToolsUtilities.FileManager.SaveEmbeddedResource(
                    assembly,
                    "XnaAndWinforms.Content.Font18Arial.fnt",
                    targetFntFileName);

                WahooToolsUtilities.FileManager.SaveEmbeddedResource(
                    assembly,
                    "XnaAndWinforms.Content.Font18Arial_0.png",
                    targetPngFileName);

                LoaderManager.Self.Initialize(null, targetFntFileName, XnaControl.Services, mManagers);
                ToolComponentManager.Self.ReactToXnaInitialize(mManagers);
            }
            catch (Exception e)
            {
                throw new Exception("Error initializing XNA");
            }
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
    }
}
