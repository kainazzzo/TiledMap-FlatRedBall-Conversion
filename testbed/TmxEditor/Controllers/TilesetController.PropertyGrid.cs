using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TmxEditor.PropertyGridDisplayers;
using TmxEditor.UI;
using TMXGlueLib;

namespace TmxEditor.Controllers
{
    public partial class TilesetController
    {

        PropertyGrid mPropertyGrid;
        TilesetTileDisplayer mDisplayer;


        public property CurrentTilesetTileProperty
        {
            get
            {
                return mDisplayer.CurrentTilesetTileProperty;
            }
        }

        private void InitializePropertyGrid(PropertyGrid propertyGrid)
        {
            mDisplayer = new TilesetTileDisplayer();
            mDisplayer.PropertyGrid = propertyGrid;
            mDisplayer.RefreshOnTimer = false;
            mDisplayer.PropertyGrid.PropertyValueChanged += HandlePropertyValueChangeInternal;
        }


        void HandlePropertyValueChangeInternal(object s, PropertyValueChangedEventArgs e)
        {
            if (AnyTileMapChange != null)
            {
                AnyTileMapChange(this, null);
            }

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

    }
}
