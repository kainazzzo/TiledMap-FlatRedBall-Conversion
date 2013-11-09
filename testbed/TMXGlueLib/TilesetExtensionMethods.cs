using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMXGlueLib
{
    public static class TilesetExtensionMethods
    {
        public static void CoordinateToIndex(this Tileset tileset, int xCoordinate, int yCoordinate, out int xIndex, out int yIndex)
        {
            xIndex = 0;
            yIndex = 0;
            if (tileset.Image.Length != 0)
            {
                // We're assuming the first image, not sure why we'd have multiple images in one tileset....or at least we won't 
                // supportthat yet.
                var image = tileset.Image[0];

                int effectiveImageWidth = tileset.Image[0].width;
                int effectiveImageHeight = tileset.Image[0].height;

                if (xCoordinate < effectiveImageWidth && yCoordinate < effectiveImageHeight)
                {


                    if (tileset.Margin != 0)
                    {
                        xCoordinate -= tileset.Margin * 2;
                        yCoordinate -= tileset.Margin * 2;
                    }


                    int effectiveTileWidth = tileset.Tilewidth + tileset.Spacing;
                    int effectiveTileHeight = tileset.Tileheight + tileset.Spacing;

                    xIndex = xCoordinate / effectiveTileWidth;
                    yIndex = yCoordinate / effectiveTileHeight;

                }

            }
        }

        public static int GetNumberOfTilesWide(this Tileset tileset)
        {
            if (tileset.Image.Length == 0)
            {
                return 0;
            }
            else
            {
                int effectiveWidth = tileset.Image[0].width;
                if (tileset.Margin != 0)
                {
                    effectiveWidth -= 2 * tileset.Margin;
                }
                if (tileset.Spacing != 0)
                {
                    effectiveWidth += tileset.Spacing; // adds to the last entry
                }

                return effectiveWidth / tileset.Tilewidth;
            }
        }

        public static int IndexToLocalId(this Tileset tileset, int xIndex, int yIndex)
        {
            return xIndex + yIndex * tileset.GetNumberOfTilesWide();

        }

        public static int CoordinateToLocalId(this Tileset tileset, int xCoordinate, int yCoordinate)
        {
            int xIndex;
            int yIndex;

            tileset.CoordinateToIndex(xCoordinate, yCoordinate, out xIndex, out yIndex);

            return tileset.IndexToLocalId(xIndex, yIndex);

        }

    }
}
;