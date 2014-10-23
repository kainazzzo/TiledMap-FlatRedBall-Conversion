using System;
using TMXGlueLib;
using FlatRedBall.IO;
using System.IO;

namespace TmxToScnx
{
    public class TmxFileCopier
    {
        public static bool CopyTmxTilesetImagesToDestination(string sourceTmx, string destinationScnx, TiledMapSave tms)
        {
            bool success = true;
            //////////Early Out///////////////////
            if (tms.Tilesets == null)
            {
                // Not sure if we should consider this a success or failure
                return success;
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
                    bool didCopySucceed = CopyTilesetImage(tmxPath, destinationPath, tileset, image);

                    if(!didCopySucceed)
                    {
                        success = false;
                    }
                }
            }

            return success;
        }

        private static bool CopyTilesetImage(string tmxPath, string destinationPath, Tileset tileset, TilesetImage image)
        {
            string sourcepath = GetImageSourcePath(tmxPath, tileset, image);

            string whyCantCopy = null;

            if(!System.IO.File.Exists(sourcepath))
            {
                whyCantCopy = "Could not find the file\n" + sourcepath + "\nwhich is referenced by the tileset " + tileset.Name + " in the tmx\n" + tmxPath;
            }

            if (!string.IsNullOrEmpty(whyCantCopy))
            {
                System.Console.Error.WriteLine(whyCantCopy);
            }
            else
            {

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
                    catch (Exception e)
                    {
                        System.Console.Error.WriteLine("Could not copy \"{0}\" to \"{1}\" \n{2}.", sourcepath, destinationFullPath, e.ToString());

                    }
                }
            }

            bool succeeded = string.IsNullOrEmpty(whyCantCopy);

            return succeeded;
        }

        private static string GetImageSourcePath(string tmxPath, Tileset tileset, TilesetImage image)
        {
            // The image may be absolute, so only prepend the tmx path if the image is relative
            string sourcepath;

            if (FileManager.IsRelative(image.Source))
            {
                sourcepath = tmxPath + image.Source;
            }
            else
            {
                sourcepath = image.Source;
            }
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
