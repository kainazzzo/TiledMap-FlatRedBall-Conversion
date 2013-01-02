﻿using System.Linq;
using FlatRedBall.Content.Scene;
using TMXGlueLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FlatRedBall.Content.Math.Geometry;
using FlatRedBall.Content;
using FlatRedBall.Content.AI.Pathfinding;

namespace TMXGlueTest
{


    /// <summary>
    ///This is a test class for TiledMapSaveTest and is intended
    ///to contain all TiledMapSaveTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TiledMapSaveTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion


        /// <summary>
        ///A test for ToCSVString
        ///</summary>
        [TestMethod()]
        public void ToCSVStringLayerPropertyTest()
        {
            const TiledMapSave.CSVPropertyType type = TiledMapSave.CSVPropertyType.Layer;
            var target = new TiledMapSave
            {
                layer = new mapLayer[] {new mapLayer()
                {
                    properties = new List<property>
                    {
                        new property() {name = "name1", value = "val"},
                        new property() {name = "name2", value = "val2"}
                    },
                    name="layer1"
                }, new mapLayer()
                    {
                        properties = new List<property>
                        {
                            new property() {name = "name1", value = "val"},
                            new property() {name = "name3", value = "val3"}
                        },
                        name="layer2"
                    }}
            };

            var expected =
                "Name (required),name2,name1,name3\r\nlayer1,\"val2\",\"val\",\r\nlayer2,,\"val\",\"val3\"\r\n";
            // TODO: Initialize to an appropriate value
            var actual = target.ToCSVString(type, null);
            Assert.AreEqual(expected, actual);

            string layerName = "layer1";
            expected = "Name (required),name2,name1\r\nlayer1,\"val2\",\"val\"\r\n";
            actual = target.ToCSVString(type, layerName);
            Assert.AreEqual(expected, actual);


        }

        /// <summary>
        ///A test for ToCSVString
        ///</summary>
        [TestMethod()]
        public void ToCSVStringTilesetPropertyTest()
        {
            var target = new TiledMapSave
                {
                    tileset = new mapTileset[2]
                        {
                            new mapTileset
                                {
                                    Firstgid = 1,
                                    Name = "tileset1",
                                    Tile = new List<mapTilesetTile>
                                        {
                                            new mapTilesetTile
                                                {
                                                    id = 1,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "commonPropertyName",
                                                                    value = "commonPropertyValueTileset1Tile1"
                                                                },
                                                            new property
                                                                {
                                                                    name = "tileset1Tile1PropertyName",
                                                                    value = "tileset1Tile1PropertyValue"
                                                                },
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tileset1Tile1"
                                                                }
                                                        }
                                                },
                                            new mapTilesetTile
                                                {
                                                    id = 2,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tileset1Tile2"
                                                                },
                                                            new property
                                                                {
                                                                    name = "commonPropertyName",
                                                                    value = "commonPropertyValueTileset1Tile2"
                                                                },
                                                            new property
                                                                {
                                                                    name = "tileset1Tile2PropertyName",
                                                                    value = "tileset1Tile2PropertyValue"
                                                                }
                                                        }
                                                },
                                        },
                                },
                            new mapTileset
                                {
                                    Firstgid = 3,
                                    Name = "tileset2",
                                    Tile = new List<mapTilesetTile>
                                        {
                                            new mapTilesetTile
                                                {
                                                    id = 1,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tileset2Tile1"
                                                                },
                                                            new property
                                                                {
                                                                    name = "commonPropertyName",
                                                                    value = "commonPropertyValueTileset2Tile1"
                                                                },
                                                            new property
                                                                {
                                                                    name = "tileset2Tile1PropertyName",
                                                                    value = "tileset2Tile1PropertyValue"
                                                                }
                                                        }
                                                }
                                        }
                                }
                        },
                    layer = new mapLayer[1]
                        {
                            new mapLayer
                                {
                                    height = 32,
                                    width = 96,
                                    name = "layer1",
                                    visible = 1,
                                    data = new mapLayerData[1]
                                        {
                                            new mapLayerData
                                                {
                                                    dataTiles = new mapLayerDataTile[3]
                                                        {
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "1"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "2"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "3"
                                                                }
                                                        }
                                                }
                                        },
                                }
                        },
                    orientation = "Orthogonal",
                    tileheight = 32,
                    tilewidth = 32
                };

            string expected =
                "Name (required),tileset1Tile1PropertyName,commonPropertyName,tileset1Tile2PropertyName,tileset2Tile1PropertyName\r\ntileset1Tile1,\"tileset1Tile1PropertyValue\",\"commonPropertyValueTileset1Tile1\",,\r\ntileset1Tile2,,\"commonPropertyValueTileset1Tile2\",\"tileset1Tile2PropertyValue\",\r\ntileset2Tile1,,\"commonPropertyValueTileset2Tile1\",,\"tileset2Tile1PropertyValue\"\r\n";
            string actual = target.ToCSVString(type: TiledMapSave.CSVPropertyType.Tile, layerName: null);
            Assert.AreEqual(expected, actual);

            expected =
                "Name (required),tileset1Tile1PropertyName,commonPropertyName,tileset1Tile2PropertyName,tileset2Tile1PropertyName\r\ntileset1Tile1,\"tileset1Tile1PropertyValue\",\"commonPropertyValueTileset1Tile1\",,\r\ntileset1Tile2,,\"commonPropertyValueTileset1Tile2\",\"tileset1Tile2PropertyValue\",\r\ntileset2Tile1,,\"commonPropertyValueTileset2Tile1\",,\"tileset2Tile1PropertyValue\"\r\n";
        }

        [TestMethod()]
        public void ToCSVStringMapPropertyTest()
        {
            var target = new TiledMapSave
                {
                    properties = new List<property>
                        {
                            new property
                                {
                                    name = "mapProperty1",
                                    value = "mapValue1"
                                },
                            new property
                                {
                                    name = "mapProperty2",
                                    value = "mapValue2"
                                }
                        }
                };
            string actual = target.ToCSVString(TiledMapSave.CSVPropertyType.Map);
            const string expected = "Name (required),mapProperty2,mapProperty1\r\nmap,\"mapValue2\",\"mapValue1\"\r\n";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ToCSVStringObjectPropertyTest()
        {
            var target = new TiledMapSave
            {
                objectgroup = new mapObjectgroup[2]
                    {
                        new mapObjectgroup
                            {
                                name = "objectGroup1",
                                @object= new mapObjectgroupObject[1]
                                    {
                                        new mapObjectgroupObject
                                            {
                                                Name="object1",
                                                properties = new List<property>
                                                    {
                                                        new property
                                                            {
                                                                name="prop1",
                                                                value = "val1"
                                                            },
                                                        new property
                                                            {
                                                                name="prop2",
                                                                value = "val2"
                                                            }
                                                    }
                                            }
                                    }
                            },
                        new mapObjectgroup
                            {
                                    @object=new mapObjectgroupObject[1]
                                        {
                                            new mapObjectgroupObject
                                                {
                                                    Name = "object2",
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "prop3",
                                                                    value="val3"
                                                                },
                                                            new property
                                                                {
                                                                    name="prop2",
                                                                    value = "val4"
                                                                }
                                                        }
                                                }
                                        }
                            }
                    }
            };
            string actual = target.ToCSVString(TiledMapSave.CSVPropertyType.Object);
            const string expected =
                "Name (required),prop1,prop2,prop3\r\nobject1,\"val1\",\"val2\",\r\nobject2,,\"val4\",\"val3\"\r\n";

            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for BuildPropertyDictionaryConcurrently
        ///</summary>
        [TestMethod()]
        public void BuildPropertyDictionaryConcurrentlyTest()
        {


            var properties = new List<property>();

            for (var x = 0; x < 10000; ++x)
            {
                properties.Add(new property
                    {
                        name = string.Format("name{0}", x),
                        value = string.Format("val{0}", x)
                    });
            }
            IDictionary<string, string> actual = TiledMapSave.BuildPropertyDictionaryConcurrently(properties);

            CollectionAssert.AreEqual(properties.Select(p => p.name).OrderBy(n => n).ToList(), actual.Keys.OrderBy(k => k).ToList());
            CollectionAssert.AreEqual(properties.Select(p => p.value).OrderBy(v => v).ToList(), actual.Values.OrderBy(v => v).ToList());
        }

        /// <summary>
        ///A test for ToShapeCollectionSave
        ///</summary>
        [TestMethod()]
        public void ToShapeCollectionSaveTest()
        {
            var map = new TiledMapSave
                {
                    height = 3,
                    width = 2,
                    tileheight = 32,
                    tilewidth = 32,
                    orientation = "orthogonal",
                    tileset = new mapTileset[]
                        {
                            new mapTileset
                                {
                                    Firstgid = 1u,
                                    Name = "test",
                                    Tilewidth = 32,
                                    Tileheight = 32,
                                    Spacing = 1,
                                    Margin = 1,
                                    Image = new mapTilesetImage[]
                                        {
                                            new mapTilesetImage
                                                {
                                                    height = 199,
                                                    width = 199,
                                                    source = "nothing"
                                                }
                                        },
                                }
                        },
                    layer = new mapLayer[]
                        {
                            new mapLayer
                                {
                                    name = "Layer1",
                                    width = 2,
                                    height = 3,
                                    data = new mapLayerData[]
                                        {
                                            new mapLayerData
                                                {
                                                    dataTiles = new mapLayerDataTile[]
                                                        {
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "1"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "3"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "9"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "11"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "17"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "19"
                                                                }
                                                        }
                                                }
                                        }
                                }
                        },
                    objectgroup = new mapObjectgroup[]
                        {
                            new mapObjectgroup
                                {
                                    name = "Objects",
                                    @object = new mapObjectgroupObject[]
                                        {
                                            new mapObjectgroupObject
                                                {
                                                    x = 0,
                                                    y = 0,
                                                    width = 64,
                                                    height = 32
                                                },
                                            new mapObjectgroupObject
                                                {
                                                    x = 9,
                                                    y = 45,
                                                    polygon = new mapObjectgroupObjectPolygon[]
                                                        {
                                                            new mapObjectgroupObjectPolygon
                                                                {
                                                                    points = "0,0 42,0 23,23"
                                                                }
                                                        }
                                                },
                                            new mapObjectgroupObject
                                                {
                                                    x = 6,
                                                    y = 66,
                                                    polyline = new mapObjectgroupObjectPolyline[]
                                                        {
                                                            new mapObjectgroupObjectPolyline
                                                                {
                                                                    points = "0,0 7,19 42,19 52,-1"
                                                                }
                                                        }
                                                },
                                            new mapObjectgroupObject
                                                {
                                                    x = 8,
                                                    y = 13,
                                                    width = 14,
                                                    height = 12
                                                },
                                            new mapObjectgroupObject
                                                {
                                                    x = 38,
                                                    y = 14,
                                                    width = 17,
                                                    height = 12
                                                }
                                        }
                                }
                        }
                };
            var shapeCollectionSave = map.ToShapeCollectionSave(null);

            Assert.AreEqual(5, shapeCollectionSave.PolygonSaves.Count);

            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(0).X);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(0).Y);
            Assert.AreEqual(0, shapeCollectionSave.PolygonSaves.ElementAt(0).Z);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.First().X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.First().Y);
            Assert.AreEqual(80, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(1).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(1).Y);
            Assert.AreEqual(80, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(2).X);
            Assert.AreEqual(-48, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(2).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(3).X);
            Assert.AreEqual(-48, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(3).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(4).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(0).Points.ElementAt(4).Y);

            Assert.AreEqual(-7, shapeCollectionSave.PolygonSaves.ElementAt(1).X);
            Assert.AreEqual(-29, shapeCollectionSave.PolygonSaves.ElementAt(1).Y);
            Assert.AreEqual(0, shapeCollectionSave.PolygonSaves.ElementAt(1).Z);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.First().X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.First().Y);
            Assert.AreEqual(58, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.ElementAt(1).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.ElementAt(1).Y);
            Assert.AreEqual(39, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.ElementAt(2).X);
            Assert.AreEqual(-39, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.ElementAt(2).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.ElementAt(3).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(1).Points.ElementAt(3).Y);

            Assert.AreEqual(-10, shapeCollectionSave.PolygonSaves.ElementAt(2).X);
            Assert.AreEqual(-50, shapeCollectionSave.PolygonSaves.ElementAt(2).Y);
            Assert.AreEqual(0, shapeCollectionSave.PolygonSaves.ElementAt(2).Z);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.First().X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.First().Y);
            Assert.AreEqual(23, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.ElementAt(1).X);
            Assert.AreEqual(-35, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.ElementAt(1).Y);
            Assert.AreEqual(58, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.ElementAt(2).X);
            Assert.AreEqual(-35, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.ElementAt(2).Y);
            Assert.AreEqual(68, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.ElementAt(3).X);
            Assert.AreEqual(-15, shapeCollectionSave.PolygonSaves.ElementAt(2).Points.ElementAt(3).Y);

            Assert.AreEqual(-8, shapeCollectionSave.PolygonSaves.ElementAt(3).X);
            Assert.AreEqual(3, shapeCollectionSave.PolygonSaves.ElementAt(3).Y);
            Assert.AreEqual(0, shapeCollectionSave.PolygonSaves.ElementAt(3).Z);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(0).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(0).Y);
            Assert.AreEqual(30, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(1).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(1).Y);
            Assert.AreEqual(30, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(2).X);
            Assert.AreEqual(-28, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(2).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(3).X);
            Assert.AreEqual(-28, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(3).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(4).X);
            Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(3).Points.ElementAt(4).Y);

            Assert.AreEqual(22, shapeCollectionSave.PolygonSaves.ElementAt(4).X);
           Assert.AreEqual(2, shapeCollectionSave.PolygonSaves.ElementAt(4).Y);
             Assert.AreEqual(0, shapeCollectionSave.PolygonSaves.ElementAt(4).Z);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(0).X);
           Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(0).Y);
            Assert.AreEqual(33, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(1).X);
           Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(1).Y);
            Assert.AreEqual(33, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(2).X);
           Assert.AreEqual(-28, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(2).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(3).X);
           Assert.AreEqual(-28, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(3).Y);
            Assert.AreEqual(16, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(4).X);
           Assert.AreEqual(-16, shapeCollectionSave.PolygonSaves.ElementAt(4).Points.ElementAt(4).Y);

        }

        /// <summary>
        ///A test for ToSceneSave
        ///</summary>
        [TestMethod()]
        public void ToSpriteEditorSceneTest()
        {
            TiledMapSave.Offset = new Tuple<float, float, float>(0f, 0f, 0f);
            var target = new TiledMapSave()
                {
                    height = 64,
                    width = 64,
                    tileheight = 32,
                    tilewidth = 32,
                    tileset = new mapTileset[]
                        {
                            new mapTileset
                                {
                                    Firstgid = 1,
                                    Image = new mapTilesetImage[]
                                        {
                                            new mapTilesetImage
                                                {
                                                    height = 64,
                                                    width = 64,
                                                    source =
                                                        "../../../../../Program Files (x86)/Tiled/examples/tmw_desert_spacing.png"
                                                }
                                        },
                                    Name = "tileset1",
                                    Tile = new List<mapTilesetTile>
                                        {
                                            new mapTilesetTile
                                                {
                                                    id = 0,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tile1"
                                                                }
                                                        }
                                                },
                                            new mapTilesetTile
                                                {
                                                    id = 1,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tile2"
                                                                }
                                                        }
                                                },
                                            new mapTilesetTile
                                                {
                                                    id = 2,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tile3"
                                                                }
                                                        }
                                                },
                                                new mapTilesetTile
                                                {
                                                    id = 3,
                                                    properties = new List<property>
                                                        {
                                                            new property
                                                                {
                                                                    name = "name",
                                                                    value = "tile4"
                                                                }
                                                        }
                                                }

                                        },
                                    Tileheight = 32,
                                    Tilewidth = 32
                                }
                        },
                    layer = new mapLayer[]
                        {
                            new mapLayer
                                {
                                    name = "layer1",
                                    data = new mapLayerData[]
                                        {
                                            new mapLayerData()
                                                {
                                                    dataTiles = new mapLayerDataTile[]
                                                        {
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "1"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "3"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "4"
                                                                },
                                                            new mapLayerDataTile
                                                                {
                                                                    gid = "2"
                                                                },
                                                        }
                                                }
                                        }
                                }
                        },
                    orientation = "orthogonal"
                };
            const float scale = 1F;
            SceneSave actual = target.ToSceneSave(scale);

            Assert.AreEqual(4, actual.SpriteList.Count);

            var tile = actual.SpriteList.SingleOrDefault(s => s.Name == "tile1");
            Assert.IsNotNull(tile);
            Assert.AreEqual(16f, tile.X);
            Assert.AreEqual(-16, tile.Y);

            tile = actual.SpriteList.SingleOrDefault(s => s.Name == "tile2");
            Assert.IsNotNull(tile);
            Assert.AreEqual(112f, tile.X);
            Assert.AreEqual(-16f, tile.Y);

            tile = actual.SpriteList.SingleOrDefault(s => s.Name == "tile3");
            Assert.IsNotNull(tile);
            Assert.AreEqual(48f, tile.X);
            Assert.AreEqual(-16f, tile.Y);

            tile = actual.SpriteList.SingleOrDefault(s => s.Name == "tile4");
            Assert.IsNotNull(tile);
            Assert.AreEqual(80f, tile.X);
            Assert.AreEqual(-16f, tile.Y);
        }

        /// <summary>
        ///A test for ToNodeNetworkSave
        ///</summary>
        [TestMethod()]
        public void ToNodeNetworkSaveTest()
        {
            TiledMapSave target = new TiledMapSave(); // TODO: Initialize to an appropriate value
            bool linkHorizontally = false; // TODO: Initialize to an appropriate value
            bool linkVertically = false; // TODO: Initialize to an appropriate value
            bool linkDiagonally = false; // TODO: Initialize to an appropriate value
            bool requireTile = false; // TODO: Initialize to an appropriate value
            NodeNetworkSave expected = null; // TODO: Initialize to an appropriate value
            NodeNetworkSave actual;
            actual = target.ToNodeNetworkSave(linkHorizontally, linkVertically, linkDiagonally, requireTile);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ToNodeNetworkSave
        ///</summary>
        [TestMethod()]
        public void ToNodeNetworkSaveTest1()
        {
            TiledMapSave target = new TiledMapSave(); // TODO: Initialize to an appropriate value
            bool requireTile = false; // TODO: Initialize to an appropriate value
            NodeNetworkSave expected = null; // TODO: Initialize to an appropriate value
            NodeNetworkSave actual;
            actual = target.ToNodeNetworkSave(requireTile);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CalculateWorldCoordinates
        ///</summary>
        [TestMethod()]
        public void CalculateWorldCoordinatesTest()
        {
            //TiledMapSave target = new TiledMapSave(); // TODO: Initialize to an appropriate value
            //int layercount = 0; // TODO: Initialize to an appropriate value
            //int count = 0; // TODO: Initialize to an appropriate value
            //int tileWidth = 0; // TODO: Initialize to an appropriate value
            //int tileHeight = 0; // TODO: Initialize to an appropriate value
            //int layerWidth = 0; // TODO: Initialize to an appropriate value
            //float x = 0F; // TODO: Initialize to an appropriate value
            //float xExpected = 0F; // TODO: Initialize to an appropriate value
            //float y = 0F; // TODO: Initialize to an appropriate value
            //float yExpected = 0F; // TODO: Initialize to an appropriate value
            //float z = 0F; // TODO: Initialize to an appropriate value
            //float zExpected = 0F; // TODO: Initialize to an appropriate value
            //target.CalculateWorldCoordinates(layercount, count, tileWidth, tileHeight, layerWidth, out x, out y, out z);
            //Assert.AreEqual(xExpected, x);
            //Assert.AreEqual(yExpected, y);
            //Assert.AreEqual(zExpected, z);
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
