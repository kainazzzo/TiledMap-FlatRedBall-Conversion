using System;
using SimpleLogging;
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

            var oldDir = FileManager.RelativeDirectory;

            FileManager.RelativeDirectory = FileManager.GetDirectory(sourceTmx);
            string tmxPath = FileManager.RelativeDirectory;
            string destinationPath = FileManager.GetDirectory(destinationScnx);

            foreach (mapTileset tileset in tms.tileset)
            {
                foreach (mapTilesetImage image in tileset.Image)
                {
                    Logger.Log("Image source: {0}", image.source);
                    string sourcepath = tmxPath + image.source;
                    if (tileset.Source != null)
                    {
                        if (tileset.SourceDirectory != "." && !tileset.SourceDirectory.Contains(":"))
                        {
                            sourcepath = tmxPath +  tileset.SourceDirectory.Replace("\\", "/") + "/" + image.source;
                            sourcepath = FileManager.GetDirectory(sourcepath) + image.source;
                        }
                        else if (tileset.SourceDirectory.Contains(":"))
                        {
                            sourcepath = tileset.SourceDirectory + "/" + image.source;
                        }
                        else
                        {
                            sourcepath = tmxPath + image.source;
                        }
                    }
                    string destinationFullPath = destinationPath + image.sourceFileName;
                    if (!sourcepath.Equals(destinationFullPath, StringComparison.InvariantCultureIgnoreCase) && 
                        !FileManager.GetDirectory(destinationFullPath).Equals(FileManager.GetDirectory(sourcepath)))
                    {
                        Logger.Log("Copying \"{0}\" to \"{1}\".", sourcepath, destinationFullPath);

                        File.Copy(sourcepath, destinationFullPath, true);
                    }
                }
            }
        }

        public static void FixupImageSources(TiledMapSave tms)
        {
            Logger.Log("Fixing up tileset relative paths");
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
