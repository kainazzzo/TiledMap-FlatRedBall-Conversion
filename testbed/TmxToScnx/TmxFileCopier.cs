using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using FlatRedBall.IO;
using System.IO;

namespace TmxToScnx
{
    public class TmxFileCopier
    {
        public static void CopyTmxTilesetImagesToDestination(string sourceTmx, string destinationScnx, TiledMapSave tms)
        {
            string tmxPath = sourceTmx.Substring(0, sourceTmx.LastIndexOf('\\'));
            string destinationPath;
            if (destinationScnx.Contains("\\"))
            {
                destinationPath = destinationScnx.Substring(0, destinationScnx.LastIndexOf('\\'));
            }
            else
            {
                destinationPath = ".";
            }

            foreach (TiledMap.mapTileset tileset in tms.tileset)
            {
                foreach (TiledMap.mapTilesetImage image in tileset.image)
                {
                    string sourcepath = tmxPath + "\\" + image.source;
                    if (tileset.source != null)
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
                        Console.WriteLine(string.Format("Copying \"{0}\" to \"{1}\".", sourcepath, destinationFullPath));

                        File.Copy(sourcepath, destinationFullPath, true);
                    }
                }
            }
        }

        public static void FixupImageSources(TiledMapSave tms)
        {
            Console.WriteLine("Fixing up tileset relative paths");
            foreach (TiledMap.mapTileset tileset in tms.tileset)
            {
                foreach (TiledMap.mapTilesetImage image in tileset.image)
                {
                    image.source = image.sourceFileName;
                }
            }
        }
    }
}
