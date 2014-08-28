using FlatRedBall.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TmxEditor.Managers;
using TmxEditor.UI;
using TMXGlueLib;

namespace TmxEditor.Controllers
{
    public partial class TilesetController
    {
        #region Fields

        ListBox mTilesetsListBox;

        ToolStripMenuItem mSetSharedTileset;
        ToolStripMenuItem mCreateSharedTileset;
        ToolStripMenuItem mAddNewTilesetMenuItem;


        #endregion

        #region Properties

        public Tileset CurrentTileset
        {
            get
            {
                return mTilesetsListBox.SelectedItem as Tileset;
            }
        }

        #endregion

        #region Methods

        private void InitializeListBox(ListBox tilesetsListBox)
        {
            mTilesetsListBox = tilesetsListBox;
            mTilesetsListBox.SelectedIndexChanged += new EventHandler(HandleTilesetSelect);

            mTilesetsListBox.Click += HandleTilesetClick;

            mSetSharedTileset = new ToolStripMenuItem();
            mSetSharedTileset.Text = "Set Shared Tileset";
            mSetSharedTileset.Click += HandleSetSharedTilesetClick;

            mCreateSharedTileset = new ToolStripMenuItem();
            mCreateSharedTileset.Text = "Export Tileset to .tsx";
            mCreateSharedTileset.Click += HandleCreateSharedTilesetClick;

            mAddNewTilesetMenuItem = new ToolStripMenuItem();
            mAddNewTilesetMenuItem.Text = "Add new tileset";
            mAddNewTilesetMenuItem.Click += HandleAddNewTilesetClick;
        }

        private void HandleAddNewTilesetClick(object sender, EventArgs e)
        {
            string fileName;
            bool succeeded;
            GetTilesetFromFileOrOptions(out fileName, out succeeded);

            if (fileName != null)
            {
                Tileset newTileset = new Tileset();
                // We're going to add this to the end, so we need to see what the last tileset is, get its ID and count, and start this tileset at that number

                var lastTileset = AppState.Self.CurrentTiledMapSave.Tilesets.LastOrDefault();

                uint startingIndex = 1;

                if (lastTileset != null)
                {
                    startingIndex = lastTileset.Firstgid + (uint)lastTileset.GetTileCount();
                }

                newTileset.Firstgid = startingIndex;

                string oldRelative = FileManager.RelativeDirectory;
                FileManager.RelativeDirectory = AppState.Self.TmxFolder;
                newTileset.Source = FileManager.MakeRelative( fileName, AppState.Self.TmxFolder);
                FileManager.RelativeDirectory = oldRelative;

                AppState.Self.CurrentTiledMapSave.Tilesets.Add(newTileset);

                // refresh everything:
                RefreshListBox(newTileset);
                mTilesetsListBox.SelectedItem = newTileset;

                UpdateXnaDisplayToTileset();
                // and make sure the apps are notified about the change
                if (AnyTileMapChange != null)
                {
                    AnyTileMapChange(this, null);
                }
            }
        }

        void HandleSetSharedTilesetClick(object sender, EventArgs e)
        {
            string fileName;
            bool succeeded;
            GetTilesetFromFileOrOptions(out fileName, out succeeded);

            if (succeeded)
            {
                SetSharedTsx(fileName);
            }
        }

        private void HandleCreateSharedTilesetClick(object sender, EventArgs e)
        {

            string directory = FileManager.GetDirectory( AppState.Self.CurrentTiledMapSave.FileName );

            CreateSharedTileset(directory);

            if (AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }

            RefreshListBox(CurrentTileset);

        }

        private void CreateSharedTileset(string destinationDirectory)
        {
            SharedTilesetManager.ConvertToSharedTileset(CurrentTileset, AppState.Self.CurrentTiledMapSave, destinationDirectory);

            if (AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }
            RefreshListBox(CurrentTileset);
        }

        private void GetTilesetFromFileOrOptions(out string fileName, out bool succeeded)
        {
            bool doOptionsExist = AppState.Self.ProvidedContext.AvailableTsxFiles.Count != 0;


            succeeded = false;

            if (doOptionsExist)
            {
                succeeded = ShowTsxOptions(out fileName);
            }
            else
            {
                succeeded = TsxSelectionForm.TryGetTsxFileNameFromDisk(out fileName);
            }
        }

        private bool ShowTsxOptions(out string fileName)
        {
            bool succeeded = false;
            fileName = null;

            TsxSelectionForm form = new TsxSelectionForm();

            var dialogResult = form.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                fileName = form.FileName;
                succeeded = true;
            }



            return succeeded;
        }

        private void SetSharedTsx(string fullFileName)
        {
            if (CurrentTileset != null)
            {
                string tmxDirectory = AppState.Self.TmxFolder;
                string fileNameRelativeToTmx = ToolsUtilities.FileManager.MakeRelative(fullFileName, tmxDirectory);

                // If we set this file, the Tileset is going to try to load it.  We're going to set a relative directory,
                // but to load it it needs an absolute file name, so its going to use the FileManager's RelativeDirectory
                // property.  Therefore, we're going to set the RelativeDirecty to whatever the TMX directory is before
                // we set this property, then "pop" the value back to the original value.
                string originalRelativeDirectory = FileManager.RelativeDirectory;
                FileManager.RelativeDirectory = AppState.Self.TmxFolder;
                CurrentTileset.Source = fileNameRelativeToTmx;
                // Now that we've set the source and it loaded we can set the relative directory back:
                FileManager.RelativeDirectory = originalRelativeDirectory;


                // refresh the list box:
                RefreshListBox(CurrentTileset);
                UpdateXnaDisplayToTileset();
                // and make sure the apps are notified about the change
                if(AnyTileMapChange != null)
                {
                    AnyTileMapChange(this, null);
                }
            }
        }


        void HandleTilesetClick(object sender, EventArgs e)
        {
            PopulateRightClickMenu();
        }

        private void PopulateRightClickMenu()
        {
            bool isAnythingSelected = CurrentTileset != null;

            mTilesetsListBox.ContextMenuStrip.Items.Clear();

            if (isAnythingSelected)
            {
                bool isShared = !string.IsNullOrEmpty(CurrentTileset.Source);

                if(isShared)
                {
                    mTilesetsListBox.ContextMenuStrip.Items.Add(mSetSharedTileset);
                }
                else
                {
                    mTilesetsListBox.ContextMenuStrip.Items.Add(mCreateSharedTileset);
                }

            }

            mTilesetsListBox.ContextMenuStrip.Items.Add(mAddNewTilesetMenuItem);
        }

        void HandleTilesetSelect(object sender, EventArgs e)
        {
            UpdateXnaDisplayToTileset();
        }

        private void RefreshListBox(Tileset tileset)
        {
            // Does this tileset already exist?
            bool alreadyExists = mTilesetsListBox.Items.Contains(tileset);

            if (alreadyExists)
            {
                var index = mTilesetsListBox.Items.IndexOf(tileset);
                // This should refresh the text, but keep things selected
                mTilesetsListBox.Items[index] = tileset;
            }
            else
            {
                mTilesetsListBox.Items.Add(tileset);
            }

        }

        private void FillListBox()
        {
            mTilesetsListBox.Items.Clear();
            // this could be an empty .tmx.  Support that.
            if (ProjectManager.Self.TiledMapSave.Tilesets != null)
            {
                foreach (var tileset in ProjectManager.Self.TiledMapSave.Tilesets)
                {
                    mTilesetsListBox.Items.Add(tileset);
                }
            }


            PopulateRightClickMenu();
        }

        #endregion

    }
}
