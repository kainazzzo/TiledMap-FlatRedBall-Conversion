using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using FlatRedBall;
using FlatRedBall.Content;
using System.IO;

namespace TmxToScnx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: tmxtoscnx.exe <input.tmx> <output.scnx>");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationScnx = args[1];

                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                SpriteEditorScene save = tms.ToSpriteEditorScene();

                copyTmxTilesetImagesToDestination(sourceTmx, destinationScnx, tms);

                // Fix up the image sources to be relative to the newly copied ones.
                fixupImageSources(tms);
                tms.ToSpriteEditorScene().Save(destinationScnx);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }

        private static void fixupImageSources(TiledMapSave tms)
        {
            foreach (TiledMap.mapTileset tileset in tms.tileset)
            {
                foreach (TiledMap.mapTilesetImage image in tileset.image)
                {
                    image.source = image.sourceFileName;
                }
            }
        }

        private static void copyTmxTilesetImagesToDestination(string sourceTmx, string destinationScnx, TiledMapSave tms)
        {
            string tmxPath = sourceTmx.Substring(0, sourceTmx.LastIndexOf('\\'));
            string destinationPath;
            if (destinationScnx.Contains("\\"))
            {
                destinationPath = destinationScnx.Substring(0, destinationScnx.LastIndexOf('\\'));
            }
            else
            {
                destinationPath = ".";
            }

            foreach (TiledMap.mapTileset tileset in tms.tileset)
            {
                foreach (TiledMap.mapTilesetImage image in tileset.image)
                {
                    string sourcepath = tmxPath + "\\" + image.source;
                    File.Copy(sourcepath, destinationPath + "\\" + image.sourceFileName, true);
                }
            }
        }
    }
}
