using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PDCore.Helpers
{
    public class Vector
    {
        private const double Eps = 1e-7;

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator *(double d, Vector v)
        {
            return new Vector(d * v.X, d * v.Y);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            if (a is null)
            {
                return b is null;
            }

            if (b is null)
            {
                return false;
            }

            return Math.Abs(a.X - b.X) < Eps && Math.Abs(a.Y - b.Y) < Eps;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public static implicit operator Vector(double[] point)
        {
            return new Vector(point[0], point[1]);
        }

        public static implicit operator Vector(Point point)
        {
            return new Vector(point.X, point.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vector;

            return other != null && Math.Abs(other.X - X) < Eps && Math.Abs(other.Y - Y) < Eps;
        }

        public override string ToString()
        {
            return string.Format("Vector({0:0.0000}, {1:0.0000})", X, Y);
        }
    }
}
