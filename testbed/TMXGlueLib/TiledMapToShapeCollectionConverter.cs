using FlatRedBall.Content.Math.Geometry;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Math.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMXGlueLib
{
    public static class TiledMapToShapeCollectionConverter
    {
        public static ShapeCollection ToShapeCollection(this TiledMapSave tiledMapSave, string layerName = null)
        {
            var scs = tiledMapSave.ToShapeCollectionSave(layerName);

            return scs.ToShapeCollection();
        }

        public static ShapeCollectionSave ToShapeCollectionSave(this TiledMapSave tiledMapSave, string layerName)
        {
            MapLayer mapLayer = null;
            if (!string.IsNullOrEmpty(layerName))
            {
                mapLayer = tiledMapSave.Layers.FirstOrDefault(l => l.Name.Equals(layerName));
            }
            var shapes = new ShapeCollectionSave();

            if ((mapLayer != null && !mapLayer.IsVisible && mapLayer.VisibleBehavior == TMXGlueLib.TiledMapSave.LayerVisibleBehavior.Skip) ||
                tiledMapSave.objectgroup == null || tiledMapSave.objectgroup.Length == 0)
            {
                return shapes;
            }

            foreach (mapObjectgroup group in tiledMapSave.objectgroup)
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
                                PolygonSave p = tiledMapSave.ConvertTmxObjectToFrbPolygonSave(@object.Name, @group.width, @group.height,
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
                                PolygonSave p = tiledMapSave.ConvertTmxObjectToFrbPolygonSave(@object.Name, @group.width, @group.height,
                                    @object.x, @object.y, polyline.points, false);
                                if (p != null)
                                {
                                    shapes.PolygonSaves.Add(p);
                                }
                            }
                        }

                        if (@object.polygon == null && @object.polyline == null)
                        {
                            PolygonSave p = tiledMapSave.ConvertTmxObjectToFrbPolygonSave(@object.Name, @group.width, @group.height, @object.x, @object.y, @object.width, @object.height);
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



        private static PolygonSave ConvertTmxObjectToFrbPolygonSave(this TiledMapSave tiledMapSave, string name, int groupWidth, int groupHeight, int x, int y, int w, int h)
        {
            var pointsSb = new StringBuilder();

            pointsSb.Append("0,0");

            pointsSb.AppendFormat(" {0},{1}", w, 0);
            pointsSb.AppendFormat(" {0},{1}", w, h);
            pointsSb.AppendFormat(" {0},{1}", 0, h);


            return tiledMapSave.ConvertTmxObjectToFrbPolygonSave(name, groupWidth, groupHeight, x, y, pointsSb.ToString(), true);
        }

        private static PolygonSave ConvertTmxObjectToFrbPolygonSave(this TiledMapSave tiledMapSave, string name, int w, int h, int x, int y, string points, bool connectBackToStart)
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

            if ("orthogonal".Equals(tiledMapSave.orientation))
            {
                fx -= tiledMapSave.tilewidth / 2.0f;
                fy -= tiledMapSave.tileheight + (tiledMapSave.tileheight / 2.0f);
            }
            else if ("isometric".Equals(tiledMapSave.orientation))
            {
                fx -= tiledMapSave.tilewidth / 4.0f;
                fy -= tiledMapSave.tileheight / 2.0f;
            }

            tiledMapSave.CalculateWorldCoordinates(
                0, fx / tiledMapSave.tileheight, fy / tiledMapSave.tileheight, 
                tiledMapSave.tilewidth, tiledMapSave.tileheight, 
                w * tiledMapSave.tilewidth, out newx, out newy, out z);

            polygon.X = newx - tiledMapSave.tilewidth / 2.0f;
            polygon.Y = newy - tiledMapSave.tileheight / 2.0f;
            var pointsArr = new Point[pointString.Length + (connectBackToStart ? 1 : 0)];

            int count = 0;
            foreach (string pointStr in pointString)
            {
                string[] xy = pointStr.Split(",".ToCharArray());
                int relativeX = Convert.ToInt32(xy[0]);
                int relativeY = Convert.ToInt32(xy[1]);

                float normalizedX = relativeX / (float)tiledMapSave.tileheight;
                float normalizedY = relativeY / (float)tiledMapSave.tileheight;

                tiledMapSave.CalculateWorldCoordinates(0, normalizedX, normalizedY, tiledMapSave.tilewidth, tiledMapSave.tileheight, w * tiledMapSave.tilewidth, out newx, out newy, out z);

                pointsArr[count].X = newx;
                pointsArr[count].Y = newy;



                ++count;
            }

            if (connectBackToStart)
            {
                string[] xy = pointString[0].Split(",".ToCharArray());
                int relativeX = Convert.ToInt32(xy[0]);
                int relativeY = Convert.ToInt32(xy[1]);

                float normalizedX = relativeX / (float)tiledMapSave.tileheight;
                float normalizedY = relativeY / (float)tiledMapSave.tileheight;

                tiledMapSave.CalculateWorldCoordinates(0, normalizedX, normalizedY, tiledMapSave.tilewidth, tiledMapSave.tileheight, w * tiledMapSave.tilewidth, out newx, out newy, out z);

                pointsArr[count].X = newx;
                pointsArr[count].Y = newy;
            }
            polygon.Points = pointsArr;

            return polygon;
        }


    }
}
