using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RenderingLibrary.Graphics;
using RenderingLibrary;
using RenderingLibrary.Content;
using System.Windows.Forms;
using FlatRedBall.IO;
using FlatRedBall.SpecializedXnaControls.Input;
using TMXGlueLib;
using XnaAndWinforms;
using FlatRedBall.SpecializedXnaControls;
using InputLibrary;
using TmxEditor.PropertyGridDisplayers;
using TmxEditor.GraphicalDisplay.Tilesets;
using TmxEditor.UI;

namespace TmxEditor.Controllers
{
    public partial class TilesetController : ToolComponent<TilesetController>
    {
        #region Fields

        ListBox mTilesetsListBox;
        mapTilesetTile mCurrentTilesetTile;

        SystemManagers mManagers;

        CameraPanningLogic mCameraPanningLogic;

        InputLibrary.Cursor mCursor;
        InputLibrary.Keyboard mKeyboard;
        GraphicsDeviceControl mControl;
        PropertyGrid mPropertyGrid;
        TilesetTileDisplayer mDisplayer;

        

        Label mInfoLabel;
        #endregion

        #region Properties

        Camera Camera
        {
            get
            {
                return mManagers.Renderer.Camera;
            }
        }

        public Tileset CurrentTileset
        {
            get
            {
                return mTilesetsListBox.SelectedItem as Tileset;
            }
        }

        public mapTilesetTile CurrentTilesetTile
        {
            get
            {
                return mCurrentTilesetTile;
            }
            set
            {
                mCurrentTilesetTile = value;


                mDisplayer.Instance = mCurrentTilesetTile;
                mDisplayer.PropertyGrid.Refresh();

                UpdateHighlightRectangle();
            }
        }

        #endregion

        #region Events

        public event EventHandler AnyTileMapChange;

        #endregion

        public void Initialize(GraphicsDeviceControl control, ListBox tilesetsListBox, Label infoLabel, PropertyGrid propertyGrid)
        {
            mDisplayer = new TilesetTileDisplayer();
            mDisplayer.PropertyGrid = propertyGrid;
            mDisplayer.RefreshOnTimer = false;
            mDisplayer.PropertyGrid.PropertyValueChanged += HandlePropertyValueChangeInternal;


            mPropertyGrid = propertyGrid;
            mControl = control;
            ToolComponentManager.Self.Register(this);

            mTilesetsListBox = tilesetsListBox;
            mTilesetsListBox.SelectedIndexChanged += new EventHandler(HandleTilesetSelect);

            ReactToLoadedFile = HandleLoadedFile;
            ReactToXnaInitialize = HandleXnaInitialize;
            
            ReactToWindowResize = HandleWindowResize;

            ReactToLoadedAndMergedProperties = HandleLoadedAndMergedProperties;

            control.XnaUpdate += new Action(HandleXnaUpdate);
            
            mInfoLabel = infoLabel;
        }

        void HandlePropertyValueChangeInternal(object s, PropertyValueChangedEventArgs e)
        {
            if (AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }

        }

        void HandleLoadedFile(string fileName)
        {
            FillListBox();

            ClearAllHighlights();

            mSprite.Visible = false;
            mOutlineRectangle.Visible = false;
        }

        void HandleLoadedAndMergedProperties(string fileName)
        {
            FillListBox();

            ClearAllHighlights();

            mSprite.Visible = false;
            mOutlineRectangle.Visible = false;


        }

        private void FillListBox()
        {
            mTilesetsListBox.Items.Clear();
            // this could be an empty .tmx.  Support that.
            if (ProjectManager.Self.TiledMapSave.tileset != null)
            {
                foreach (var tileset in ProjectManager.Self.TiledMapSave.tileset)
                {
                    mTilesetsListBox.Items.Add(tileset);
                }
            }
        }

        void HandleTilesetSelect(object sender, EventArgs e)
        {
            UpdateXnaDisplayToTileset();
        }


        private void GetTopLeftWidthHeight(mapTilesetTile tile, out float left, out float top, out float width, out float height)
        {
            var currentTileset = mTilesetsListBox.SelectedItem as Tileset;

            int numberTilesWide = 0;
            if (currentTileset.Spacing != 0)
            {
                numberTilesWide = 1;
                int widthToUse = mSprite.Texture.Width - currentTileset.Tilewidth;

                numberTilesWide += widthToUse / (currentTileset.Tilewidth + currentTileset.Spacing);
            }
            else
            {
                numberTilesWide = mSprite.Texture.Width / (currentTileset.Tilewidth);
            }


            // I think GID are global IDs for tiles in the map
            // but within a tile set the IDs start at 0
            //int index = (int)(tile.id - currentTileset.Firstgid);
            int index = tile.id;

            long xIndex = index % numberTilesWide;
            long yIndex = index / numberTilesWide;

            left = xIndex * currentTileset.Tilewidth;
            top = yIndex * currentTileset.Tileheight;
            width = currentTileset.Tilewidth;
            height = currentTileset.Tileheight;
        }

        internal void HandleAddPropertyClick()
        {
            var tile = AppState.Self.CurrentMapTilesetTile;
            if (tile == null)
            {
                MessageBox.Show("You must first select a Layer");
            }
            else
            {
                NewPropertyWindow window = new NewPropertyWindow();
                DialogResult result = window.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string name = window.ResultName;
                    string type = window.ResultType;
                    var newProperty = new TMXGlueLib.property();
                    LayersController.SetPropertyNameFromNameAndType(name, type, newProperty);

                    

                    tile.properties.Add(newProperty);


                    mDisplayer.UpdateDisplayedProperties();
                    mDisplayer.PropertyGrid.Refresh();

                    if (AnyTileMapChange != null)
                    {
                        AnyTileMapChange(this, null);
                    }
                }

            }

        }

        internal void HandleRemovePropertyClick()
        {
            property property = AppState.Self.CurrentTilesetTileProperty;

            if (property != null)
            {
                var result =
                    MessageBox.Show("Are you sure you'd like to remove the property " + property.name + "?", "Remove property?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    AppState.Self.CurrentMapLayer.properties.Remove(property);
                    mDisplayer.UpdateDisplayedProperties();
                    mDisplayer.PropertyGrid.Refresh();
                    if (AnyTileMapChange != null)
                    {
                        AnyTileMapChange(this, null);
                    }
                }
            }

        }

        public property CurrentTilesetTileProperty 
        { 
            get
            {
                return mDisplayer.CurrentTilesetTileProperty;
            }
        }
    }
}
