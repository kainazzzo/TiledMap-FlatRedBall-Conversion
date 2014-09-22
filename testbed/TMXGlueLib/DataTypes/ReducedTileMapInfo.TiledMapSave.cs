using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FlatRedBall.Content;
using FlatRedBall.Content.Scene;

namespace TMXGlueLib.DataTypes
{
    public partial class ReducedQuadInfo
    {


        internal static ReducedQuadInfo FromSpriteSave(SpriteSave spriteSave, int textureWidth, int textureHeight)
        {
            ReducedQuadInfo toReturn = new ReducedQuadInfo();
            toReturn.LeftQuadCoordinate = spriteSave.X - spriteSave.ScaleX;
            toReturn.BottomQuadCorodinate = spriteSave.Y - spriteSave.ScaleY;

            
            bool isRotated = spriteSave.RotationZ != 0;
            if (isRotated)
            {
                toReturn.FlipFlags = (byte)(toReturn.FlipFlags | ReducedQuadInfo.FlippedDiagonallyFlag);
            }

            var leftTextureCoordinate = System.Math.Min( spriteSave.LeftTextureCoordinate, spriteSave.RightTextureCoordinate);
            var topTextureCoordinate = System.Math.Min(spriteSave.TopTextureCoordinate, spriteSave.BottomTextureCoordinate);

            if (spriteSave.LeftTextureCoordinate > spriteSave.RightTextureCoordinate)
            {
                toReturn.FlipFlags = (byte)(toReturn.FlipFlags | ReducedQuadInfo.FlippedHorizontallyFlag);
            }
            
            if(spriteSave.TopTextureCoordinate > spriteSave.BottomTextureCoordinate)
            {
                toReturn.FlipFlags = (byte)(toReturn.FlipFlags | ReducedQuadInfo.FlippedVerticallyFlag);
            }
            
            toReturn.LeftTexturePixel = (ushort)FlatRedBall.Math.MathFunctions.RoundToInt(leftTextureCoordinate * textureWidth);
            toReturn.TopTexturePixel = (ushort)FlatRedBall.Math.MathFunctions.RoundToInt(topTextureCoordinate * textureHeight);

            toReturn.Name = spriteSave.Name;

            return toReturn;
        }


    }

    public partial class ReducedTileMapInfo
    {
        private static Point GetTextureDimensions(string directory, Dictionary<string, Point> loadedTextures, SpriteSave spriteSave)
        {
            string absoluteFile = spriteSave.Texture;
            if (FlatRedBall.IO.FileManager.IsRelative(absoluteFile))
            {
                absoluteFile = directory + absoluteFile;
            }

            Point point;
            if (loadedTextures.ContainsKey(absoluteFile))
            {
                point = loadedTextures[absoluteFile];
            }
            else
            {
                point = GetDimensions(absoluteFile);
                loadedTextures.Add(absoluteFile, point);
            }
            return point;
        }
        static Point GetDimensions(string texture)
        {
            var dimensions = ImageHeader.GetDimensions(texture);
            return new Point(dimensions.Width, dimensions.Height);
        }



        public static ReducedTileMapInfo FromTiledMapSave(TiledMapSave tiledMapSave, float scale, float zOffset, string directory, FileReferenceType referenceType)
        {
            var toReturn = new ReducedTileMapInfo();

            toReturn.NumberCellsTall = tiledMapSave.Height;
            toReturn.NumberCellsWide = tiledMapSave.Width;

            var ses = tiledMapSave.ToSceneSave(scale, referenceType);

            ses.SpriteList.Sort((first, second) => first.Z.CompareTo(second.Z));

            ReducedLayerInfo reducedLayerInfo = null;


            Dictionary<string, Point> loadedTextures = new Dictionary<string, Point>();


            if (ses.SpriteList.Count != 0)
            {
                SpriteSave spriteSave = ses.SpriteList[0];
                Point point = GetTextureDimensions(directory, loadedTextures, spriteSave);

                if (tiledMapSave.Tilesets.Count != 0)
                {
                    toReturn.CellHeightInPixels =
                        (ushort)tiledMapSave.Tilesets[0].Tileheight;

                    toReturn.CellWidthInPixels =
                        (ushort)tiledMapSave.Tilesets[0].Tilewidth;
                }
                else
                {
                    toReturn.CellHeightInPixels = (ushort)FlatRedBall.Math.MathFunctions.RoundToInt((spriteSave.BottomTextureCoordinate - spriteSave.TopTextureCoordinate) * point.Y);
                    toReturn.CellWidthInPixels = (ushort)FlatRedBall.Math.MathFunctions.RoundToInt((spriteSave.RightTextureCoordinate - spriteSave.LeftTextureCoordinate) * point.X);
                }

                toReturn.QuadWidth = spriteSave.ScaleX * 2;
                toReturn.QuadHeight = spriteSave.ScaleY * 2;
            }

            int indexInLayer = 0;
            int layerIndex = 0;
            float z = float.NaN;

            for(int i = 0; i < ses.SpriteList.Count; i++)
            {
            //foreach (SpriteSave spriteSave in ses.SpriteList)
            //{
                SpriteSave spriteSave = ses.SpriteList[i];

                if (spriteSave.Z != z)
                {
                    indexInLayer = 0;

                    z = spriteSave.Z;
                    reducedLayerInfo = new ReducedLayerInfo();
                    reducedLayerInfo.Z = spriteSave.Z;
                    reducedLayerInfo.Texture = spriteSave.Texture;

                    // This doesn't work if the map has a non-zero Z offset:
                    reducedLayerInfo.Name = tiledMapSave.Layers[ FlatRedBall.Math.MathFunctions.RoundToInt(z - zOffset)].Name;
                    //reducedLayerInfo.Name = tiledMapSave.Layers[layerIndex].Name;

                    toReturn.Layers.Add(reducedLayerInfo);
                    layerIndex++;
                }

                Point point = GetTextureDimensions(directory, loadedTextures, spriteSave);

                ReducedQuadInfo quad = ReducedQuadInfo.FromSpriteSave(spriteSave, point.X, point.Y);


                reducedLayerInfo.Quads.Add(quad);

            }

            return toReturn;



        }


    }
}
