using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RenderingLibrary;

namespace TmxEditor
{

    public abstract class ToolComponentBase
    {

        public Action<string> ReactToLoadedFile;
        public Action<string> ReactToLoadedAndMergedProperties;
        public Action<SystemManagers> ReactToXnaInitialize;
        public Action ReactToWindowResize;

    }

    public abstract class ToolComponent<T> : ToolComponentBase where T : new()
    {
        static T mSelf;

        public static T Self
        {
            get
            {
                if (mSelf == null)
                {
                    mSelf = new T();
                }
                return mSelf;
            }
        }
    }
}
