using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            toReturn.BottomQuadCoordinate = spriteSave.Y - spriteSave.ScaleY;

            
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
        /// <summary>
        /// Converts a TiledMapSave to a ReducedTileMapInfo object
        /// </summary>
        /// <param name="tiledMapSave">The TiledMapSave to convert</param>
        /// <param name="scale">The amount to scale by - default of 1</param>
        /// <param name="zOffset">The zOffset</param>
        /// <param name="directory">The directory of the file associated with the tiledMapSave, used to find file references.</param>
        /// <param name="referenceType">How the files in the .tmx are referenced.</param>
        /// <returns></returns>
        public static ReducedTileMapInfo FromTiledMapSave(TiledMapSave tiledMapSave, float scale, float zOffset, string directory, FileReferenceType referenceType)
        {
            var toReturn = new ReducedTileMapInfo();

            toReturn.NumberCellsTall = tiledMapSave.Height;
            toReturn.NumberCellsWide = tiledMapSave.Width;

            var ses = tiledMapSave.ToSceneSave(scale, referenceType);

            ses.SpriteList.Sort((first, second) => first.Z.CompareTo(second.Z));

            ReducedLayerInfo reducedLayerInfo = null;

            // If we rely on the image, it's both slow (have to open the images), and
            // doesn't work at runtime in games:
            //Dictionary<string, Point> loadedTextures = new Dictionary<string, Point>();
            //SetCellWidthAndHeight(tiledMapSave, directory, toReturn, ses, loadedTextures);

            toReturn.CellHeightInPixels = (ushort)tiledMapSave.tileheight;
            toReturn.CellWidthInPixels = (ushort)tiledMapSave.tilewidth;


            SetQuadWidthAndHeight(toReturn, ses);

            float z = float.NaN;


            int textureWidth = 0;
            int textureHeight = 0;


            for (int i = 0; i < ses.SpriteList.Count; i++)
            {
                SpriteSave spriteSave = ses.SpriteList[i];

                if (spriteSave.Z != z)
                {
                    z = spriteSave.Z;
                    reducedLayerInfo = new ReducedLayerInfo();
                    reducedLayerInfo.Z = spriteSave.Z;
                    reducedLayerInfo.Texture = spriteSave.Texture;

                    int layerIndex = FlatRedBall.Math.MathFunctions.RoundToInt(z - zOffset);
                    var mapLayer = tiledMapSave.Layers[layerIndex];


                    // This should have data:

                    var idOfTexture = mapLayer.data[0].tiles.FirstOrDefault(item => item != 0);
                    Tileset tileSet = tiledMapSave.GetTilesetForGid(idOfTexture);
                    var tilesetIndex = tiledMapSave.Tilesets.IndexOf(tileSet);

                    textureWidth = tileSet.Images[0].width;
                    textureHeight = tileSet.Images[0].height;

                    reducedLayerInfo.Name = mapLayer.Name;
                    reducedLayerInfo.TextureId = tilesetIndex;
                    toReturn.Layers.Add(reducedLayerInfo);
                }

                ReducedQuadInfo quad = ReducedQuadInfo.FromSpriteSave(spriteSave, textureWidth, textureHeight);
                reducedLayerInfo.Quads.Add(quad);
            }
            return toReturn;



        }

        private static void SetQuadWidthAndHeight(ReducedTileMapInfo toReturn, SceneSave ses)
        {
            if (ses.SpriteList.Count != 0)
            {
                SpriteSave spriteSave = ses.SpriteList[0];

                toReturn.QuadWidth = spriteSave.ScaleX * 2;
                toReturn.QuadHeight = spriteSave.ScaleY * 2;
            }
        }
        


    }
}
