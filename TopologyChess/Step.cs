using System;
using System.Windows;
using System.Windows.Media;

namespace TopologyChess
{
    public struct Step : IEquatable<Step>
    {
        public IntVector P { get; set; }
        public IntVector V { get; set; }
        public Matrix M { get; set; }

        public Step() { }
        public Step(IntVector p, IntVector v, Matrix m)
        {
            P = p; V = v; M = m;
        }

        public bool Equals(Step other) => P == other.P && V == other.V;

        public static bool operator ==(Step left, Step right) => left.Equals(right);

        public static bool operator !=(Step left, Step right) => !left.Equals(right);

        public override bool Equals(object obj) => obj is Step step && Equals(step);

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString()
        {
            return "P = " + P.ToString() + "\nV = " + V.ToString() + "\nM = " + M.ToString() + "\n\n";
        }
    }
}
