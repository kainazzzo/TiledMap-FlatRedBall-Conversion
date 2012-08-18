using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using FlatRedBall;
using FlatRedBall.Content;
using System.IO;
using FlatRedBall.IO;

namespace TmxToScnx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tmxtoscnx.exe <input.tmx> <output.scnx> [scale=##.#]");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationScnx = args[1];
                float scale = 1.0f;
                if (args.Length >= 3)
                {
                    for (int x = 2; x < args.Length; ++x)
                    {
                        string arg = args[x];
                        string[] tokens = arg.Split("=".ToCharArray());
                        if (tokens != null && tokens.Length == 2)
                        {
                            string key = tokens[0];
                            string value = tokens[1];

                            switch(key)
                            {
                                case "scale":
                                    if (!float.TryParse(value, out scale))
                                    {
                                        scale = 1.0f;
                                    }
                                    break;
                                default:
                                    Console.Error.WriteLine("Invalid command line argument: {0}", arg);
                                    break;
                            }
                        }
                    }
                }

                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                SpriteEditorScene save = tms.ToSpriteEditorScene(scale);

                Console.WriteLine(string.Format("{0} converted successfully.", sourceTmx));
                copyTmxTilesetImagesToDestination(sourceTmx, destinationScnx, tms);

                // Fix up the image sources to be relative to the newly copied ones.
                fixupImageSources(tms);

                Console.WriteLine(string.Format("Saving \"{0}\".", destinationScnx));
                tms.ToSpriteEditorScene(scale).Save(destinationScnx.Trim());
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }

        private static void fixupImageSources(TiledMapSave tms)
        {
            Console.WriteLine("Fixing up tileset relative paths");
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
                    if (tileset.source != null)
                    {
                        sourcepath = tmxPath + "\\" + tileset.SourceDirectory + "\\" + image.source;
                    }
                    string destinationFullPath = destinationPath + "\\" + image.sourceFileName;
                    if (!sourcepath.Equals(destinationFullPath, StringComparison.InvariantCultureIgnoreCase) && 
                        !FileManager.GetDirectory(destinationFullPath).Equals(FileManager.GetDirectory(sourcepath)))
                    {
                        Console.WriteLine(string.Format("Copying \"{0}\" to \"{1}\".", sourcepath, destinationFullPath));
                        File.Copy(sourcepath, destinationFullPath, true);
                    }
                }
            }
        }
    }
}
