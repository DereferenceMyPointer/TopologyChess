using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TopologyChess
{
    public struct IntVector : IEquatable<IntVector>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public IntVector() { }
        public IntVector(int x, int y)
        {
            X = x; Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is IntVector vec && vec.X == X && vec.Y == Y;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return "(" + X.ToString() + "; " + Y.ToString() + ")";
        }

        public bool Equals(IntVector other) => this == other;

        public static bool operator ==(IntVector left, IntVector right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(IntVector left, IntVector right) => !(left == right);

        public static IntVector operator +(IntVector left, IntVector right)
        {
            return new IntVector(left.X + right.X, left.Y + right.Y);
        }

        public static IntVector operator -(IntVector left, IntVector right)
        {
            return new IntVector(left.X - right.X, left.Y - right.Y);
        }

        public static IntVector operator -(IntVector vector)
        {
            return new IntVector(-vector.X, -vector.Y);
        }

        public static explicit operator Point(IntVector vector)
        {
            return new Point(vector.X, vector.Y);
        }

        public static explicit operator IntVector(Point point)
        {
            return new IntVector(Convert.ToInt32(point.X), Convert.ToInt32(point.Y));
        }

        public static explicit operator Vector(IntVector vector)
        {
            return new Vector(vector.X, vector.Y);
        }

        public static explicit operator IntVector(Vector vector)
        {
            return new IntVector(Convert.ToInt32(vector.X), Convert.ToInt32(vector.Y));
        }
    }
}
