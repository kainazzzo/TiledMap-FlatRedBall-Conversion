using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using FlatRedBall;
using FlatRedBall.Content;

namespace TmxToScnx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: tmxtoscnx.exe <input.tmx> <output.scnx>");
                return;
            }

            try
            {
                TiledMapSave tms = TiledMapSave.FromFile(args[0]);

                SpriteEditorScene save = tms.ToSpriteEditorScene();
                save.Save(args[1]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }
    }
}
