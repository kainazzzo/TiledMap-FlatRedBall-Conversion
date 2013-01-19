using System;
using TMXGlueLib;
using FlatRedBall.IO;
using System.IO;

namespace TmxToScnx
{
    public class TmxFileCopier
    {
        public static void CopyTmxTilesetImagesToDestination(string sourceTmx, string destinationScnx, TiledMapSave tms)
        {
            //////////Early Out///////////////////
            if (tms.tileset == null)
            {
                return;
            }
            ////////End Early Out////////////////

            string tmxPath = sourceTmx.Substring(0, sourceTmx.LastIndexOf('\\'));
            string destinationPath = destinationScnx.Contains("\\") ? destinationScnx.Substring(0, destinationScnx.LastIndexOf('\\')) : ".";

            foreach (mapTileset tileset in tms.tileset)
            {
                foreach (mapTilesetImage image in tileset.Image)
                {
                    string sourcepath = tmxPath + "\\" + image.source;
                    if (tileset.Source != null)
                    {
                        if (tileset.SourceDirectory != ".")
                        {
                            sourcepath = tmxPath + "\\" + tileset.SourceDirectory + "\\" + image.source;
                        }
                        else
                        {
                            sourcepath = tmxPath + "\\" + image.source;

                        }
                    }
                    string destinationFullPath = destinationPath + "\\" + image.sourceFileName;
                    if (!sourcepath.Equals(destinationFullPath, StringComparison.InvariantCultureIgnoreCase) && 
                        !FileManager.GetDirectory(destinationFullPath).Equals(FileManager.GetDirectory(sourcepath)))
                    {
                        Console.WriteLine("Copying \"{0}\" to \"{1}\".", sourcepath, destinationFullPath);

                        File.Copy(sourcepath, destinationFullPath, true);
                    }
                }
            }
        }

        public static void FixupImageSources(TiledMapSave tms)
        {
            Console.WriteLine("Fixing up tileset relative paths");
            if (tms.tileset != null)
            {
                foreach (mapTileset tileset in tms.tileset)
                {
                    foreach (mapTilesetImage image in tileset.Image)
                    {
                        image.source = image.sourceFileName;
                    }
                }
            }
        }
    }
}
