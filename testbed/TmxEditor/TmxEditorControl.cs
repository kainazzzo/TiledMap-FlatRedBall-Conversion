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

namespace TmxEditor
{
    public partial class TmxEditorControl : UserControl
    {
        SystemManagers mManagers;


        public TmxEditorControl()
        {
            InitializeComponent();

            TilesetDisplayManager.Self.Initialize(this.XnaControl, ListBox, this.StatusLabel);
            XnaControl.XnaInitialize += new Action(HandleXnaInitialize);
            XnaControl.XnaUpdate += new Action(HandleXnaUpdate);
            XnaControl.XnaDraw += new Action(HandleXnaDraw);
        }


        public void LoadFile(string fileName)
        {

            if (!string.IsNullOrEmpty(fileName))
            {

                ProjectManager.Self.LoadTiledMapSave(fileName);
                ToolComponentManager.Self.ReactToLoadedFile(fileName);

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
    }
}
