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

namespace TmxEditor.GraphicalDisplay.Tilesets
{
    public class TilesetDisplayManager : ToolComponent
    {
        #region Fields

        static TilesetDisplayManager mSelf;
        ListBox mTilesetsListBox;
        List<TilePropertyHighlight> mHighlights = new List<TilePropertyHighlight>();

        Sprite mSprite;

        SystemManagers mManagers;

        CameraPanningLogic mCameraPanningLogic;

        InputLibrary.Cursor mCursor;
        InputLibrary.Keyboard mKeyboard;
        GraphicsDeviceControl mControl;
        PropertyGrid mPropertyGrid;

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

        public static TilesetDisplayManager Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new TilesetDisplayManager();
                }
                return mSelf;
            }
        }

        #endregion

        public void Initialize(GraphicsDeviceControl control, ListBox tilesetsListBox, Label infoLabel, PropertyGrid propertyGrid)
        {
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

        void HandleXnaUpdate()
        {

            mCursor.Activity(TimeManager.Self.CurrentTime);
            //mKeyboard.Activity();

            float x = mCursor.GetWorldX(mManagers);
            float y = mCursor.GetWorldY(mManagers);
            string whatToShow = null;

            foreach (var highlight in mHighlights)
            {
                if (x > highlight.X && x < highlight.X + highlight.Width &&
                    y > highlight.Y && y < highlight.Y + highlight.Height)
                {
                    mapTilesetTile tile = highlight.Tag as mapTilesetTile;

                    foreach (var kvp in tile.PropertyDictionary)
                    {
                        whatToShow += "(" + kvp.Key + ", " + kvp.Value + ") ";

                    }
                    break;
                }
            }
            mInfoLabel.Text = whatToShow;
        }


        void HandleWindowResize()
        {

            Camera.X = mManagers.Renderer.GraphicsDevice.Viewport.Width / 2.0f;
            Camera.Y = mManagers.Renderer.GraphicsDevice.Viewport.Height / 2.0f;
        }

        public void HandleXnaInitialize(SystemManagers managers)
        {
            mManagers = managers;

            mSprite = new Sprite(null);
            mSprite.Visible = false;
            mManagers.SpriteManager.Add(mSprite);
            HandleWindowResize();
            mCursor = new InputLibrary.Cursor();
            mCursor.Initialize(mControl);
            mCameraPanningLogic = new CameraPanningLogic(mControl, managers, mCursor, mKeyboard);
        }


        void HandleLoadedFile(string fileName)
        {
            FillListBox();

            ClearAllHighlights();

            mSprite.Visible = false;
        }

        void HandleLoadedAndMergedProperties(string fileName)
        {
            FillListBox();

            ClearAllHighlights();

            mSprite.Visible = false;


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

            var image = currentTileset.Image[0];

            string fileName = image.source;
            string absoluteFile = ProjectManager.Self.MakeAbsolute(fileName);
            mSprite.Visible = true;
            mSprite.Texture = LoaderManager.Self.Load(absoluteFile, mManagers);

            int numberTilesWide = mSprite.Texture.Width / currentTileset.Tilewidth;
            int numberTilesTall = mSprite.Texture.Height / currentTileset.Tileheight;

            foreach (var kvp in currentTileset.TileDictionary)
            {
                uint index = kvp.Key - currentTileset.Firstgid;
                int count = kvp.Value.properties.Count;

                if (count != 0)
                {
                    TilePropertyHighlight tph = new TilePropertyHighlight(mManagers);

                    long xIndex = index % numberTilesWide;
                    long yIndex = index / numberTilesWide;

                    tph.X = xIndex * currentTileset.Tilewidth;
                    tph.Y = yIndex * currentTileset.Tileheight;
                    
                    tph.Width = currentTileset.Tilewidth;
                    tph.Height = currentTileset.Tileheight;
                    tph.Count = count;
                    tph.AddToManagers();

                    tph.Tag = kvp.Value;

                    mHighlights.Add(tph);
                }
            }
        }

        private void ClearAllHighlights()
        {
            foreach (TilePropertyHighlight tph in mHighlights)
            {
                tph.RemoveFromManagers();
            }
            mHighlights.Clear();
        }


    }
}
