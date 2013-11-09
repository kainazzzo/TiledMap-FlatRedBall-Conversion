using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMXGlueLib;
using FlatRedBall.IO;

namespace TmxEditor
{
    public class ProjectManager
    {
        #region Fields

        static ProjectManager mSelf;

        #endregion

        #region Properties

        public static ProjectManager Self
        {
            get { return mSelf ?? (mSelf = new ProjectManager()); }
        }

        public TiledMapSave TiledMapSave { get; private set; }

        public string LastLoadedFile
        {
            get;
            private set;
        }
        #endregion

        public string MakeAbsolute(string fileName)
        {
            return FileManager.GetDirectory(LastLoadedFile) + fileName;
        }

        public void LoadTiledMapSave(string fileName)
        {
            LastLoadedFile = fileName;
            TiledMapSave = TiledMapSave.FromFile(fileName);


        }

        public void SaveTiledMapSave(string fileName)
        {
            TiledMapSave.Save(fileName);
        }

        internal void LoadTilesetFrom(string fileName, out string output)
        {
            TiledMapSave toCopyFrom = TiledMapSave.FromFile(fileName);

            var stringBuilder = new StringBuilder();


            foreach (var tileset in toCopyFrom.tileset)
            {
                var copyTo = GetTilesetByName(TiledMapSave, tileset.Name);

                if (copyTo != null)
                {
                    copyTo.RefreshTileDictionary();

                    stringBuilder.AppendLine("Modified " + tileset.Name + " count before: " + copyTo.Tiles.Count + ", count after: " + tileset.Tiles.Count);

                    copyTo.Tiles = tileset.Tiles;

                }
            }

            output = stringBuilder.ToString();
        }


        internal Tileset GetTilesetByName(TiledMapSave tms, string name)
        {
            return tms.tileset.FirstOrDefault(tileset => tileset != null && tileset.Name == name);
        }
    }
}
