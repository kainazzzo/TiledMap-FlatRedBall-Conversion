using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;

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

                var copyTo = GetTilesetByName(mTiledMapSave, tileset.name);

                if (tileset != null && copyTo != null)
                {

                    stringBuilder.AppendLine("Modified " + tileset.name + " count before: " + copyTo.tile.Count + ", count after: " + tileset.tile.Count);

                    copyTo.tile = tileset.tile;


                }
            }

            output = stringBuilder.ToString();
        }


        internal mapTileset GetTilesetByName(TiledMapSave tms, string name)
        {
            foreach (var tileset in tms.tileset)
            {
                if (tileset != null && tileset.name == name)
                {
                    return tileset;
                }
            }

            return null;

        }

    }
}
