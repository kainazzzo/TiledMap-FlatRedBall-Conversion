using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.Glue.GuiDisplay;
using TMXGlueLib;

namespace TmxEditor.PropertyGridDisplayers
{
    public class TilesetTileDisplayer : PropertyGridDisplayer
    {
        public override object Instance
        {
            get
            {
                return base.Instance;
            }
            set
            {
                mInstance = value;
                UpdatePropertiesToInstance(value as mapTilesetTile);



                base.Instance = value;


            }
        }

        mapTilesetTile TilesetTileInstance
        {
            get
            {
                return Instance as mapTilesetTile;
            }
        }

        private void UpdatePropertiesToInstance(mapTilesetTile tileSet)
        {

            if (tileSet != null)
            {
                ExcludeAllMembers();

                this.DisplayProperties(tileSet.properties);

            }
            PropertyGrid.Visible = tileSet != null;

            this.PropertyGrid.Refresh();
        }

        public property CurrentTilesetTileProperty 
        {
            get
            {

                if (PropertyGrid.SelectedGridItem == null)
                {
                    return null;
                }
                else
                {
                    string name = PropertyGrid.SelectedGridItem.Label;

                    if (this.TilesetTileInstance != null)
                    {
                        var property = DisplayerExtensionMethods.GetPropertyByName(name, TilesetTileInstance.properties);
                        return property;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
         
        }

        public void UpdateDisplayedProperties()
        {
            UpdatePropertiesToInstance(Instance as mapTilesetTile);
        }
    }
}
