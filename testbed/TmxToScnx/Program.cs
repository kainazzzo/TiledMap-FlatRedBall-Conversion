using System;
using System.IO;
using FlatRedBall.Content;
using FlatRedBall.Content.Scene;
using FlatRedBall.IO;
using TMXGlueLib;
using TMXGlueLib.DataTypes;

namespace TmxToScnx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Usage: tmxtoscnx.exe <input.tmx> <output.scnx or output.tilb> [scale=##.#] [layervisibilitybehavior=Ignore|Skip|Match] [offset=xf,yf,zf] copyimages=true|false");
                return;
            }

            try
            {


                TmxToScnxCommandLineArgs parsedArgs = new TmxToScnxCommandLineArgs();
                parsedArgs.ParseOptionalCommandLineArgs(args);

                TiledMapSave.LayerVisibleBehaviorValue = parsedArgs.LayerVisibleBehavior;
                TiledMapSave.Offset = parsedArgs.Offset;
                TiledMapSave tms = TiledMapSave.FromFile(parsedArgs.SourceFile);
                // Convert once in case of any exceptions

                System.Console.WriteLine("{0} converted successfully.", parsedArgs.SourceFile);

                if (parsedArgs.CopyImages)
                {
                    TmxFileCopier.CopyTmxTilesetImagesToDestination(parsedArgs.SourceFile, parsedArgs.DestinationFile, tms);
                }

                // Fix up the image sources to be relative to the newly copied ones.
                // I don't know why we're doing this, but it wipes out the old relative 
                // directory structure.  We should not do this...
                //TmxFileCopier.FixupImageSources(tms);

                PerformSave(parsedArgs, tms);

                System.Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Error: [" + ex.Message + "] Stack trace: [" + ex.StackTrace + "]");
            }
        }

        private static void PerformSave(TmxToScnxCommandLineArgs parsedArgs, TiledMapSave tms)
        {
            string extension = FileManager.GetExtension(parsedArgs.DestinationFile);

            if (extension == "scnx")
            {

                System.Console.WriteLine("Saving \"{0}\".", parsedArgs.DestinationFile);

                SceneSave spriteEditorScene = tms.ToSceneSave(parsedArgs.Scale);

                spriteEditorScene.FileName = parsedArgs.DestinationFile;
                spriteEditorScene.ScenePath = FileManager.GetDirectory(parsedArgs.DestinationFile);
                spriteEditorScene.AssetsRelativeToSceneFile = true;
                var result = spriteEditorScene.GetMissingFiles();
                foreach(var missing in result)
                {
                    System.Console.Error.WriteLine("Missing file: " + spriteEditorScene.ScenePath + missing);
                }
                spriteEditorScene.Save(parsedArgs.DestinationFile.Trim());
            }
            else if (extension == "tilb")
            {
                System.Console.WriteLine("Saving \"{0}\".", parsedArgs.DestinationFile);
                // all files should have been copied over, and since we're using the .scnx files,
                // we are going to use the destination instead of the source.

                FileReferenceType referenceType = FileReferenceType.NoDirectory;

                if (parsedArgs.CopyImages == false)
                {
                    referenceType = FileReferenceType.Absolute;
                }

                ReducedTileMapInfo rtmi = 
                    ReducedTileMapInfo.FromTiledMapSave(tms, parsedArgs.Scale, FileManager.GetDirectory(parsedArgs.DestinationFile), referenceType);

                if (parsedArgs.CopyImages == false)
                {
                    foreach (var item in rtmi.Layers)
                    {
                        item.Texture = FileManager.MakeRelative(item.Texture, FileManager.GetDirectory(parsedArgs.SourceFile));
                    }
                }

                if (System.IO.File.Exists(parsedArgs.DestinationFile))
                {
                    System.IO.File.Delete(parsedArgs.DestinationFile);
                }

                using (Stream outputStream =System.IO.File.OpenWrite(parsedArgs.DestinationFile))
                using (BinaryWriter binaryWriter = new BinaryWriter(outputStream))
                {
                    rtmi.WriteTo(binaryWriter);
                    System.Console.WriteLine("Saved \"{0}\".", parsedArgs.DestinationFile);

                }

            }
            else
            {
                System.Console.WriteLine("The following extension is not understood: " + extension);
            }
        }
    }
}
