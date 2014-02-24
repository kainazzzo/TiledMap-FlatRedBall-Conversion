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
using FlatRedBall.Utilities;

namespace TmxEditor.Controllers
{
    public partial class TilesetController : ToolComponent<TilesetController>
    {

        public const string HasCollisionVariableName = "HasCollision";
        public const string EntityToCreatePropertyName = "EntityToCreate";



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
        CheckBox mHasCollisionsCheckBox;
        ComboBox mEntitiesComboBox;
        TextBox mNameTextBox;

        Tileset mTempTileset;

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


                RefreshUiToSelectedTile();
            }
        }

        public TilesetTileDisplayer Displayer
        {
            get
            {
                return mDisplayer;
            }
        }


        #endregion

        #region Events

        public event EventHandler AnyTileMapChange;

        #endregion

        public void Initialize(GraphicsDeviceControl control, ListBox tilesetsListBox, 
            Label infoLabel, PropertyGrid propertyGrid, CheckBox hasCollisionsCheckBox, TextBox nameTextBox, ComboBox entitiesComboBox)
        {
            mDisplayer = new TilesetTileDisplayer();
            mDisplayer.PropertyGrid = propertyGrid;
            mDisplayer.RefreshOnTimer = false;
            mDisplayer.PropertyGrid.PropertyValueChanged += HandlePropertyValueChangeInternal;

            mHasCollisionsCheckBox = hasCollisionsCheckBox;
            mEntitiesComboBox = entitiesComboBox;
            

            mNameTextBox = nameTextBox;
            mNameTextBox.KeyDown += HandleNameTextBoxKeyDown;

            mPropertyGrid = propertyGrid;
            mControl = control;
            ToolComponentManager.Self.Register(this);

            mTilesetsListBox = tilesetsListBox;
            mTilesetsListBox.SelectedIndexChanged += new EventHandler(HandleTilesetSelect);
            mTilesetsListBox.LostFocus += HandleLostFocus;

            ReactToLoadedFile = HandleLoadedFile;
            ReactToXnaInitialize = HandleXnaInitialize;
            
            ReactToWindowResize = HandleWindowResize;

            ReactToLoadedAndMergedProperties = HandleLoadedAndMergedProperties;

            control.XnaUpdate += new Action(HandleXnaUpdate);
            
            mInfoLabel = infoLabel;

            CurrentTilesetTile = null;
        }

        void HandleLostFocus(object sender, EventArgs e)
        {



            HandleNameChange();
        }

        void HandleNameTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                HandleNameChange();
                e.Handled = true;
            }
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
            if (ProjectManager.Self.TiledMapSave.Tilesets != null)
            {
                foreach (var tileset in ProjectManager.Self.TiledMapSave.Tilesets)
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

            int numberTilesWide = currentTileset.GetNumberOfTilesWide();


            // I think GID are global IDs for tiles in the map
            // but within a tile set the IDs start at 0
            //int index = (int)(tile.id - currentTileset.Firstgid);
            int index = tile.id;

            long xIndex = index % numberTilesWide;
            long yIndex = index / numberTilesWide;

            int leftAsInt;
            int topAsInt;
            currentTileset.IndexToCoordinate(xIndex, yIndex, out leftAsInt, out topAsInt);

            left = leftAsInt;
            top = topAsInt;
            width = currentTileset.Tilewidth;
            height = currentTileset.Tileheight;
        }

        internal void HandleAddPropertyClick()
        {
            var tile = AppState.Self.CurrentMapTilesetTile;
            if (tile == null)
            {
                MessageBox.Show("You must first select a Tile");
            }
            else
            {
                NewPropertyWindow window = new NewPropertyWindow();
                DialogResult result = window.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string name = window.ResultName;
                    string type = window.ResultType;
                    AddProperty(tile, name, type);
                }

            }

        }

        public property GetExistingProperty(string propertyName, mapTilesetTile tile)
        {
            Func<property, bool> predicate = item =>
                {
                    return item.name == propertyName ||
                        item.name.StartsWith(propertyName + " ") ||
                        item.name.StartsWith(propertyName + "(");

                };

            return tile.properties.FirstOrDefault(predicate);
        }

        public TMXGlueLib.property AddProperty(mapTilesetTile tile, string name, string type,
            bool raiseChangedEvent = true)
        {
            var newProperty = new TMXGlueLib.property();
            LayersController.SetPropertyNameFromNameAndType(name, type, newProperty);



            tile.properties.Add(newProperty);

            bool newTileAdded = false;
            if (AppState.Self.CurrentTileset.Tiles.Contains(tile) == false)
            {
                AppState.Self.CurrentTileset.Tiles.Add(tile);
                newTileAdded = true;
            }
            UpdateXnaDisplayToTileset();

            mDisplayer.UpdateDisplayedProperties();
            mDisplayer.PropertyGrid.Refresh();

            if (raiseChangedEvent && AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }
            return newProperty;
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
                    AppState.Self.CurrentMapTilesetTile.properties.Remove(property);
                    mDisplayer.UpdateDisplayedProperties();
                    mDisplayer.PropertyGrid.Refresh();
                    UpdateXnaDisplayToTileset();
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
        internal void EntitiesComboBoxChanged(string entityToCreate)
        {
            var tileset = AppState.Self.CurrentMapTilesetTile;

            var existingProperty = GetExistingProperty(EntityToCreatePropertyName, CurrentTilesetTile);

            bool changesOccurred = false;
            if (!string.IsNullOrEmpty(entityToCreate) && existingProperty == null)
            {
                // New property added
                const bool raiseChangedEvent = false;
                existingProperty = AddProperty(CurrentTilesetTile, EntityToCreatePropertyName, "string",
                    raiseChangedEvent);
                existingProperty.value = entityToCreate;

                changesOccurred = true;

                if (GetExistingProperty("Name", CurrentTilesetTile) == null)
                {
                    AddRandomNameTo(CurrentTilesetTile);
                }
            }
            else if (!string.IsNullOrEmpty(entityToCreate) && existingProperty != null)
            {
                // existingProperty is not null, so check if it changed
                if (existingProperty.value != entityToCreate)
                {
                    // Changed
                    changesOccurred = true;
                    existingProperty.value = entityToCreate;
                }
            }
            else if (string.IsNullOrEmpty(entityToCreate) && existingProperty != null)
            {
                // The property was removed
                CurrentTilesetTile.properties.Remove(existingProperty);

                UpdateXnaDisplayToTileset();
                changesOccurred = true;
            }
            

            if (changesOccurred)
            {
                RefreshUiToSelectedTile();
            }

            if (changesOccurred && AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }
        }

        internal void HasCollisionsCheckBoxChanged(bool hasCollisions)
        {
            // let's see if the property exists
            var tileset = AppState.Self.CurrentMapTilesetTile;

            var existingProperty = GetExistingProperty(HasCollisionVariableName, CurrentTilesetTile);

            bool changesOccurred = false;

            if (hasCollisions && existingProperty == null)
            {
                // We'll do it after setting the value
                const bool raiseChangedEvent = false;
                existingProperty = AddProperty(CurrentTilesetTile, HasCollisionVariableName, "bool", raiseChangedEvent);
                existingProperty.value = hasCollisions.ToString();
                changesOccurred = true;


                if (GetExistingProperty("Name", CurrentTilesetTile) == null)
                {
                    AddRandomNameTo(CurrentTilesetTile);
                }

            }
            else if (hasCollisions == false && existingProperty != null)
            {
                CurrentTilesetTile.properties.Remove(existingProperty);

                UpdateXnaDisplayToTileset();
                changesOccurred = true;
            }


            if (changesOccurred)
            {
                RefreshUiToSelectedTile();
            }

            if (changesOccurred && AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }
        }

        private void AddRandomNameTo(mapTilesetTile tile)
        {
            string value = "Unnamed1";

            while (GetTilesetTileByName(value) != null)
            {
                value = StringFunctions.IncrementNumberAtEnd(value);
            }

            AddProperty(tile, "Name", "string", false).value = value;
        }

        private object GetTilesetTileByName(string value)
        {
            foreach (var tileset in AppState.Self.CurrentTiledMapSave.Tilesets)
            {
                foreach (var tile in tileset.Tiles)
                {
                    foreach (var property in tile.properties)
                    {
                        if (property.GetStrippedName(property.name).ToLower() == "name" &&
                            property.value == value)
                        {
                            return tile;
                        }

                    }
                }


            }

            return null;
        }


        private void HandleNameChange()
        {
            var tileset = AppState.Self.CurrentMapTilesetTile;

            if (tileset != null)
            {

                var existingProperty = GetExistingProperty("Name", CurrentTilesetTile);

                bool changesOccurred = false;

                bool hasName = string.IsNullOrEmpty(mNameTextBox.Text) == false;

                if (hasName)
                {
                    if (existingProperty == null)
                    {
                        // There's no property for Name, so let's add it...
                        const bool raiseChangedEvent = false;
                        existingProperty = AddProperty(CurrentTilesetTile, "Name", "string", raiseChangedEvent);
                    }

                    // ...and now that we've added, let's modify it:
                    existingProperty.value = mNameTextBox.Text;
                    changesOccurred = true;
                }
                else if (hasName == false && existingProperty != null)
                {
                    CurrentTilesetTile.properties.Remove(existingProperty);

                    UpdateXnaDisplayToTileset();
                    changesOccurred = true;
                }


                if (changesOccurred && AnyTileMapChange != null)
                {
                    AnyTileMapChange(this, null);
                }
            }
        }

        private void RefreshUiToSelectedTile()
        {
            mDisplayer.Instance = mCurrentTilesetTile;
            mDisplayer.PropertyGrid.Refresh();

            UpdateHighlightRectangle();

            mHasCollisionsCheckBox.Enabled = mCurrentTilesetTile != null;
            mNameTextBox.Enabled = mCurrentTilesetTile != null;
            mEntitiesComboBox.Enabled = mCurrentTilesetTile != null;

            if (mHasCollisionsCheckBox.Enabled)
            {
                Func<property, bool> predicate = item => property.GetStrippedName( item.name ) == TilesetController.HasCollisionVariableName;

                mHasCollisionsCheckBox.Checked =
                    mCurrentTilesetTile.properties.Any(predicate) &&
                    mCurrentTilesetTile.properties.First(predicate).value.ToLowerInvariant() == "true";

                var entityProperty =
                    mCurrentTilesetTile.properties.FirstOrDefault(item => property.GetStrippedName(item.name) == TilesetController.EntityToCreatePropertyName);
                if (entityProperty != null)
                {
                    foreach (var item in mEntitiesComboBox.Items)
                    {
                        var value = item as string;
                        if (value == entityProperty.value)
                        {
                            mEntitiesComboBox.SelectedItem = item;
                            break;
                        }
                    }
                }

                var nameProperty = mCurrentTilesetTile.properties.FirstOrDefault(item => property.GetStrippedName(item.name) == "Name");

                if (nameProperty != null)
                {
                    mNameTextBox.Text = nameProperty.value;
                }
                else
                {
                    mNameTextBox.Text = "";
                }
            }
            else
            {
                mHasCollisionsCheckBox.Checked = false;
                mNameTextBox.Text = "";
                mEntitiesComboBox.SelectedValue = null;
            }

            
        }

        
    }
}
