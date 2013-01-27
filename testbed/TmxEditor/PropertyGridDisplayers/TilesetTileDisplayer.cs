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
            this.PropertyGrid.Refresh();
        }
    }
}
