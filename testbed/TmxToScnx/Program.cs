using System;
using FlatRedBall.Content;
using TMXGlueLib;

namespace TmxToScnx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tmxtoscnx.exe <input.tmx> <output.scnx> [scale=##.#] [layervisibilitybehavior=Ignore|Skip|Match]");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationScnx = args[1];
                float scale = 1.0f;
                var layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
                if (args.Length >= 3)
                {
                    ParseOptionalCommandLineArgs(args, out scale, out layerVisibleBehavior);
                }
                TiledMapSave.LayerVisibleBehaviorValue = layerVisibleBehavior;
                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
// ReSharper disable UnusedVariable
                SpriteEditorScene save = tms.ToSpriteEditorScene(scale);
// ReSharper restore UnusedVariable

                Console.WriteLine("{0} converted successfully.", sourceTmx);
                TmxFileCopier.CopyTmxTilesetImagesToDestination(sourceTmx, destinationScnx, tms);

                // Fix up the image sources to be relative to the newly copied ones.
                TmxFileCopier.FixupImageSources(tms);

                Console.WriteLine("Saving \"{0}\".", destinationScnx);
                SpriteEditorScene spriteEditorScene = tms.ToSpriteEditorScene(scale);

                spriteEditorScene.Save(destinationScnx.Trim());
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }

        private static void ParseOptionalCommandLineArgs(string[] args, out float scale, out TiledMapSave.LayerVisibleBehavior layerVisibleBehavior)
        {
            scale = 1.0f;
            layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
            for (int x = 2; x < args.Length; ++x)
            {
                string arg = args[x];
                string[] tokens = arg.Split("=".ToCharArray());
                if (tokens.Length == 2)
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
                            if (!Enum.TryParse(value, out layerVisibleBehavior))
                            {
                                layerVisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
                            }
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
