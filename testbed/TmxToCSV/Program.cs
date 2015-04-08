using System;
using System.IO;
using System.Reflection;
using TMXGlueLib;

namespace TmxToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Usage: tmxtocsv.exe <input.tmx> <output.csv> [type=Tile|Layer|Map|Object] [layername=name]");
                return;
            }

            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                AssemblyName assemblyName = assembly.GetName();
                Version version = assemblyName.Version;
                Console.WriteLine("TMX to CSV converter version " + version.ToString());

                string sourceTmx = args[0];
                string destinationCSV = args[1];
                string layerName = null;

                var type = TiledMapSave.CSVPropertyType.Tile;
                if (args.Length >= 3)
                {
                    ParseOptionalCommandLineArgs(args, out type, out layerName);
                }
                Console.WriteLine("Create columns from " + type);
                
                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                
                // Convert once in case of any exceptions
                string csvstring = tms.ToCSVString(type: type, layerName: layerName);

                System.IO.File.WriteAllText(destinationCSV, csvstring);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is FileNotFoundException)
                {
                    var exception = ex.InnerException;
                    Console.Error.WriteLine("Error: [" + exception.Message + "] Stack trace: [" + exception.StackTrace + "]");
                }
                else
                {
                    Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
                }
            }
        }

        private static void ParseOptionalCommandLineArgs(string[] args, out TiledMapSave.CSVPropertyType type, out string layerName)
        {
            type = TiledMapSave.CSVPropertyType.Tile;
            layerName = null;
            if (args.Length >= 3)
            {
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
                            case "type":
                                const bool ignoreCase = true;
                                if (!Enum.TryParse(value, ignoreCase, out type))
                                {
                                    type = TiledMapSave.CSVPropertyType.Tile;
                                }
                                break;
                            case "layername":
                                layerName = value;
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
}
