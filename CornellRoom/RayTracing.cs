using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CornellRoom
{
    internal class RayTracing
    {
        private readonly Point background_color = new Point(0, 0);
        public Camera camera;
        private readonly List<Light> lights;
        private readonly int max_depth = 5;
        private readonly List<Shape> objects;
        private readonly int width;
        private readonly int height;
        private readonly double x_size;
        private readonly double y_size;

        public RayTracing(int width, int height, Camera camera, double x_size, double y_size, int max_depth = 5)
        {
            objects = new List<Shape>();
            lights = new List<Light>();
            this.width = width;
            this.height = height;
            this.camera = camera;
            this.x_size = x_size;
            this.y_size = y_size;
            this.max_depth = max_depth;
        }

        private double fit(double f, double ceil)
        {
            return f > ceil ? ceil : f;
        }

        private Color ToColor(Point point)
        {
            var r = (int) Math.Round(fit(point.x, 1) * 255);
            var g = (int) Math.Round(fit(point.y, 1) * 255);
            var b = (int) Math.Round(fit(point.z, 1) * 255);
            return Color.FromArgb(r, g, b);
        }

        private Point ToPoint(Color color)
        {
            return new Point(color.R / 255.0, color.G / 255.0, color.B / 255.0);
        }

        public void AddObj(Shape shape)
        {
            objects.Add(shape);
        }

        public void AddLight(Light light)
        {
            lights.Add(light);
        }

        public Bitmap calculate()
        {
            var result = new Bitmap(width, height);
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var pixel = new Point(x, y);
                    var pixel_3d = get_pixel_position(pixel);
                    var color = TraceRay(new Ray {Start = camera.Pos, Dir = pixel_3d}, 0);
                    var draw_color = ToColor(color);
                    result.SetPixel(x, y, draw_color);
                }
            }

            return result;
        }

        private Point get_pixel_position(Point pixel)
        {
            pixel.x = (pixel.x - width / 2.0) * x_size / (2 * width);
            pixel.y = -(pixel.y - height / 2.0) * y_size / (2 * height);
            return (camera.Forward + camera.Right.times(pixel.x) + camera.Up.times(pixel.y)).norm();
        }

        private IEnumerable<ISect> Intersections(Ray ray)
        {
            return objects.Select(obj => obj.Intersect(ray))
                .Where(inter => inter != null)
                .OrderBy(inter => inter.Dist);
        }

        private double TestRay(Ray ray)
        {
            var isects = Intersections(ray);
            var isect = isects.FirstOrDefault();
            if (isect == null)
                return 0;
            return isect.Dist;
        }

        private Point TraceRay(Ray ray, int depth)
        {
            var isects = Intersections(ray);
            var isect = isects.FirstOrDefault();
            if (isect == null)
                return background_color;
            return Shade(isect, depth);
        }

        private Point GetNaturalColor(Shape thing, Point pos, Point norm, Point rd)
        {
            var result = background_color;
            foreach (var light in lights)
            {
                var light_distance = light.position - pos;
                var light_norm = light_distance.norm();
                var near_t = TestRay(new Ray {Start = pos, Dir = light_norm});
                var isInShadow = !(near_t > light_distance.mag() || near_t == 0);
                if (!isInShadow)
                {
                    var illum = light_norm * norm;
                    var lcolor = illum > 0 ? illum * light.color * light.intensity : new Point(0, 0);
                    var specular = light_norm * rd.norm();
                    var scolor = specular > 0
                        ? Math.Pow(specular, thing.surface.Roughness) * light.color * light.intensity
                        : new Point(0, 0);
                    result += thing.surface.Diffuse(pos).times(lcolor) + thing.surface.Specular(pos).times(scolor);
                }
            }

            return result;
        }

        private Point GetReflectionColor(Shape thing, Point pos, Point norm, Point rd, int depth)
        {
            return thing.surface.Reflect(pos) * TraceRay(new Ray {Start = pos, Dir = rd}, depth + 1);
        }

        private Point Shade(ISect isect, int depth)
        {
            var pos = isect.Dist * isect.Ray.Dir + isect.Ray.Start;
            var normal = isect.Thing.normal(pos);
            var reflectDir = isect.Ray.Dir - 2 * (normal * isect.Ray.Dir) * normal;
            var result = background_color;
            result += GetNaturalColor(isect.Thing, pos, normal, reflectDir);
            if (depth >= max_depth)
            {
                return result + new Point(.5, .5, .5);
            }

            return result + GetReflectionColor(isect.Thing, pos + .001 * reflectDir, normal, reflectDir, depth);
        }

        //Point cast(Point start, Point direction)
        //{
        //    double min_distance = double.MaxValue;
        //    int id = -1;
        //    for (int i = 0; i < objects.Count; i++)
        //    {
        //        var obj = objects[i];
        //        var (intersect, distance) = obj.Intersect(start, direction);
        //        if (intersect && distance < min_distance)
        //        {
        //            min_distance = distance;
        //            id = i;
        //        }
        //    }
        //    var result = background_color;
        //    if (id == -1)
        //        return result;
        //    var min_obj = objects[id];
        //    //result += min_obj.color;
        //    var int_point = start + min_distance * direction;
        //    var normal = int_point.norm();
        //    var reflect_dir = direction - ((direction * normal) * 2) * normal;
        //    foreach (var light in lights)
        //    {
        //        var light_dir = light.position - int_point;
        //        var light_norm = light_dir.norm();
        //        bool shadowed = false;
        //        for (int i = 0; i < objects.Count; i++)
        //        {
        //            var obj = objects[i];
        //            var (intersect, distance) = obj.Intersect(int_point, light_norm);
        //            if(intersect)
        //            {
        //                shadowed = true;
        //                break;
        //            }
        //        }
        //        if(!shadowed)
        //        {
        //            //double illum = light_norm * normal;
        //            //var lcolor = illum > 0 ? illum * light.color : background_color;
        //            //double specular = light_norm * reflect_dir;
        //            //var scolor = specular > 0 ? Math.Pow(specular, 1) * light.color : background_color;
        //            //result += lcolor + scolor;
        //            result += min_obj.color * light.intensity * Math.Max(0, light_norm * (int_point - min_obj.Center()).norm());
        //        }
        //    }
        //    return result;
        //}
    }
}