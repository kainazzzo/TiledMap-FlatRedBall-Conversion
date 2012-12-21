using System;
using FlatRedBall.Content.AI.Pathfinding;
using TMXGlueLib;

namespace TmxToNntx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: tmxtonntx.exe <input.tmx> <output.csv> [requiretile=true|false] [layervisibilitybehavior=Ignore|Skip|Match]");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationNntx = args[1];
                bool requiretile = true;
                var layerVisibilityBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
                if (args.Length >= 3)
                {
                    ParseOptionalCommandLineArgs(args, out requiretile, out layerVisibilityBehavior);
                }

                TiledMapSave.LayerVisibleBehaviorValue = layerVisibilityBehavior;
                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                NodeNetworkSave save = tms.ToNodeNetworkSave(requiretile);

                save.Save(destinationNntx);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }

        private static void ParseOptionalCommandLineArgs(string[] args, out bool requiretile, out TiledMapSave.LayerVisibleBehavior layerVisibleBehavior)
        {
            requiretile = true;
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
                        case "requiretile":
                            if (!bool.TryParse(value, out requiretile))
                            {
                                requiretile = true;
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
