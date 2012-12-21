using System;
using FlatRedBall.Content.Math.Geometry;
using TMXGlueLib;

namespace TmxToShcx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tmxtoshcx.exe <input.tmx> <output.shcx> [layername=name] [layervisibilitybehavior=Ignore|Skip|Match]");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationShcx = args[1];
                string layername = null;
                var layerVisibilityBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;

                if (args.Length >= 3)
                {
                    ParseOptionalCommandLineArgs(args, out layername, out layerVisibilityBehavior);
                }
                TiledMapSave.LayerVisibleBehaviorValue = layerVisibilityBehavior;
                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                ShapeCollectionSave save = tms.ToShapeCollectionSave(layername);

                save.Save(destinationShcx.Trim());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }

        private static void ParseOptionalCommandLineArgs(string[] args, out string layername,
            out TiledMapSave.LayerVisibleBehavior layerVisibilityBehavior)
        {
            layername = "";
            layerVisibilityBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
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
                        case "layername":
                            layername = value;
                            break;
                        case "layervisibilitybehavior":
                            if (!Enum.TryParse(value, out layerVisibilityBehavior))
                            {
                                layerVisibilityBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
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
