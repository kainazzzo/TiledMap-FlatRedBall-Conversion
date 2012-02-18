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
using FlatRedBall.Math.Geometry;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Content.Math.Geometry;

namespace TmxToShcx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Usage: tmxtoshcx.exe <input.tmx> <output.shcx> [layerName]");
                return;
            }

            try
            {
                string sourceTmx = args[0];
                string destinationShcx = args[1];
                string layername = null;

                if (args.Length >= 3)
                {
                    layername = args[2];
                }

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
    }
}
