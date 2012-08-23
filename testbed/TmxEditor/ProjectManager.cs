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
            TiledMapSave tms = TiledMapSave.FromFile(fileName);

            foreach (var tileset in tms.tileset)
            {



            }
        }
    }
}
