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

                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                string csvstring = tms.ToCSVString();

                System.IO.File.WriteAllText(destinationCSV, csvstring);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }
    }
}
