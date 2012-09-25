using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledMap;
using System.Xml.Serialization;
using FlatRedBall.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace TiledMap
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class mapTileset
    {
        private mapTilesetTile[] tileField;

        private mapTilesetImage[] imageField;

        private mapTilesetTileOffset[] tileOffsetField;

        private string sourceField;

        [XmlIgnore]
        public string SourceDirectory
        {
            get
            {
                if (sourceField != null && sourceField.Contains("\\"))
                {
                    return sourceField.Substring(0, sourceField.LastIndexOf('\\'));
                }
                else
                {
                    return ".";
                }
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("source", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string source
        {
            get
            {
                return sourceField;
            }
            set
            {
                this.sourceField = value;
                if (this.sourceField != null)
                {
                    sourceField = sourceField.Replace("/", "\\");
                    tileset xts = FileManager.XmlDeserialize<tileset>(sourceField);
                    image = new mapTilesetImage[xts.image.Length];

                    try
                    {
                        Parallel.For(0, xts.image.Length, (count) =>
                        {
                            tilesetImage ximage = xts.image[count];
                            this.image[count] = new mapTilesetImage();
                            this.image[count].source = xts.image[count].source;
                            if (xts.image[count].height != 0)
                            {
                                this.image[count].height = xts.image[count].height;
                            }
                            else
                            {
                                this.image[count].height = xts.tileheight;
                            }

                            if (xts.image[count].width != 0)
                            {
                                this.image[count].width = xts.image[count].width;
                            }
                            else
                            {
                                this.image[count].width = xts.tilewidth;
                            }
                        });
                    }
                    catch (AggregateException)
                    {
                        throw;
                    }

                    this.name = xts.name;
                    this.margin = xts.margin;
                    this.spacing = xts.spacing;
                    this.tileheight = xts.tileheight;
                    this.tilewidth = xts.tilewidth;
                    this.tile = xts.tile;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tileoffset", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public mapTilesetTileOffset[] tileoffset
        {
            get
            {
                return this.tileOffsetField;
            }
            set
            {
                if (this.tileOffsetField != null && this.tileOffsetField.Length > 0)
                {
                    return;
                }
                else
                {
                    this.tileOffsetField = value;
                }
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("image", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public mapTilesetImage[] image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                if (this.imageField != null && this.imageField.Length > 0)
                {
                    return;
                }
                else
                {
                    this.imageField = value;
                }
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("tile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public List<mapTilesetTile> tile = new List<mapTilesetTile>();
        //{
        //    get
        //    {
        //        return this.tileField;
        //    }
        //    set
        //    {
        //        if (this.tileField != null && this.tileField.Length > 0)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            this.tileField = value;
        //        }
        //    }
        //}

        private IDictionary<uint, mapTilesetTile> tileDictionaryField = null;

        [XmlIgnore]
        public IDictionary<uint, mapTilesetTile> tileDictionary
        {
            get
            {
                lock (this)
                {
                    if (tileDictionaryField == null)
                    {
                        tileDictionaryField = new ConcurrentDictionary<uint, mapTilesetTile>();

                        if (tile != null)
                        {

                            try
                            {
                                //Parallel.ForEach(tile, (t) =>
                                //            {
                                //                if (t != null && !tileDictionaryField.ContainsKey((uint)t.id + 1))
                                //                {
                                //                    tileDictionaryField.Add((uint)t.id + 1, t);
                                //                }
                                //            });

                                foreach (var t in tile)
                                {
                                    uint key = (uint)t.id + 1;
                                    if (t != null && !tileDictionaryField.ContainsKey(key))
                                    {
                                        tileDictionaryField.Add(key, t);
                                    }
                                }


                            }
                            catch (AggregateException)
                            {

                                throw;
                            }
                        }

                        return tileDictionaryField;

                    }
                    else
                    {
                        return tileDictionaryField;
                    }
                }

            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint firstgid
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int tilewidth
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int tileheight
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int spacing
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int margin
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}
