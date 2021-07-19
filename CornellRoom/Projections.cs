using System;

namespace CornellRoom
{
    public static class Proections
    {
        public static double[,] multilyMatrix(double[,] left, double[,] right)
        {
            var r1 = left.GetLength(0);
            var c1 = left.GetLength(1);

            var r2 = right.GetLength(0);
            var c2 = right.GetLength(1);

            if (c1 != r2)
            {
                throw new Exception("Matrix multiplication with non-fitting sizes");
            }

            var res = new double[r1, c2];
            for (var i = 0; i < r1; i++)
            {
                for (var j = 0; j < c2; j++)
                {
                    res[i, j] = 0;
                    for (var k = 0; k < c1; k++)
                    {
                        res[i, j] += left[i, k] * right[k, j];
                    }
                }
            }

            return res;
        }

        public static Point pCentralProj(Point point, double xc, double yc, double zc)
        {
            //double a = yc == 0 ? 0 : -1.0 / yc;
            var b = zc == 0 ? 0 : -1.0 / zc;
            //double c = xc == 0 ? 0 : -1.0 / xc;
            double[,] left = {{point.y, point.z, point.x, 1}};
            double[,] right =
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 0, b},
                {0, 0, 0, 1}
            };
            var points = multilyMatrix(left, right);

            return new Point(points[0, 0] / points[0, 3], points[0, 1] / points[0, 3], points[0, 2] / points[0, 3]);
        }

        public static Point pIsometricProj(Point point, double phi, double psi)
        {
            var degPhi = phi * Math.PI / 180;
            var degPsi = psi * Math.PI / 180;

            double[,] left = {{point.x, point.y, point.z, 1}};
            double[,] right =
            {
                {Math.Cos(degPsi), Math.Sin(degPhi) * Math.Sin(degPsi), 0, 0},
                {0, Math.Cos(degPhi), 0, 0},
                {Math.Sin(degPsi), -Math.Sin(degPhi) * Math.Cos(degPsi), 0, 0},
                {0, 0, 0, 1}
            };

            var points = multilyMatrix(left, right);

            return new Point(points[0, 0], points[0, 1], points[0, 2]);
        }

        public static Point pOrtProj(Point point, char axis)
        {
            double xVal = 1, yVal = 1, zVal = 1;
            switch (axis)
            {
                case 'x':
                    xVal = 0;
                    break;
                case 'y':
                    yVal = 0;
                    break;
                case 'z':
                    zVal = 0;
                    break;
            }

            double[,] left = {{point.x, point.y, point.z, 1}};
            double[,] right =
            {
                {xVal, 0, 0, 0},
                {0, yVal, 0, 0},
                {0, 0, zVal, 0},
                {0, 0, 0, 1}
            };
            var points = multilyMatrix(left, right);
            double x = 0, y = 0, z = 0;
            switch (axis)
            {
                case 'x':
                    z = points[0, 0];
                    x = points[0, 1];
                    y = points[0, 2];
                    break;
                case 'y':
                    x = points[0, 0];
                    z = points[0, 1];
                    y = points[0, 2];
                    break;
                case 'z':
                    x = points[0, 0];
                    y = points[0, 1];
                    z = points[0, 2];
                    break;
            }

            return new Point(x, y, z);
        }
    }
}