﻿using FlatRedBall.Content.Math.Geometry;
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
                                PolygonSave p = tiledMapSave.ConvertTmxObjectToFrbPolygonSave(@object.Name,
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
                                PolygonSave p = tiledMapSave.ConvertTmxObjectToFrbPolygonSave(@object.Name, 
                                    @object.x, @object.y, polyline.points, false);
                                if (p != null)
                                {
                                    shapes.PolygonSaves.Add(p);
                                }
                            }
                        }

                        if (@object.polygon == null && @object.polyline == null)
                        {
                            PolygonSave p = tiledMapSave.ConvertTmxObjectToFrbPolygonSave(@object.Name, @object.x, @object.y, @object.width, @object.height);
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



        private static PolygonSave ConvertTmxObjectToFrbPolygonSave(this TiledMapSave tiledMapSave, string name, int x, int y, int w, int h)
        {
            var pointsSb = new StringBuilder();

            pointsSb.Append("0,0");

            pointsSb.AppendFormat(" {0},{1}", w, 0);
            pointsSb.AppendFormat(" {0},{1}", w, -h);
            pointsSb.AppendFormat(" {0},{1}", 0, -h);


            return tiledMapSave.ConvertTmxObjectToFrbPolygonSave(name, x, y, pointsSb.ToString(), true);
        }

        private static PolygonSave ConvertTmxObjectToFrbPolygonSave(this TiledMapSave tiledMapSave, string name, int x, int y, string points, bool connectBackToStart)
        {
            if (string.IsNullOrEmpty(points))
            {
                return null;
            }

            var polygon = new PolygonSave();
            string[] pointString = points.Split(" ".ToCharArray());
            float z;

            polygon.Name = name;

            // Nov. 19th, 2014 - Domenic:
            // I am ripping this code apart a little, because shapes really should not involve tile sizes in their x/y calculations.
            // I'm not sure why this was ever done this way, as TMX gives the X/Y and width/height already. The old way was basically to convert
            // the x/y coordinates into tile based coordinates and then re-convert back to full x/y coordinates. This makes no sense any more to me.
            //
            // Having examined TMX format a little more, it seems that the x/y position is always specified
            //
            //float fx = x;
            //float fy = y;

            //if ("orthogonal".Equals(tiledMapSave.orientation))
            //{
            //    fx -= tiledMapSave.tilewidth / 2.0f;
            //    fy -= tiledMapSave.tileheight + (tiledMapSave.tileheight / 2.0f);
            //}
            //else if ("isometric".Equals(tiledMapSave.orientation))
            //{
            //    fx -= tiledMapSave.tilewidth / 4.0f;
            //    fy -= tiledMapSave.tileheight / 2.0f;
            //}

            //tiledMapSave.CalculateWorldCoordinates(
            //    0, fx / tiledMapSave.tileheight, fy / tiledMapSave.tileheight, 
            //    tiledMapSave.tilewidth, tiledMapSave.tileheight, 
            //    w * tiledMapSave.tilewidth, out newx, out newy, out z);

            //polygon.X = newx - tiledMapSave.tilewidth / 2.0f;
            //polygon.Y = newy - tiledMapSave.tileheight / 2.0f;
            //var pointsArr = new Point[pointString.Length + (connectBackToStart ? 1 : 0)];

            var pointsList =
                pointString.Select(p =>
                {
                    var xy = p.Split(",".ToCharArray());
                    return new Point
                    {
                        X = Convert.ToInt32(xy[0]),
                        Y = Convert.ToInt32(xy[1])
                    };
                }).ToList();

            if (connectBackToStart)
            {
                pointsList.Add(new Point(pointsList[0].X, pointsList[0].Y));
            }

            polygon.Points = pointsList.ToArray();
            polygon.X = x;
            polygon.Y = -y;

            return polygon;
        }


    }
}