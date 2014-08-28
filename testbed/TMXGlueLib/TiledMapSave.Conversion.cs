using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Threading.Tasks;
using FlatRedBall;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Content;
using FlatRedBall.Content.AI.Pathfinding;
using FlatRedBall.Content.Math.Geometry;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Content.Scene;
using FlatRedBall.IO;
using FlatRedBall.Math.Geometry;

namespace TMXGlueLib
{
    #region FileReferenceType enum
    public enum FileReferenceType
    {
        NoDirectory,
        Absolute,
        Relative
    }
    #endregion

    public partial class TiledMapSave
    {
        #region Enums

        public enum CSVPropertyType { Tile, Layer, Map, Object };

        enum LessOrGreaterDesired
        {
            Less,
            Greater,
            NoChange
        }
        #endregion

        #region Fields

        public static LayerVisibleBehavior LayerVisibleBehaviorValue = LayerVisibleBehavior.Ignore;
        public static int MaxDegreeOfParallelism = 1;



        private static Tuple<float, float, float> _offset = new Tuple<float, float, float>(0f, 0f, 0f);

        #endregion

        #region Properties

        public static Tuple<float, float, float> Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        #endregion

        public Scene ToScene(string contentManagerName, float scale)
        {
            var scene = ToSceneSave(scale);
            return scene.ToScene(contentManagerName);
        }

        public ShapeCollection ToShapeCollection(string layerName=null)
        {
            var scs = ToShapeCollectionSave(layerName);

            return scs.ToShapeCollection();
        }

        public string ToCSVString(CSVPropertyType type = CSVPropertyType.Tile, string layerName = null)
        {
            var sb = new StringBuilder();
            IEnumerable<string> columnNames = GetColumnNames(type, layerName);
            var enumerable = columnNames as IList<string> ?? columnNames.ToList();
            WriteColumnHeader(sb, enumerable);
            WriteColumnValues(sb, enumerable, type, layerName);

            return sb.ToString();
        }

        private void WriteColumnValues(StringBuilder sb, IList<string> columnNames, CSVPropertyType type, string layerName)
        {
            columnNames = columnNames.Select(item => property.GetStrippedName(item)).ToList();


            // TODO: There is probably a good way to refactor this code
            switch (type)
            {
                case CSVPropertyType.Tile:
                    foreach (Tileset tileSet in this.Tilesets)
                    {
                        if (tileSet.Tiles != null)
                        {
                            foreach (mapTilesetTile tile in tileSet.Tiles.Where(t=>t.PropertyDictionary.Count > 0))
                            {
                                var propertyDictionary = tile.PropertyDictionary;

                                WriteValuesFromDictionary(sb, null, propertyDictionary, 0, 0, columnNames);
                            }
                        }
                    }
                    break;
                case CSVPropertyType.Layer:

                    this.Layers.Where(
                        l =>
                        layerName == null ||
                        (l.Name != null && l.Name.Equals(layerName, StringComparison.OrdinalIgnoreCase))).ToList()
                        .ForEach(l => WriteValuesFromDictionary(sb, null, l.PropertyDictionary, 0, 0, columnNames));
                    break;
                case CSVPropertyType.Map:
                    WriteValuesFromDictionary(sb, null, PropertyDictionary, 0, 0, columnNames);
                    break;
                case CSVPropertyType.Object:
                    this.objectgroup.Where(
                        og =>
                        layerName == null ||
                        (og.name != null && og.name.Equals(layerName, StringComparison.OrdinalIgnoreCase)))
                        .SelectMany(o => o.@object, (o, c) => new { group = o, obj = c, X = c.x, Y = c.y })
                        .ToList()
                        .ForEach(o => WriteValuesFromDictionary(sb, o.group.PropertyDictionary, o.obj.PropertyDictionary, o.X, o.Y, columnNames));
                    break;
            }
        }

        static int numberOfUnnamedTiles = 0;

        private void WriteValuesFromDictionary(StringBuilder sb, IDictionary<string, string> pDictionary, IDictionary<string, string> iDictionary, int x, int y, IEnumerable<string> columnNames)
        {
            // January 26, 2014
            // I spent quite a while
            // trying to figure out why
            // my TMX wasn't making a CSV
            // properly, but then I should
            // comment it.  The general idea
            // is that each tile will be represented
            // as a row in the CSV.  The CSV will be deserialized
            // to a dictionary, and the Name value is the Key.  Therefore
            // each tile must have a Name value so that it can be referenced
            // in the resulting dictionary.  If there is no Name value, then we're 
            // going to skip the tile.
            // Update January 26, 2014
            // I got confused about this
            // and was able to solve it by
            // digging into the source, but
            // others won't have this benefit,
            // and those who do know this may forget
            // (as I did).  So let's make this easier
            // to work with by generating a name if one
            // doesn't exist.  This makes it much easier
            // to figure out the intended usage of this plugin.
            //if (doesDictionaryContainNameValue)
            //{

            string nameValue = null;

            bool doesDictionaryContainNameValue = 
                iDictionary.Any(p => property.GetStrippedName(p.Key).Equals("name", StringComparison.CurrentCultureIgnoreCase));

            if (doesDictionaryContainNameValue)
            {
                nameValue = iDictionary.First(p => property.GetStrippedName(p.Key).Equals("name", StringComparison.CurrentCultureIgnoreCase)).Value;
            }
            else
            {
                nameValue = "UnnamedTile" + numberOfUnnamedTiles;
                numberOfUnnamedTiles++;
            }

            sb.Append(nameValue);

            foreach (string columnName in columnNames)
            {
                string strippedColumnName = property.GetStrippedName(columnName);


                if (!strippedColumnName.Equals("name", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (iDictionary.Any(p => property.GetStrippedName(p.Key).Equals(strippedColumnName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var value =
                            iDictionary.First(p => property.GetStrippedName(p.Key).Equals(strippedColumnName, StringComparison.CurrentCultureIgnoreCase)).Value;

                        if (value != null)
                        {
                            value = value.Replace("\"", "\"\"");
                        }

                        sb.AppendFormat(",\"{0}\"", value);
                    }
                    else if (pDictionary != null && pDictionary.Any(p => p.Key.Equals(strippedColumnName, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        var value =
                            pDictionary.First(p => p.Key.Equals(strippedColumnName, StringComparison.CurrentCultureIgnoreCase)).Value;

                        if (value != null)
                        {
                            value = value.Replace("\"", "\"\"");
                        }

                        sb.AppendFormat(",\"{0}\"", value);
                    }
                    else
                    {
                        sb.Append(",");
                    }
                }
            }
            sb.AppendLine();
        }

        private static void WriteColumnHeader(StringBuilder sb, IEnumerable<string> columnNames)
        {
            sb.Append("Name (required)");
            foreach (string columnName in columnNames)
            {
                string strippedName = property.GetStrippedName(columnName);


                bool isName = strippedName.Equals("name", StringComparison.CurrentCultureIgnoreCase);

                if (!isName)
                {
                    // Update August 27, 2012
                    // We can't just assume that
                    // all of the column names are
                    // going to be capitalized.  This
                    // was likely done to force the Name
                    // property to be capitalized, which we
                    // want, but we don't want to do it for everything.
                    //if (columnName.Length > 1)
                    //{
                    //    sb.AppendFormat(",{0}{1}", columnName.Substring(0, 1).ToUpper(), columnName.Substring(1));
                    //}
                    //else
                    //{
                    //    sb.AppendFormat(",{0}", columnName.ToUpper());
                    //}
                    sb.Append("," + columnName);
                }
            }
            sb.AppendLine();
        }

        /// <summary>
        /// Sets all Image width/height values inside all Tilesets according to actual
        /// texture sizes.  The reason this is needed is because the user could have chnaged
        /// the size of the images (like resized a PNG) and not opened/resaved the file in Tiled.
        /// Therefore, the image size may be incorrect in the TMX.
        /// </summary>
        /// <param name="directory">The directory of the TMX, which is used to load images which typically use relative paths.</param>
        public void CorrectImageSizes(string directory)
        {
            foreach (var tileset in this.Tilesets)
            {
                foreach (var image in tileset.Images)
                {
                    string absolutePath = image.Source;

                    if (FileManager.IsRelative(absolutePath))
                    {
                        absolutePath = directory + absolutePath;
                    }

                    if (System.IO.File.Exists(absolutePath))
                    {
                        var dimensions = ImageHeader.GetDimensions(absolutePath);

                        image.width = dimensions.Width;
                        image.height = dimensions.Height;
                    }
                }
            }
        }

        /// <summary>
        /// Compares the stripped name of properties - removing the type
        /// </summary>
        public class CaseInsensitivePropertyEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {

                return property.GetStrippedName(x).Equals(property.GetStrippedName(y), StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return property.GetStrippedName(obj).ToLowerInvariant().GetHashCode();
            }
        }

        private IEnumerable<string> GetColumnNames(CSVPropertyType type, string layerName)
        {
            var comparer = new CaseInsensitivePropertyEqualityComparer();

            var columnNames = new HashSet<string>();

            switch (type)
            {
                case CSVPropertyType.Tile:
                    return
                        this.Tilesets.SelectMany(t => t.Tiles)
                            .SelectMany(tile => tile.PropertyDictionary)
                            .Select(d => d.Key)
                            .Distinct(comparer);
                case CSVPropertyType.Layer:
                    return
                        this.Layers.Where(
                            l =>
                            layerName == null ||
                            (l.Name != null && l.Name.Equals(layerName, StringComparison.OrdinalIgnoreCase)))
                            .SelectMany(l => l.PropertyDictionary)
                            .Select(d => d.Key)
                            .Distinct(comparer);
                case CSVPropertyType.Map:
                    return this.PropertyDictionary.Select(d => d.Key).Distinct(comparer);
                case CSVPropertyType.Object:
                    return ((new[] {"X (int)", "Y (int)"}.Select(d => d)
                        .Union(objectgroup.Where(l =>
                                                 layerName == null ||
                                                 (l.name != null &&
                                                  l.name.Equals(layerName, StringComparison.OrdinalIgnoreCase)))
                                   .SelectMany(o => o.@object)
                                   .SelectMany(o => o.PropertyDictionary)
                                   .Select(d => d.Key), comparer)
                        .Union(objectgroup.Where(l =>
                                                 layerName == null ||
                                                 (l.name != null &&
                                                  l.name.Equals(layerName, StringComparison.OrdinalIgnoreCase)))
                                   .SelectMany(o => o.PropertyDictionary)
                                   .Select(d => d.Key), comparer)));
            }
            return columnNames;
        }

        public ShapeCollectionSave ToShapeCollectionSave(string layerName)
        {
            MapLayer mapLayer = null;
            if (!string.IsNullOrEmpty(layerName))
            {
                mapLayer = this.Layers.FirstOrDefault(l => l.Name.Equals(layerName));
            }
            var shapes = new ShapeCollectionSave();

            if ((mapLayer != null && !mapLayer.IsVisible && mapLayer.VisibleBehavior == LayerVisibleBehavior.Skip) ||
                this.objectgroup == null || this.objectgroup.Length == 0)
            {
                return shapes;
            }

            foreach (mapObjectgroup group in this.objectgroup)
            {
                if (group.@object != null && !string.IsNullOrEmpty(group.name) && (string.IsNullOrEmpty(layerName) || group.name.Equals(layerName)))
                {
                    foreach (mapObjectgroupObject @object in group.@object)
                    {
                        if (@object.polygon != null)
                        {
                            foreach (mapObjectgroupObjectPolygon polygon in @object.polygon)
                            {
                                // TODO: Make this a rectangle object
                                PolygonSave p = ConvertTmxObjectToFrbPolygonSave(@object.Name, @group.width, @group.height,
                                    @object.x, @object.y, polygon.points, true);
                                if (p != null)
                                {
                                    shapes.PolygonSaves.Add(p);
                                }
                            }
                        }

                        if (@object.polyline != null)
                        {
                            foreach (mapObjectgroupObjectPolyline polyline in @object.polyline)
                            {
                                PolygonSave p = ConvertTmxObjectToFrbPolygonSave(@object.Name, @group.width, @group.height,
                                    @object.x, @object.y, polyline.points, false);
                                if (p != null)
                                {
                                    shapes.PolygonSaves.Add(p);
                                }
                            }
                        }

                        if (@object.polygon == null && @object.polyline == null)
                        {
                            PolygonSave p = ConvertTmxObjectToFrbPolygonSave(@object.Name, @group.width, @group.height, @object.x, @object.y, @object.width, @object.height);
                            if (p != null)
                            {
                                shapes.PolygonSaves.Add(p);
                            }
                        }
                    }
                }
            }
            return shapes;
        }

        private PolygonSave ConvertTmxObjectToFrbPolygonSave(string name, int groupWidth, int groupHeight, int x, int y, int w, int h)
        {
            var pointsSb = new StringBuilder();

            pointsSb.Append("0,0");

            pointsSb.AppendFormat(" {0},{1}", w, 0);
            pointsSb.AppendFormat(" {0},{1}", w, h);
            pointsSb.AppendFormat(" {0},{1}", 0, h);


            return ConvertTmxObjectToFrbPolygonSave(name, groupWidth, groupHeight, x, y, pointsSb.ToString(), true);
        }

        private PolygonSave ConvertTmxObjectToFrbPolygonSave(string name, int w, int h, int x, int y, string points, bool connectBackToStart)
        {
            if (string.IsNullOrEmpty(points))
            {
                return null;
            }
            var polygon = new PolygonSave();
            string[] pointString = points.Split(" ".ToCharArray());
            float z;
            float newx;
            float newy;
            float fx = x;
            float fy = y;

            polygon.Name = name;

            if ("orthogonal".Equals(this.orientation))
            {
                fx -= tilewidth / 2.0f;
                fy -= tileheight + (tileheight / 2.0f);
            }
            else if ("isometric".Equals(this.orientation))
            {
                fx -= tilewidth / 4.0f;
                fy -= tileheight / 2.0f;
            }

            CalculateWorldCoordinates(0, fx / tileheight, fy / tileheight, this.tilewidth, this.tileheight, w * tilewidth, out newx, out newy, out z);
            polygon.X = newx - tilewidth / 2.0f;
            polygon.Y = newy - tileheight / 2.0f;
            var pointsArr = new Point[pointString.Length + (connectBackToStart ? 1 : 0)];

            int count = 0;
            foreach (string pointStr in pointString)
            {
                string[] xy = pointStr.Split(",".ToCharArray());
                int relativeX = Convert.ToInt32(xy[0]);
                int relativeY = Convert.ToInt32(xy[1]);

                float normalizedX = relativeX / (float)tileheight;
                float normalizedY = relativeY / (float)tileheight;

                CalculateWorldCoordinates(0, normalizedX, normalizedY, this.tilewidth, this.tileheight, w * tilewidth, out newx, out newy, out z);

                pointsArr[count].X = newx;
                pointsArr[count].Y = newy;



                ++count;
            }

            if (connectBackToStart)
            {
                string[] xy = pointString[0].Split(",".ToCharArray());
                int relativeX = Convert.ToInt32(xy[0]);
                int relativeY = Convert.ToInt32(xy[1]);

                float normalizedX = relativeX / (float)tileheight;
                float normalizedY = relativeY / (float)tileheight;

                CalculateWorldCoordinates(0, normalizedX, normalizedY, this.tilewidth, this.tileheight, w * tilewidth, out newx, out newy, out z);

                pointsArr[count].X = newx;
                pointsArr[count].Y = newy;
            }
            polygon.Points = pointsArr;

            return polygon;
        }

        public NodeNetwork ToNodeNetwork(bool requireTile = true)
        {
            return ToNodeNetwork(true, true, true, requireTile);
        }

        public NodeNetworkSave ToNodeNetworkSave(bool linkHorizontally, bool linkVertically, bool linkDiagonally, bool requireTile)
        {
            NodeNetwork nodeNetwork = ToNodeNetwork(linkHorizontally, linkVertically, linkDiagonally, requireTile);
            return NodeNetworkSave.FromNodeNetwork(nodeNetwork);
        }

        public NodeNetworkSave ToNodeNetworkSave(bool requireTile = true)
        {
            return ToNodeNetworkSave(true, true, true, requireTile);
        }

        public NodeNetwork ToNodeNetwork(bool linkHorizontally, bool linkVertically, bool linkDiagonally, bool requireTile)
        {
            var toReturn = new NodeNetwork();


            int layercount = 0;
            foreach (MapLayer mapLayer in this.Layers)
            {
                if (!mapLayer.IsVisible)
                {
                    switch (mapLayer.VisibleBehavior)
                    {
                        case LayerVisibleBehavior.Ignore:
                            break;
                        case LayerVisibleBehavior.Skip:
                            continue;
                    }
                }
                var allNodes = new Dictionary<int, Dictionary<int, Dictionary<int, PositionedNode>>>();
                allNodes[layercount] = new Dictionary<int, Dictionary<int, PositionedNode>>();


                MapLayer mLayer = mapLayer;
                int mLayerCount = layercount;
                Parallel.For(0, mapLayer.data[0].tiles.Count, count =>
                    {
                        uint gid = mLayer.data[0].tiles[count];

                        Tileset tileSet = GetTilesetForGid(gid);
                        if (tileSet != null || !requireTile)
                        {
                            var node = new PositionedNode();

                            //int tileWidth = requireTile ? tileSet.tilewidth : tilewidth;
                            //int tileHeight = requireTile ? tileSet.tileheight : tileheight;
                            int x = count % this.width;
                            int y = count / this.width;

                            float nodex;
                            float nodey;
                            float nodez;

                            CalculateWorldCoordinates(mLayerCount, count, tilewidth, tileheight, mLayer.width, out nodex, out nodey, out nodez);

                            node.X = nodex;
                            node.Y = nodey;
                            node.Z = nodez;

                            lock (allNodes)
                            {
                                if (!allNodes[mLayerCount].ContainsKey(x))
                                {
                                    allNodes[mLayerCount][x] = new Dictionary<int, PositionedNode>();
                                }

                                allNodes[mLayerCount][x][y] = node;
                            }
                            node.Name = string.Format("Node {0}", count);
                            lock (toReturn)
                            {
                                toReturn.AddNode(node);
                            }
                        }
                    });
                SetupNodeLinks(linkHorizontally, linkVertically, linkDiagonally, allNodes[layercount]);

                RemoveExcludedNodesViaPolygonLayer(toReturn, mapLayer, allNodes[layercount]);
                LowerNodesInNodesDownShapeCollection(mapLayer, allNodes[layercount]);
                RaiseNodesInNodesUpShapeCollection(mapLayer, allNodes[layercount]);

                ++layercount;
            }
            toReturn.UpdateShapes();

            return toReturn;
        }

        private void RaiseNodesInNodesUpShapeCollection(MapLayer mapLayer, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            ShapeCollection sc = this.ToShapeCollection(mapLayer.Name + " nodesup");
            List<PositionedNode> nodesToMoveUp = GetNodesThatCollideWithShapeCollection(sc, allNodes);

            foreach (var node in nodesToMoveUp)
            {
                node.Z += .001f;
            }
        }

        private void LowerNodesInNodesDownShapeCollection(MapLayer mapLayer, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            ShapeCollection sc = this.ToShapeCollection(mapLayer.Name + " nodesdown");
            List<PositionedNode> nodesToMoveDown = GetNodesThatCollideWithShapeCollection(sc, allNodes);

            foreach (var node in nodesToMoveDown)
            {
                node.Z -= .001f;
            }
        }

        private void RemoveExcludedNodesViaPolygonLayer(NodeNetwork nodeNetwork, MapLayer mapLayer, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            ShapeCollection sc = this.ToShapeCollection(mapLayer.Name + " nonodes");
            List<PositionedNode> nodesToRemove = GetNodesThatCollideWithShapeCollection(sc, allNodes);

            foreach (var node in nodesToRemove)
            {
                nodeNetwork.Remove(node);
            }
        }

        private List<PositionedNode> GetNodesThatCollideWithShapeCollection(ShapeCollection sc, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            var returnValue = new List<PositionedNode>();

            if (sc != null && sc.Polygons != null)
            {
                foreach (Polygon polygon in sc.Polygons)
                {
                    polygon.ForceUpdateDependencies();
                }

                foreach (var xpair in allNodes)
                {
                    foreach (var ypair in xpair.Value)
                    {
                        PositionedNode node = ypair.Value;
                        var rectangle = new AxisAlignedRectangle { Position = node.Position, ScaleX = 1, ScaleY = 1 };

                        if (sc.CollideAgainst(rectangle))
                        {
                            returnValue.Add(node);
                        }
                    }
                }
            }
            return returnValue;
        }

        private static void SetupNodeLinks(bool linkHorizontally, bool linkVertically, bool linkDiagonally, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            foreach (var xpair in allNodes)
            {
                int x = xpair.Key;
                foreach (var ypair in xpair.Value)
                {
                    int y = ypair.Key;

                    if (linkVertically && allNodes.ContainsKey(x - 1) && allNodes[x - 1].ContainsKey(y))
                    {
                        float cost = (ypair.Value.Position - allNodes[x - 1][y].Position).Length();
                        ypair.Value.LinkTo(allNodes[x - 1][y], cost);
                    }
                    if (linkHorizontally && xpair.Value.ContainsKey(y - 1))
                    {
                        float cost = (ypair.Value.Position - xpair.Value[y - 1].Position).Length();
                        ypair.Value.LinkTo(xpair.Value[y - 1], cost);
                    }
                    if (linkDiagonally && allNodes.ContainsKey(x - 1) && allNodes[x - 1].ContainsKey(y - 1))
                    {
                        float cost = (ypair.Value.Position - allNodes[x - 1][y - 1].Position).Length();
                        ypair.Value.LinkTo(allNodes[x - 1][y - 1], cost);
                    }
                    if (linkDiagonally && allNodes.ContainsKey(x + 1) && allNodes[x + 1].ContainsKey(y - 1))
                    {
                        float cost = (ypair.Value.Position - allNodes[x + 1][y - 1].Position).Length();
                        ypair.Value.LinkTo(allNodes[x + 1][y - 1], cost);
                    }
                }
            }
        }

        public SceneSave ToSceneSave(float scale, FileReferenceType referenceType = FileReferenceType.NoDirectory)
        {
            var toReturn = new SceneSave { CoordinateSystem = FlatRedBall.Math.CoordinateSystem.RightHanded };

            // TODO: Somehow add all layers separately

            int layercount = 0;
            foreach (MapLayer mapLayer in this.Layers)
            {
                if (!mapLayer.IsVisible)
                {
                    switch (mapLayer.VisibleBehavior)
                    {
                        case LayerVisibleBehavior.Ignore:
                            break;
                        case LayerVisibleBehavior.Skip:
                            continue;
                    }
                }

                MapLayer mLayer = mapLayer;
                int mLayerCount = layercount;

                for(int i = 0; i < mapLayer.data[0].tiles.Count; i++)
                //Parallel.For(0, mapLayer.data[0].tiles.Count, i =>
                {
                    uint gid = mLayer.data[0].tiles[i];
                    Tileset tileSet = GetTilesetForGid(gid);
                    if (tileSet != null)
                    {
                        SpriteSave sprite = CreateSpriteSaveFromMapTileset(scale, mLayerCount, mLayer, i, gid, tileSet, referenceType);
                        lock (toReturn)
                        {
                            toReturn.SpriteList.Add(sprite);
                        }
                    }
                    }
                //);
                ++layercount;
            }

            return toReturn;
        }

        private SpriteSave CreateSpriteSaveFromMapTileset(float scale, int layercount, MapLayer mapLayer, int tileIndex, uint gid, Tileset tileSet, FileReferenceType referenceType = FileReferenceType.NoDirectory)
        {
            var sprite = new SpriteSave();
            if (!mapLayer.IsVisible && mapLayer.VisibleBehavior == LayerVisibleBehavior.Match)
            {
                sprite.Visible = false;
            }

            int imageWidth = tileSet.Images[0].width;
            int imageHeight = tileSet.Images[0].height;
            int tileWidth = tileSet.Tilewidth;
            int spacing = tileSet.Spacing;
            int tileHeight = tileSet.Tileheight;
            int margin = tileSet.Margin;

            // TODO: only calculate these once per tileset. Perhaps it can be done in the deserialize method
            //int tilesWide = (imageWidth - margin) / (tileWidth + spacing);
            //int tilesHigh = (imageHeight - margin) / (tileHeight + spacing);

            if (referenceType == FileReferenceType.NoDirectory)
            {
                sprite.Texture = tileSet.Images[0].sourceFileName;
            }
            else if (referenceType == FileReferenceType.Absolute)
            {
                string directory = FileManager.GetDirectory(this.FileName);
                sprite.Texture = FileManager.RemoveDotDotSlash( directory + tileSet.Images[0].Source );

            }
            else
            {
                throw new NotImplementedException();
            }

            uint tileTextureRelativeToStartOfTileset = 
                (0x0fffffff & gid) - tileSet.Firstgid + 1;

            if (tileSet.TileDictionary.ContainsKey(tileTextureRelativeToStartOfTileset))
            {
                var dictionary = tileSet.TileDictionary[tileTextureRelativeToStartOfTileset].PropertyDictionary;

                foreach (var kvp in dictionary)
                {
                    var key = kvp.Key;

                    if (IsName(key))
                    {
                        sprite.Name = kvp.Value;
                    }
                }
            }

            SetSpriteTextureCoordinates(gid, sprite, tileSet, imageWidth, imageHeight, tileWidth, spacing, tileHeight, margin);
            CalculateWorldCoordinates(layercount, tileIndex, tileWidth, tileHeight, this.width, out sprite.X, out sprite.Y, out sprite.Z);

            sprite.ScaleX = tileWidth / 2.0f;
            sprite.ScaleY = tileHeight / 2.0f;

            if (tileSet.Tileoffset != null && tileSet.Tileoffset.Length == 1)
            {
                sprite.X += tileSet.Tileoffset[0].x;
                sprite.Y -= tileSet.Tileoffset[0].y;
            }


            sprite.X *= scale;
            sprite.Y *= scale;
            // Update August 28, 2012
            // The TMX converter splits
            // the Layers by their Z values.
            // We want each Layer to have its
            // own explicit Z value, so we don't
            // want to adjust the Z's when we scale:
            //sprite.Z *= scale;

            sprite.ScaleX *= scale;
            sprite.ScaleY *= scale;
            return sprite;
        }

        private static bool IsName(string key)
        {
            return property.GetStrippedName(key).ToLower() == "name";
        }

        public void CalculateWorldCoordinates(int layercount, int count, int tileWidth, int tileHeight, int layerWidth, out float x, out float y, out float z)
        {
            int normalizedX = count % this.width;
            int normalizedY = count / this.width;
            CalculateWorldCoordinates(layercount, normalizedX, normalizedY, tileWidth, tileHeight, layerWidth, out x, out y, out z);
        }

        private void CalculateWorldCoordinates(int layercount, float normalizedX, float normalizedY, int tileWidth, int tileHeight, int layerWidth, out float x, out float y, out float z)
        {
            if (this.orientation == null || this.orientation.Equals("orthogonal"))
            {
                x = (normalizedX * this.tilewidth) + (this.tilewidth / 2.0f);
                x += (tileWidth - this.tilewidth) / 2.0f;
                y = -(normalizedY * this.tileheight) - (this.tileheight / 2.0f);
                y += (tileHeight - this.tileheight) / 2.0f;
                z = layercount;
            }
            else if (this.orientation != null && this.orientation.Equals("isometric"))
            {
                y = -((normalizedX * this.tilewidth / 2.0f) + (normalizedY * this.tilewidth / 2.0f)) / 2;
                y += tileHeight / 2.0f;
                x = -((normalizedY * this.tilewidth / 2.0f) - (normalizedX * this.tileheight / 2.0f) * 2);
                x += tileWidth / 2.0f;
                z = ((normalizedY * layerWidth + normalizedX) * .000001f) + layercount;
            }
            else
            {
                throw new NotImplementedException("Unknown orientation type");
            }

            x += Offset.Item1;
            y += Offset.Item2;
            z += Offset.Item3;
        }

        public void SetSpriteTextureCoordinates(uint gid, SpriteSave sprite, Tileset tileSet, int imageWidth, int imageHeight, int tileWidth, int spacing, int tileHeight, int margin)
        {
            var gidWithoutRotation = gid & 0x0fffffff;

            const uint FlippedHorizontallyFlag = 0x80000000;
            const uint FlippedVerticallyFlag   = 0x40000000;
            const uint FlippedDiagonallyFlag   = 0x20000000;

            bool flipHorizontally = (gid & FlippedHorizontallyFlag) == FlippedHorizontallyFlag;
            bool flipVertically = (gid & FlippedVerticallyFlag) == FlippedVerticallyFlag;
            bool flipDiagonally = (gid & FlippedDiagonallyFlag) == FlippedDiagonallyFlag;

            // Calculate pixel coordinates in the texture sheet
            int leftPixelCoord = CalculateXCoordinate(gidWithoutRotation - tileSet.Firstgid, imageWidth, tileWidth, spacing, margin);
            int topPixelCoord = CalculateYCoordinate(gidWithoutRotation - tileSet.Firstgid, imageWidth, tileWidth, tileHeight, spacing, margin);
            int rightPixelCoord = leftPixelCoord + tileWidth;
            int bottomPixelCoord = topPixelCoord + tileHeight;

            if ((flipHorizontally && flipDiagonally == false) ||
                (flipVertically && flipDiagonally))
            {
                var temp = rightPixelCoord;
                rightPixelCoord = leftPixelCoord;
                leftPixelCoord = temp;
            }

            if ((flipVertically && flipDiagonally == false) ||
                (flipHorizontally && flipDiagonally))
            {
                var temp = topPixelCoord;
                topPixelCoord = bottomPixelCoord;
                bottomPixelCoord = temp;
                
            }

            //if (flipDiagonally)
            //{
            //    // this turns:
            //    // 1---2
            //    // |   |
            //    // 3---4

            //    // into:
            //    // 1---3
            //    // |   |
            //    // 2---4

            //    int newLeft = topPixelCoord;
            //    int newRight = bottomPixelCoord;
            //    int newTop = leftPixelCoord;
            //    int newBottom = rightPixelCoord;

            //    topPixelCoord = newTop;
            //    bottomPixelCoord = newBottom;
            //    leftPixelCoord = newLeft;
            //    rightPixelCoord = newRight;
            //}

            // Calculate relative texture coordinates based on pixel coordinates
            var changeVal = LessOrGreaterDesired.Greater;

            if (this.orientation != null && this.orientation.Equals("isometric"))
            {
                changeVal = LessOrGreaterDesired.NoChange;
            }

            sprite.TopTextureCoordinate = GetTextureCoordinate(topPixelCoord, imageHeight, changeVal);
            sprite.LeftTextureCoordinate = GetTextureCoordinate(leftPixelCoord, imageWidth, changeVal);

            changeVal = LessOrGreaterDesired.Less;
            if (this.orientation != null && this.orientation.Equals("isometric"))
            {
                changeVal = LessOrGreaterDesired.NoChange;
            }

            sprite.RightTextureCoordinate = GetTextureCoordinate(rightPixelCoord, imageWidth, changeVal);
            sprite.BottomTextureCoordinate = GetTextureCoordinate(bottomPixelCoord, imageHeight, changeVal);

            if (flipDiagonally)
            {
                sprite.RotationZ = Microsoft.Xna.Framework.MathHelper.PiOver2;
                sprite.FlipHorizontal = true;
            }
        }

        public Tileset GetTilesetForGid(uint gid)
        {
            // Assuming tilesets are sorted by the firstgid value...
            // Resort with LINQ if not
            if (Tilesets != null)
            {
                for (int i = Tilesets.Count - 1; i >= 0; --i)
                {
                    Tileset tileSet = Tilesets[i];
                    if (gid >= tileSet.Firstgid)
                    {
                        return tileSet;
                    }
                }
            }
            return null;
        }

        private float GetTextureCoordinate(int pixelCoord, int dimension, LessOrGreaterDesired lessOrGreaterDesired)
        {
            float asFloat = pixelCoord / (float)dimension;

            //const float modValue = .000001f;
            const float modValue = .000002f;
            //const float modValue = .00001f;
            switch (lessOrGreaterDesired)
            {
                case LessOrGreaterDesired.Greater:
                    return asFloat + modValue;
                case LessOrGreaterDesired.Less:
                    return asFloat - modValue;
                default:
                    return asFloat;
            }
        }
            
        private static int CalculateYCoordinate(uint gid, int imageWidth, int tileWidth, int tileHeight, int spacing, int margin)
        {

            int tilesWide = TilesetExtensionMethods.GetNumberOfTilesWide(
                imageWidth, margin, tileWidth, spacing);

            int normalizedy = (int)(gid / tilesWide);
            int pixely = normalizedy * (tileHeight + spacing) + margin;

            return pixely;
        }

        private static int CalculateXCoordinate(uint gid, int imageWidth, int tileWidth, int spacing, int margin)
        {
            var tilesWide = TilesetExtensionMethods.GetNumberOfTilesWide(
                imageWidth, margin, tileWidth, spacing);


            int normalizedX = (int)(gid % tilesWide);
            int pixelX = normalizedX * (tileWidth + spacing) + margin;

            return pixelX;
        }

        public static TiledMapSave FromFile(string fileName)
        {
            string oldRelativeDirectory = FileManager.RelativeDirectory;
            try
            {
                FileManager.RelativeDirectory = FileManager.GetDirectory(fileName);
            }
            catch
            {
            }
            var tms = FileManager.XmlDeserialize<TiledMapSave>(fileName);


            if (FileManager.IsRelative(fileName))
            {
                tms.FileName = FileManager.RelativeDirectory + fileName;
            }
            else
            {
                tms.FileName = fileName;
            }

            FileManager.RelativeDirectory = oldRelativeDirectory;

            foreach (MapLayer layer in tms.Layers)
            {
                if (!layer.PropertyDictionary.ContainsKey("VisibleBehavior"))
                {
                    layer.VisibleBehavior = LayerVisibleBehaviorValue;
                }
                else
                {
                    if (!Enum.TryParse(layer.PropertyDictionary["VisibleBehavior"], out layer.VisibleBehavior))
                    {
                        layer.VisibleBehavior = LayerVisibleBehaviorValue;
                    }
                }
            }
            return tms;
        }

        public void Save(string fileName)
        {
            FileManager.XmlSerialize(this, fileName);

        }
    }

}
