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

        TiledMapSave mTiledMapSave;

        #endregion

        #region Properties

        public static ProjectManager Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new ProjectManager();
                }
                return mSelf;
            }

        }

        public TiledMapSave TiledMapSave
        {
            get
            {

                return mTiledMapSave;
            }
        }

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
            mTiledMapSave = TiledMapSave.FromFile(fileName);


        }

        public void SaveTiledMapSave(string fileName)
        {
            mTiledMapSave.Save(fileName);
        }

        internal void LoadTilesetFrom(string fileName, out string output)
        {
            TiledMapSave toCopyFrom = TiledMapSave.FromFile(fileName);

            StringBuilder stringBuilder = new StringBuilder();


            for(int i = 0; i < toCopyFrom.tileset.Length; i++)
            {
                var tileset = toCopyFrom.tileset[i];

                var copyTo = GetTilesetByName(mTiledMapSave, tileset.Name);

                if (tileset != null && copyTo != null)
                {
                    copyTo.RefreshTileDictionary();

                    stringBuilder.AppendLine("Modified " + tileset.Name + " count before: " + copyTo.Tile.Count + ", count after: " + tileset.Tile.Count);

                    copyTo.Tile = tileset.Tile;

                }
            }

            output = stringBuilder.ToString();
        }


        internal mapTileset GetTilesetByName(TiledMapSave tms, string name)
        {
            foreach (var tileset in tms.tileset)
            {
                if (tileset != null && tileset.Name == name)
                {
                    return tileset;
                }
            }

            return null;

        }

    }
}
