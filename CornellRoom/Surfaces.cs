using System;

namespace CornellRoom
{
    internal static class Surfaces
    {
        public static readonly Surface CheckerBoard =
            new Surface
            {
                Diffuse = pos => (Math.Floor(pos.z) + Math.Floor(pos.x)) % 2 != 0
                    ? new Point(1, 1, 1)
                    : new Point(0, 0),
                Specular = pos => new Point(1, 1, 1),
                Reflect = pos => (Math.Floor(pos.z) + Math.Floor(pos.x)) % 2 != 0
                    ? .1
                    : .7,
                Roughness = 150
            };

        public static Surface fill(Point color)
        {
            return new Surface
            {
                Diffuse = pos => color,
                Specular = pos => color,
                Reflect = pos => 0,
                Roughness = 10
            };
        }

        public static Surface Shiny(double reflect = 0.6, double roughness = 50)
        {
            return new Surface
            {
                Diffuse = pos => new Point(1, 1, 1),
                Specular = pos => new Point(.5, .5, .5),
                Reflect = pos => reflect,
                Roughness = roughness
            };
        }
    }
}