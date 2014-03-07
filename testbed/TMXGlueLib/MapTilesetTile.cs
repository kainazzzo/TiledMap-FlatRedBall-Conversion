﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TMXGlueLib
{

        [XmlType(AnonymousType = true)]
        public partial class mapTilesetTile
        {
            private IDictionary<string, string> propertyDictionaryField = null;

            [XmlIgnore]
            public IDictionary<string, string> PropertyDictionary
            {
                get
                {
                    lock (this)
                    {
                        if (propertyDictionaryField == null)
                        {
                            ForceRebuildPropertyDictionary();
                        }
                        return propertyDictionaryField;
                    }
                }
            }



            List<property> mProperties = new List<property>();

            public List<property> properties
            {
                get { return mProperties; }
                set
                {
                    mProperties = value;
                }
            }

            /// <remarks/>
            [XmlAttribute()]
            public int id
            {
                get;
                set;
            }

            public override string ToString()
            {
                string toReturn = id.ToString();

                if(PropertyDictionary.Count != null)
                {
                    toReturn += " (";

                    foreach (var kvp in PropertyDictionary)
                    {
                        toReturn += "(" + kvp.Key + "," + kvp.Value + ")";
                    }


                    toReturn += ")";
                }
                return toReturn;
            }

            public void ForceRebuildPropertyDictionary()
            {
                propertyDictionaryField = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);
            }
        }
}
