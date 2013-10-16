using System.Collections.Generic;
using System.Xml.Serialization;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using FlatRedBall.IO;

namespace TMXGlueLib
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
// ReSharper disable InconsistentNaming
    public class mapTileset
// ReSharper restore InconsistentNaming
    {
        private mapTilesetImage[] _imageField;

        private mapTilesetTileOffset[] _tileOffsetField;

        private string _sourceField;

        [XmlIgnore]
        public string SourceDirectory
        {
            get
            {
                if (_sourceField != null && _sourceField.Contains("\\"))
                {
                    return _sourceField.Substring(0, _sourceField.LastIndexOf('\\'));
                }
                else
                {
                    return ".";
                }
            }
        }

        [XmlAttribute("source", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Source
        {
            get
            {
                return _sourceField;
            }
            set
            {
                this._sourceField = value;
                if (this._sourceField != null)
                {
                    _sourceField = _sourceField.Replace("/", "\\");
                    var xts = FileManager.XmlDeserialize<tileset>(_sourceField);
                    Image = new mapTilesetImage[xts.image.Length];

                    Parallel.For(0, xts.image.Length, count =>
                        {
                            this.Image[count] = new mapTilesetImage
                                {
                                    source = xts.image[count].source,
                                    height = xts.image[count].height != 0 ? xts.image[count].height : xts.tileheight,
                                    width = xts.image[count].width != 0 ? xts.image[count].width : xts.tilewidth
                                };
                        });

                    this.Name = xts.name;
                    this.Margin = xts.margin;
                    this.Spacing = xts.spacing;
                    this.Tileheight = xts.tileheight;
                    this.Tilewidth = xts.tilewidth;
                    this.Tiles = xts.tile;
                }
            }
        }

        /// <remarks/>
        [XmlElement("tileoffset", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public mapTilesetTileOffset[] Tileoffset
        {
            get
            {
                return this._tileOffsetField;
            }
            set
            {
                if (this._tileOffsetField == null || this._tileOffsetField.Length == 0)
                {
                    this._tileOffsetField = value;
                }
            }
        }


        /// <remarks/>
        [XmlElement("image", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public mapTilesetImage[] Image
        {
            get
            {
                return this._imageField;
            }
            set
            {
                if (this._imageField == null || this._imageField.Length == 0)
                {
                    this._imageField = value;
                }
            }
        }

        [XmlArray("terraintypes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public List<mapTilesetTerrain> terraintypes = new List<mapTilesetTerrain>();

        [XmlElement("tile", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public List<mapTilesetTile> Tiles = new List<mapTilesetTile>();
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

        public void RefreshTileDictionary()
        {
            _tileDictionaryField = null;
        }


        private IDictionary<uint, mapTilesetTile> _tileDictionaryField;

        [XmlIgnore]
        public IDictionary<uint, mapTilesetTile> TileDictionary
        {
            get
            {
                lock (this)
                {
                    if (_tileDictionaryField == null)
                    {
                        _tileDictionaryField = new ConcurrentDictionary<uint, mapTilesetTile>();

                        if (Tiles != null)
                        {
                            //Parallel.ForEach(tile, (t) =>
                            //            {
                            //                if (t != null && !tileDictionaryField.ContainsKey((uint)t.id + 1))
                            //                {
                            //                    tileDictionaryField.Add((uint)t.id + 1, t);
                            //                }
                            //            });

                            foreach (var t in Tiles)
                            {
                                uint key = (uint)t.id + 1;
                                if (!_tileDictionaryField.ContainsKey(key))
                                {
                                    _tileDictionaryField.Add(key, t);
                                }
                            }
                        }

                        return _tileDictionaryField;

                    }
                    else
                    {
                        return _tileDictionaryField;
                    }
                }

            }
        }



        /// <remarks/>
        [XmlAttribute("firstgid")]
        public uint Firstgid
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("tilewidth")]
        public int Tilewidth
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("tileheight")]
        public int Tileheight
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("spacing")]
        public int Spacing
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("margin")]
        public int Margin
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class mapTilesetTerrain
    {
        public string name { get; set; }
        public int tile { get; set; }
    }
}
