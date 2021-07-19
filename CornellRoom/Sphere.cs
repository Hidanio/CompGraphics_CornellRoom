using System;

namespace CornellRoom
{
    public class Sphere : Shape
    {
        public Point center;
        public double r;

        public Point Center()
        {
            return center;
        }

        public override ISect Intersect(Ray ray)
        {
            var eo = Center() - ray.Start;
            var v = eo * ray.Dir;
            double dist;
            if (v < 0)
            {
                dist = 0;
            }
            else
            {
                var disc = Math.Pow(r, 2) - (eo * eo - Math.Pow(v, 2));
                dist = disc < 0 ? 0 : v - Math.Sqrt(disc);
            }

            if (dist == 0) return null;
            return new ISect
            {
                Thing = this,
                Ray = ray,
                Dist = dist
            };
            //Point s = Center() - start;
            //double a = direction * direction;
            //double b = s * direction;
            //double c = s * s - r * r;
            //double D = b * b - a * c;
            //if (D < Shape.eps)
            //    return (false, -1);
            //double qD = Math.Sqrt(D / 4);
            //double t1 = (b + qD) / a;
            //double t2 = (b - qD) / a;
            //double t = -1;
            //if (t1 < 0 && t2 < 0)
            //    return (false, -1);
            //else if (t1 < 0)
            //    t = t2;
            //else if (t2 < 0)
            //    t = t1;
            //else t = Math.Min(t1, t2);
            //return (true, t);
        }

        public override Point normal(Point pos)
        {
            return (pos - Center()).norm();
        }
    }
}