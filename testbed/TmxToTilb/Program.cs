using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using FlatRedBall.Content;
using TMXGlueLib.DataTypes;
using FlatRedBall.IO;
using TmxToScnx;
using System.IO;

namespace TmxToTilb
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tmxtotilb.exe <input.tmx> <output.tilb> [scale=##.#]");
                return;
            }


            try
            {
                string sourceTmx = args[0];
                string destinationFile = args[1];
                float scale = 1.0f;
                //TiledMapSave.LayerVisibleBehavior layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
                if (args.Length >= 3)
                {
                    ParseOptionalCommandLineArgs(args, out scale);
                }
                //TiledMapSave.layerVisibleBehavior = layerVisibleBehavior;
                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                //SpriteEditorScene save = tms.ToSpriteEditorScene(scale);


                Console.WriteLine(string.Format("{0} converted successfully.", sourceTmx));
                TmxFileCopier.CopyTmxTilesetImagesToDestination(sourceTmx, destinationFile, tms);

                // Fix up the image sources to be relative to the newly copied ones.
                TmxFileCopier.FixupImageSources(tms);

                Console.WriteLine(string.Format("Saving \"{0}\".", destinationFile));
                ReducedTileMapInfo rtmi = ReducedTileMapInfo.FromTiledMapSave(tms, scale,
                    FileManager.GetDirectory(sourceTmx));

                using(FileStream fileStream = File.OpenWrite(destinationFile))
                using (BinaryWriter writer = new BinaryWriter(fileStream))
                {
                    rtmi.WriteTo(writer);
                }

                
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }




        }



        private static void ParseOptionalCommandLineArgs(string[] args, out float scale)
        {
            scale = 1.0f;
            //layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
            for (int x = 2; x < args.Length; ++x)
            {
                string arg = args[x];
                string[] tokens = arg.Split("=".ToCharArray());
                if (tokens != null && tokens.Length == 2)
                {
                    string key = tokens[0];
                    string value = tokens[1];

                    switch (key.ToLowerInvariant())
                    {
                        case "scale":
                            if (!float.TryParse(value, out scale))
                            {
                                scale = 1.0f;
                            }
                            break;
                        case "layervisiblebehavior":
                            //if (!Enum.TryParse(value, out layerVisibleBehavior))
                            //{
                            //    layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
                            //}
                            break;
                        default:
                            Console.Error.WriteLine("Invalid command line argument: {0}", arg);
                            break;
                    }
                }
            }
        }
    }
}
