using System;
using System.Xml.Serialization;

namespace TMXGlueLib
{
    [Serializable]
    [XmlInclude(typeof(MapLayer))]
    [XmlInclude(typeof(mapObjectgroup))]
    public class AbstractMapLayer
    {
    }
}