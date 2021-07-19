namespace CornellRoom
{
    internal class Camera
    {
        public Point Forward;
        public Point Pos;
        public Point Right;
        public Point Up;

        public static Camera Create(Point pos, Point lookAt)
        {
            var forward = (lookAt - pos).norm();
            var down = new Point(0, -1);
            var right = forward.cross(down).norm().times(1.5);
            var up = forward.cross(right).norm().times(1.5);

            return new Camera {Pos = pos, Forward = forward, Up = up, Right = right};
        }
    }
}