using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TmxEditor.GraphicalDisplay.Tilesets;
using RenderingLibrary;
using RenderingLibrary.Content;
using System.Reflection;
using FlatRedBall.SpecializedXnaControls;

namespace TmxEditor
{
    public partial class Form1 : Form
    {
        SystemManagers mManagers;


        public Form1()
        {
            InitializeComponent();
            TilesetDisplayManager.Self.Initialize(this.XnaControl, ListBox, this.StatusLabel);
            XnaControl.XnaInitialize += new Action(HandleXnaInitialize);
            XnaControl.XnaUpdate += new Action(HandleXnaUpdate);
            XnaControl.XnaDraw += new Action(HandleXnaDraw);
        }

        void HandleXnaUpdate()
        {
            TimeManager.Self.Activity();
        }

        void HandleXnaDraw()
        {
            mManagers.Renderer.Draw(mManagers);
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

        private void loadTMXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = ShowLoadFile("tmx");

            if (!string.IsNullOrEmpty(fileName))
            {
                ProjectManager.Self.LoadTiledMapSave(fileName);
                ToolComponentManager.Self.ReactToLoadedFile(fileName);

                this.LoadedTmxLabel.Text = fileName;
            }
        }
        
        private void loadTilesetPropertiesFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = ShowLoadFile("tmx");

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

        private string ShowLoadFile(string extension)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*." + extension + ")|*." + extension + "";
            var result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private void saveTMXAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = SaveFile("tmx");
            if (!string.IsNullOrEmpty(fileName))
            {
                ProjectManager.Self.SaveTiledMapSave(fileName);
            }
        }

        private string SaveFile(string extension)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "(*." + extension + ")|*." + extension + "";
            var result = fileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return fileDialog.FileName;
            }
            else
            {
                return null;
            }
        }



        private void Form1_Resize(object sender, EventArgs e)
        {
            ToolComponentManager.Self.ReactToWindowResize();
        }
    }
}
