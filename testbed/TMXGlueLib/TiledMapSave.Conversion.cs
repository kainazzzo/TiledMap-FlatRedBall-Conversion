using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatRedBall.IO;
using FlatRedBall.Content.Scene;
using FlatRedBall.Content;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Math.Geometry;
using FlatRedBall.Content.AI.Pathfinding;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Content.Math.Geometry;
using FlatRedBall;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace TiledMap
{
    public partial class TiledMapSave
    {
        public enum CSVPropertyType { Tile, Layer, Map };
        public enum LayerVisibleBehavior { Ignore, Match, Skip };

        public static LayerVisibleBehavior layerVisibleBehavior = LayerVisibleBehavior.Ignore;

        enum LessOrGreaterDesired
        {
            Less,
            Greater,
            NoChange
        }

        public Scene ToScene(string contentManagerName, float scale)
        {
            SpriteEditorScene scene = ToSpriteEditorScene(scale);
            return scene.ToScene(contentManagerName);
        }

        public ShapeCollectionSave ToShapeCollectionSave()
        {
            return ToShapeCollectionSave(null);
        }

        public ShapeCollection ToShapeCollection()
        {
            return ToShapeCollectionSave().ToShapeCollection();
        }

        public ShapeCollection ToShapeCollection(string layerName)
        {
            ShapeCollectionSave scs = ToShapeCollectionSave(layerName);

            return scs.ToShapeCollection();
        }

        public string ToCSVString(CSVPropertyType type = CSVPropertyType.Tile)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> columnNames = GetColumnNames(type);
            WriteColumnHeader(sb, columnNames);
            WriteColumnValues(sb, columnNames, type);

            return sb.ToString();
        }

        private void WriteColumnValues(StringBuilder sb, IEnumerable<string> columnNames, CSVPropertyType type)
        {
            // TODO: There is probably a good way to refactor this code
            if (type == CSVPropertyType.Tile)
            {
                foreach (mapTileset tileSet in this.tileset)
                {
                    if (tileSet.tile != null)
                    {
                        foreach (mapTilesetTile tile in tileSet.tile)
                        {
                            WriteValuesFromDictionary(sb, tile.PropertyDictionary, columnNames);
                        }
                    }
                }
            }
            else if (type == CSVPropertyType.Layer)
            {
                foreach (mapLayer layer in this.layer)
                {
                    WriteValuesFromDictionary(sb, layer.PropertyDictionary, columnNames);
                }
            }
            else if (type == CSVPropertyType.Map)
            {
                WriteValuesFromDictionary(sb, PropertyDictionary, columnNames);
            }
        }

        private void WriteValuesFromDictionary(StringBuilder sb, IDictionary<string, string> iDictionary, IEnumerable<string> columnNames)
        {
            if (iDictionary.Any(p => p.Key.Equals("name", StringComparison.CurrentCultureIgnoreCase)))
            {
                sb.Append(iDictionary.Where(p => p.Key.Equals("name", StringComparison.CurrentCultureIgnoreCase)).First().Value);

                foreach (string columnName in columnNames)
                {
                    if (!columnName.Equals("name", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (iDictionary.Any(p => p.Key.Equals(columnName, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            sb.AppendFormat(",\"{0}\"", iDictionary.First(p => p.Key.Equals(columnName, StringComparison.CurrentCultureIgnoreCase)).Value.Replace("\"", "\"\""));
                        }
                        else
                        {
                            sb.Append(",");
                        }
                    }
                }
                sb.AppendLine();
            }
        }

        private static void WriteColumnHeader(StringBuilder sb, IEnumerable<string> columnNames)
        {
            sb.Append("Name (required)");
            foreach (string columnName in columnNames)
            {
                if (!columnName.Equals("name", StringComparison.CurrentCultureIgnoreCase))
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

        public class CaseInsensitiveEqualityComparer<T> : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return x.Equals(y, StringComparison.CurrentCultureIgnoreCase);
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLowerInvariant().GetHashCode();
            }
        }

        private IEnumerable<string> GetColumnNames(CSVPropertyType type)
        {
            HashSet<string> columnNames = new HashSet<string>();

            if (type == CSVPropertyType.Tile)
            {
                return this.tileset.SelectMany(t => t.tile).SelectMany(tile => tile.PropertyDictionary).Select(d => d.Key).Distinct(new CaseInsensitiveEqualityComparer<string>());
            }
            else if (type == CSVPropertyType.Layer)
            {
                return this.layer.SelectMany(l => l.PropertyDictionary).Select(d => d.Key).Distinct(new CaseInsensitiveEqualityComparer<string>());
            }
            else if (type == CSVPropertyType.Map)
            {
                return this.PropertyDictionary.Select(d => d.Key).Distinct(new CaseInsensitiveEqualityComparer<string>());
            }
            return columnNames;
        }

        public ShapeCollectionSave ToShapeCollectionSave(string layerName)
        {
            mapLayer layer = null;
            if (!string.IsNullOrEmpty(layerName))
            {
                layer = this.layer.FirstOrDefault(l => l.name.Equals(layerName));
            }
            ShapeCollectionSave shapes = new ShapeCollectionSave();

            if ((layer != null && !layer.IsVisible && layer.VisibleBehavior == LayerVisibleBehavior.Skip) ||
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
                                PolygonSave p = convertTMXObjectToFRBPolygonSave(group.width, group.height,
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
                                PolygonSave p = convertTMXObjectToFRBPolygonSave(group.width, group.height,
                                    @object.x, @object.y, polyline.points, false);
                                if (p != null)
                                {
                                    shapes.PolygonSaves.Add(p);
                                }
                            }
                        }

                        if (@object.polygon == null && @object.polyline == null)
                        {
                            PolygonSave p = convertTMXObjectToFRBPolygonSave(group.width, group.height, @object.x, @object.y, @object.width, @object.height);
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

        private PolygonSave convertTMXObjectToFRBPolygonSave(int groupWidth, int groupHeight, int x, int y, int width, int height)
        {
            StringBuilder pointsSB = new StringBuilder();

            pointsSB.Append("0,0");

            pointsSB.AppendFormat(" {0},{1}", width, 0);
            pointsSB.AppendFormat(" {0},{1}", width, height);
            pointsSB.AppendFormat(" {0},{1}", 0, height);


            return convertTMXObjectToFRBPolygonSave(groupWidth, groupHeight, x, y, pointsSB.ToString(), true);
        }

        private PolygonSave convertTMXObjectToFRBPolygonSave(int width, int height, int x, int y, string points, bool connectBackToStart)
        {
            if (string.IsNullOrEmpty(points))
            {
                return null;
            }
            PolygonSave polygon = new PolygonSave();
            string[] pointString = points.Split(" ".ToCharArray());
            float z;
            float newx;
            float newy;
            float fx = x;
            float fy = y;

            if ("orthogonal".Equals(this.orientation))
            {
                fx -= tilewidth / 2.0f;
                fy -= tileheight - (tileheight / 2.0f);
            }
            else if ("isometric".Equals(this.orientation))
            {
                fx -= tilewidth / 4.0f;
                fy -= tileheight / 2.0f;
            }

            calculateWorldCoordinates(0, fx / (float)tileheight, fy / (float)tileheight, this.tilewidth, this.tileheight, width * tilewidth, out newx, out newy, out z);
            polygon.X = newx - tilewidth / 2.0f;
            polygon.Y = newy - tileheight / 2.0f;
            Point[] pointsArr = new Point[pointString.Length + (connectBackToStart ? 1 : 0)];

            int count = 0;
            foreach (string pointStr in pointString)
            {
                string[] xy = pointStr.Split(",".ToCharArray());
                int relativeX = Convert.ToInt32(xy[0]);
                int relativeY = Convert.ToInt32(xy[1]);

                float normalizedX = relativeX / (float)tileheight;
                float normalizedY = relativeY / (float)tileheight;

                calculateWorldCoordinates(0, normalizedX, normalizedY, this.tilewidth, this.tileheight, width * tilewidth, out newx, out newy, out z);

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

                calculateWorldCoordinates(0, normalizedX, normalizedY, this.tilewidth, this.tileheight, width * tilewidth, out newx, out newy, out z);

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
            NodeNetwork toReturn = new NodeNetwork();


            int layercount = 0;
            foreach (mapLayer layer in this.layer)
            {
                if (!layer.IsVisible)
                {
                    switch (layer.VisibleBehavior)
                    {
                        case LayerVisibleBehavior.Ignore:
                            break;
                        case LayerVisibleBehavior.Skip:
                            continue;
                    }
                }
                Dictionary<int, Dictionary<int, Dictionary<int, PositionedNode>>> allNodes = new Dictionary<int, Dictionary<int, Dictionary<int, PositionedNode>>>();
                allNodes[layercount] = new Dictionary<int, Dictionary<int, PositionedNode>>();


                try
                {
                    Parallel.For(0, layer.data[0].tiles.Count, (count) =>
                    {
                        uint gid = layer.data[0].tiles[(int)count];

                        mapTileset tileSet = GetTilesetForGid(gid);
                        if (tileSet != null || !requireTile)
                        {
                            PositionedNode node = new PositionedNode();

                            int tileWidth = requireTile ? tileSet.tilewidth : tilewidth;
                            int tileHeight = requireTile ? tileSet.tileheight : tileheight;
                            int X = count % this.width;
                            int Y = count / this.width;

                            float nodex;
                            float nodey;
                            float nodez;

                            CalculateWorldCoordinates(layercount, count, tilewidth, tileheight, layer.width, out nodex, out nodey, out nodez);

                            node.X = nodex;
                            node.Y = nodey;
                            node.Z = nodez;

                            lock (allNodes)
                            {
                                if (!allNodes[layercount].ContainsKey(X))
                                {
                                    allNodes[layercount][X] = new Dictionary<int, PositionedNode>();
                                }

                                allNodes[layercount][X][Y] = node;
                            }
                            node.Name = string.Format("Node {0}", count);
                            lock (toReturn)
                            {
                                toReturn.AddNode(node);
                            }
                        }
                    });


                }
                catch (AggregateException)
                {

                    throw;
                } setupNodeLinks(linkHorizontally, linkVertically, linkDiagonally, allNodes[layercount]);

                removeExcludedNodesViaPolygonLayer(toReturn, layer, allNodes[layercount]);
                lowerNodesInNodesDownShapeCollection(layer, allNodes[layercount]);
                raiseNodesInNodesUpShapeCollection(layer, allNodes[layercount]);

                ++layercount;
            }
            toReturn.UpdateShapes();

            return toReturn;
        }

        private void raiseNodesInNodesUpShapeCollection(mapLayer layer, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            ShapeCollection sc = this.ToShapeCollection(layer.name + " nodesup");
            List<PositionedNode> nodesToMoveUp = getNodesThatCollideWithShapeCollection(sc, allNodes);

            foreach (PositionedNode node in nodesToMoveUp)
            {
                node.Z += .001f;
            }
        }

        private void lowerNodesInNodesDownShapeCollection(mapLayer layer, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            ShapeCollection sc = this.ToShapeCollection(layer.name + " nodesdown");
            List<PositionedNode> nodesToMoveDown = getNodesThatCollideWithShapeCollection(sc, allNodes);

            foreach (PositionedNode node in nodesToMoveDown)
            {
                node.Z -= .001f;
            }
        }

        private void removeExcludedNodesViaPolygonLayer(NodeNetwork nodeNetwork, mapLayer layer, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            ShapeCollection sc = this.ToShapeCollection(layer.name + " nonodes");
            List<PositionedNode> nodesToRemove = getNodesThatCollideWithShapeCollection(sc, allNodes);

            foreach (PositionedNode node in nodesToRemove)
            {
                nodeNetwork.Remove(node);
            }
        }

        private List<PositionedNode> getNodesThatCollideWithShapeCollection(ShapeCollection sc, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            List<PositionedNode> returnValue = new List<PositionedNode>();

            if (sc != null && sc.Polygons != null)
            {
                foreach (Polygon polygon in sc.Polygons)
                {
                    polygon.ForceUpdateDependencies();
                }

                foreach (KeyValuePair<int, Dictionary<int, PositionedNode>> xpair in allNodes)
                {
                    int x = xpair.Key;
                    foreach (KeyValuePair<int, PositionedNode> ypair in xpair.Value)
                    {
                        PositionedNode node = ypair.Value;
                        AxisAlignedRectangle rectangle = new AxisAlignedRectangle();
                        rectangle.Position = node.Position;
                        rectangle.ScaleX = 1;
                        rectangle.ScaleY = 1;

                        if (sc.CollideAgainst(rectangle))
                        {
                            returnValue.Add(node);
                        }
                    }
                }
            }
            return returnValue;
        }

        private static void setupNodeLinks(bool linkHorizontally, bool linkVertically, bool linkDiagonally, Dictionary<int, Dictionary<int, PositionedNode>> allNodes)
        {
            foreach (KeyValuePair<int, Dictionary<int, PositionedNode>> xpair in allNodes)
            {
                int x = xpair.Key;
                foreach (KeyValuePair<int, PositionedNode> ypair in xpair.Value)
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

        public SpriteEditorScene ToSpriteEditorScene(float scale)
        {
            SpriteEditorScene toReturn = new SpriteEditorScene();
            toReturn.CoordinateSystem = FlatRedBall.Math.CoordinateSystem.RightHanded;

            // TODO: Somehow add all layers separately

            int layercount = 0;
            foreach (mapLayer layer in this.layer)
            {
                if (!layer.IsVisible)
                {
                    switch (layer.VisibleBehavior)
                    {
                        case LayerVisibleBehavior.Ignore:
                            break;
                        case LayerVisibleBehavior.Skip:
                            continue;
                    }
                }

                try
                {
                    Parallel.For(0, layer.data[0].tiles.Count, (count) =>
                    {
                        uint gid = layer.data[0].tiles[count];
                        mapTileset tileSet = GetTilesetForGid(gid);
                        if (tileSet != null)
                        {
                            SpriteSave sprite = CreateSpriteSaveFromMapTileset(scale, layercount, layer, count, gid, tileSet);
                            lock (toReturn)
                            {
                                toReturn.SpriteList.Add(sprite);
                            }
                        }
                    });

                }
                catch (AggregateException)
                {

                    throw;
                } ++layercount;
            }

            return toReturn;
        }

        private SpriteSave CreateSpriteSaveFromMapTileset(float scale, int layercount, mapLayer layer, int count, uint gid, mapTileset tileSet)
        {
            SpriteSave sprite = new SpriteSave();
            if (!layer.IsVisible && layer.VisibleBehavior == LayerVisibleBehavior.Match)
            {
                sprite.Visible = false;
            }

            int imageWidth = tileSet.image[0].width;
            int imageHeight = tileSet.image[0].height;
            int tileWidth = tileSet.tilewidth;
            int spacing = tileSet.spacing;
            int tileHeight = tileSet.tileheight;
            int margin = tileSet.margin;

            // TODO: only calculate these once per tileset. Perhaps it can be done in the deserialize method
            int tilesWide = (imageWidth - margin) / (tileWidth + spacing);
            int tilesHigh = (imageHeight - margin) / (tileHeight + spacing);


            sprite.Texture = tileSet.image[0].source;
            if (tileSet.tileDictionary.ContainsKey(gid - tileSet.firstgid + 1))
            {
                if (tileSet.tileDictionary[gid - tileSet.firstgid + 1].PropertyDictionary.ContainsKey("name"))
                {
                    sprite.Name = tileSet.tileDictionary[gid - tileSet.firstgid + 1].PropertyDictionary["name"];
                }
                else if (tileSet.tileDictionary[gid - tileSet.firstgid + 1].PropertyDictionary.ContainsKey("Name"))
                {
                    sprite.Name = tileSet.tileDictionary[gid - tileSet.firstgid + 1].PropertyDictionary["Name"];
                }
            }

            setSpriteTextureCoordinates(gid, sprite, tileSet, imageWidth, imageHeight, tileWidth, spacing, tileHeight, margin);
            CalculateWorldCoordinates(layercount, count, tileWidth, tileHeight, this.width, out sprite.X, out sprite.Y, out sprite.Z);

            sprite.ScaleX = tileWidth / 2;
            sprite.ScaleY = tileHeight / 2;

            if (tileSet.tileoffset != null && tileSet.tileoffset.Length == 1)
            {
                sprite.X += tileSet.tileoffset[0].x;
                sprite.Y -= tileSet.tileoffset[0].y;
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

        public void CalculateWorldCoordinates(int layercount, int count, int tileWidth, int tileHeight, int layerWidth, out float x, out float y, out float z)
        {
            int normalizedX = count % this.width;
            int normalizedY = count / this.width;
            calculateWorldCoordinates(layercount, normalizedX, normalizedY, tileWidth, tileHeight, layerWidth, out x, out y, out z);
        }

        private void calculateWorldCoordinates(int layercount, float normalizedX, float normalizedY, int tileWidth, int tileHeight, int layerWidth, out float x, out float y, out float z)
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
                y = -(float)((normalizedX * this.tilewidth / 2.0f) + (normalizedY * this.tilewidth / 2.0f)) / 2;
                y += tileHeight / 2.0f;
                x = -(float)(((normalizedY * this.tilewidth / 2.0f) - (normalizedX * this.tileheight / 2.0f) * 2));
                x += tileWidth / 2.0f;
                z = ((normalizedY * layerWidth + normalizedX) * .000001f) + layercount;
            }
            else
            {
                throw new NotImplementedException("Unknown orientation type");
            }
        }

        private void setSpriteTextureCoordinates(uint gid, SpriteSave sprite, mapTileset tileSet, int imageWidth, int imageHeight, int tileWidth, int spacing, int tileHeight, int margin)
        {
            // Calculate pixel coordinates in the texture sheet
            int leftPixelCoord = TiledMapSave.calculateXCoordinate(gid - tileSet.firstgid, imageWidth, tileWidth, spacing, margin);
            int topPixelCoord = TiledMapSave.calculateYCoordinate(gid - tileSet.firstgid, imageWidth, tileWidth, tileHeight, spacing, margin);
            int rightPixelCoord = leftPixelCoord + tileWidth;
            int bottomPixelCoord = topPixelCoord + tileHeight;

            // Calculate relative texture coordinates based on pixel coordinates
            LessOrGreaterDesired changeVal = LessOrGreaterDesired.Greater;

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
        }

        public mapTileset GetTilesetForGid(uint gid)
        {
            // Assuming tilesets are sorted by the firstgid value...
            // Resort with LINQ if not
            for (int i = tileset.Length - 1; i >= 0; --i)
            {
                mapTileset tileSet = tileset[i];
                if (gid >= tileSet.firstgid)
                {
                    return tileSet;
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
            if (lessOrGreaterDesired == LessOrGreaterDesired.Greater)
            {
                return asFloat + modValue;
            }
            else if (lessOrGreaterDesired == LessOrGreaterDesired.Less)
            {
                return asFloat - modValue;
            }
            else
            {
                return asFloat;
            }
        }

        private static int calculateYCoordinate(uint gid, int imageWidth, int tileWidth, int tileHeight, int spacing, int margin)
        {
            int tilesWide = (imageWidth - margin) / (tileWidth + spacing);
            int normalizedy = (int)(gid / tilesWide);
            int pixely = normalizedy * (tileHeight + spacing) + margin;

            return pixely;
        }

        private static int calculateXCoordinate(uint gid, int imageWidth, int tileWidth, int spacing, int margin)
        {
            int tilesWide = (imageWidth - margin) / (tileWidth + spacing);
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
            catch (Exception)
            {
            }
            TiledMapSave tms = FileManager.XmlDeserialize<TiledMapSave>(fileName);
            FileManager.RelativeDirectory = oldRelativeDirectory;

            foreach (mapLayer layer in tms.layer)
            {
                if (!layer.PropertyDictionary.ContainsKey("VisibleBehavior"))
                {
                    layer.VisibleBehavior = layerVisibleBehavior;
                }
                else
                {
                    if (!Enum.TryParse<LayerVisibleBehavior>(layer.PropertyDictionary["VisibleBehavior"], out layer.VisibleBehavior))
                    {
                        layer.VisibleBehavior = layerVisibleBehavior;
                    }
                }
            }
            return tms;
        }

        public void Save(string fileName)
        {
            FileManager.XmlSerialize<TiledMapSave>(this, fileName);

        }
    }

    public partial class mapLayer
    {
        [XmlIgnore]
        public TiledMapSave.LayerVisibleBehavior VisibleBehavior = TiledMapSave.LayerVisibleBehavior.Ignore;
    }
}
