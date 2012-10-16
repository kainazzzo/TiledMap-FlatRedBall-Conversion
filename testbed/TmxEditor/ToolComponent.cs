using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RenderingLibrary;

namespace TmxEditor
{
    public abstract class ToolComponent
    {
        public Action<string> ReactToLoadedFile;
        public Action<string> ReactToLoadedAndMergedProperties;
        public Action<SystemManagers> ReactToXnaInitialize;
        public Action ReactToWindowResize;
    }
}
