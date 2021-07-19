namespace CornellRoom
{
    public abstract class Shape
    {
        public static double eps = 0.0001;
        public Surface surface;
        public abstract ISect Intersect(Ray ray);
        public abstract Point normal(Point point);
    }
}