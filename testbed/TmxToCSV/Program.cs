using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;

namespace TmxToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: tmxtocsv.exe <input.tmx> <output.csv> [type=tile|layer]");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationCSV = args[1];
                CSVPropertyType type = CSVPropertyType.Tile;
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

                            switch (key.ToLowerInvariant())
                            {
                                case "type":
                                    if (!Enum.TryParse<CSVPropertyType>(value, out type))
                                    {
                                        type = CSVPropertyType.Tile;
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
                string csvstring = tms.ToCSVString(type);

                System.IO.File.WriteAllText(destinationCSV, csvstring);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }
    }
}
