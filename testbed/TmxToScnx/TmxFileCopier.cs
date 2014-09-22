﻿using System;
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
            if (tms.Tilesets == null)
            {
                return;
            }
            ////////End Early Out////////////////

            var oldDir = FileManager.RelativeDirectory;

            FileManager.RelativeDirectory = FileManager.GetDirectory(sourceTmx);
            string tmxPath = FileManager.RelativeDirectory;
            string destinationPath = FileManager.GetDirectory(destinationScnx);

            foreach (Tileset tileset in tms.Tilesets)
            {
                foreach (TilesetImage image in tileset.Images)
                {
                    string sourcepath = GetImageSourcePath(tmxPath, tileset, image);

                    string destinationFullPath = destinationPath + image.sourceFileName;

                    if (!sourcepath.Equals(destinationFullPath, StringComparison.InvariantCultureIgnoreCase) && 
                        !FileManager.GetDirectory(destinationFullPath).Equals(FileManager.GetDirectory(sourcepath)))
                    {
                        System.Console.WriteLine("Copying \"{0}\" to \"{1}\".", sourcepath, destinationFullPath);

                        string fileWithoutDotDotSlash = FileManager.RemoveDotDotSlash(sourcepath);

                        try
                        {

                            File.Copy(fileWithoutDotDotSlash, destinationFullPath, true);
                        }
                        catch(Exception e)
                        {
                            System.Console.WriteLine("Could not copy \"{0}\" to \"{1}\" \n{2}.", sourcepath, destinationFullPath, e.ToString());

                        }
                    }
                }
            }
        }

        private static string GetImageSourcePath(string tmxPath, Tileset tileset, TilesetImage image)
        {
            string sourcepath = tmxPath + image.Source;
            if (tileset.Source != null)
            {
                if (tileset.SourceDirectory != "." && !tileset.SourceDirectory.Contains(":"))
                {
                    sourcepath = tmxPath + tileset.SourceDirectory.Replace("\\", "/") + "/" + image.sourceFileName;
                    sourcepath = FileManager.GetDirectory(sourcepath) + image.sourceFileName;
                }
                else if (tileset.SourceDirectory.Contains(":"))
                {
                    sourcepath = tileset.SourceDirectory + "/" + image.sourceFileName;
                }
                else
                {
                    sourcepath = tmxPath + image.sourceFileName;
                }
            }
            return sourcepath;
        }

        public static void FixupImageSources(TiledMapSave tms)
        {
            System.Console.WriteLine("Fixing up tileset relative paths");
            if (tms.Tilesets != null)
            {
                foreach (Tileset tileset in tms.Tilesets)
                {
                    foreach (TilesetImage image in tileset.Images)
                    {
                        image.Source = image.sourceFileName;
                    }
                }
            }
        }
    }
}
