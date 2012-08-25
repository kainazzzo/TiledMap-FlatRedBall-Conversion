using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;

namespace TmxEditor
{
    public class ProjectManager
    {
        static ProjectManager mSelf;

        TiledMapSave mTiledMapSave;

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


        public void LoadTiledMapSave(string fileName)
        {
            mTiledMapSave = TiledMapSave.FromFile(fileName);


        }

        public void SaveTiledMapSave(string fileName)
        {
            mTiledMapSave.Save(fileName);
        }

        internal void LoadTilesetFrom(string fileName)
        {
            TiledMapSave toCopyFrom = TiledMapSave.FromFile(fileName);

            for(int i = 0; i < toCopyFrom.tileset.Count; i++)
            {
                var tileset = toCopyFrom.tileset[i];

                var copyTo = GetTilesetByName(mTiledMapSave, tileset.name);

                if (tileset != null && copyTo != null)
                {
                    copyTo.tile = tileset.tile;
                }
            }
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
