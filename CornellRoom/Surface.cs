using System;

namespace CornellRoom
{
    public class Surface
    {
        public Func<Point, Point> Diffuse;
        public Func<Point, double> Reflect;
        public double Roughness;
        public Func<Point, Point> Specular;
    }
}