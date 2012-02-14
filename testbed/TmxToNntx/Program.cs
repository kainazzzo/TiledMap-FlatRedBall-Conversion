using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using FlatRedBall;
using FlatRedBall.Content;
using System.IO;
using FlatRedBall.IO;
using FlatRedBall.Content.AI.Pathfinding;

namespace TmxToScnx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: tmxtoscnx.exe <input.tmx> <output.nntx>");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationNntx = args[1];

                TiledMapSave tms = TiledMapSave.FromFile(sourceTmx);
                // Convert once in case of any exceptions
                NodeNetworkSave save = tms.ToNodeNetworkSave();

                save.Save(destinationNntx);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }
    }
}
