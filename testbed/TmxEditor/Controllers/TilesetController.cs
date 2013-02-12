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

        public mapTileset CurrentTileset
        {
            get
            {
                return mTilesetsListBox.SelectedItem as mapTileset;
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
            ClearAllHighlights();


            var currentTileset = mTilesetsListBox.SelectedItem as mapTileset;

            SetTilesetSpriteTexture();

            //int numberTilesTall = mSprite.Texture.Height / currentTileset.Tileheight;

            foreach (var tile in currentTileset.Tiles)
            {

                int count = tile.properties.Count;

                if (count != 0)
                {
                    float left;
                    float top;
                    float width;
                    float height;
                    GetTopLeftWidthHeight(tile, out left, out top, out width, out height);
                    TilePropertyHighlight tph = new TilePropertyHighlight(mManagers);
                    tph.X = left;
                    tph.Y = top;
                    
                    tph.Width = width;
                    tph.Height = height;
                    tph.Count = count;
                    tph.AddToManagers();

                    tph.Tag = tile;

                    mTilesWithPropertiesMarkers.Add(tph);
                }
            }
        }

        private void GetTopLeftWidthHeight(mapTilesetTile tile, out float left, out float top, out float width, out float height)
        {
            var currentTileset = mTilesetsListBox.SelectedItem as mapTileset;

            int numberTilesWide = mSprite.Texture.Width / currentTileset.Tilewidth;

            int index = (int)(tile.id - currentTileset.Firstgid);


            long xIndex = index % numberTilesWide;
            long yIndex = index / numberTilesWide;

            left = xIndex * currentTileset.Tilewidth;
            top = yIndex * currentTileset.Tileheight;
            width = currentTileset.Tilewidth;
            height = currentTileset.Tileheight;
        }
    }
}
