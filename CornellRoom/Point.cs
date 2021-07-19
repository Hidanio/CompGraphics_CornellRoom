using System;
using System.Collections.Generic;

namespace CornellRoom
{
    public class Point
    {
        public double x, y, z;

        public Point(double x, double y, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Point()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Point center()
        {
            return this;
        }

        public List<Point> points()
        {
            return new List<Point> {this};
        }

        public static bool operator ==(Point left, Point right)
        {
            if (ReferenceEquals(left, null) && !ReferenceEquals(right, null) ||
                !ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return false;
            return ReferenceEquals(left, null) && ReferenceEquals(right, null) ||
                   left.x == right.x && left.y == right.y && left.z == right.z;
        }

        public static Point operator +(Point left, Point right)
        {
            return new Point(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Point operator +(Point left, double d)
        {
            return new Point(left.x + d, left.y + d, left.z + d);
        }

        public static Point operator -(Point left, double d)
        {
            return left + -1 * d;
        }

        public static Point operator -(Point left, Point right)
        {
            return left + -1 * right;
        }

        public static Point operator *(Point left, double d)
        {
            return new Point(left.x * d, left.y * d, left.z * d);
        }

        public static Point operator /(Point left, double d)
        {
            return new Point(left.x / d, left.y / d, left.z / d);
        }

        public static Point operator *(double d, Point left)
        {
            return left * d;
        }

        public static double operator *(Point right, Point left)
        {
            return right.x * left.x + right.y * left.y + right.z * left.z;
        }

        public static bool operator !=(Point left, Point right)
        {
            return !(left == right);
        }

        public bool between(Point first, Point second)
        {
            return Math.Min(first.x, second.x) <= x &&
                   x <= Math.Max(first.x, second.x) &&
                   Math.Min(first.y, second.y) <= y &&
                   y <= Math.Max(first.y, second.y) &&
                   Math.Min(first.z, second.z) <= z &&
                   z <= Math.Max(first.z, second.z);
        }

        public double distance(Point to)
        {
            double dx = x - to.x, dy = y - to.y, dz = z - to.z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public Point norm()
        {
            var m = mag();
            var div = m == 0 ? double.PositiveInfinity : 1 / m;
            return times(div);
        }

        public double mag()
        {
            return Math.Sqrt(this * this);
        }

        public Point times(double d)
        {
            return new Point(x * d, y * d, z * d);
        }

        public Point times(Point t)
        {
            return new Point(x * t.x, y * t.y, z * t.z);
        }

        public Point cross(Point v)
        {
            return new Point(y * v.z - z * v.y,
                z * v.x - x * v.z,
                x * v.y - y * v.x);
        }
    }
}