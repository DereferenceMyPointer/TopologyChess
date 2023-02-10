﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using static System.Math;

namespace TopologyChess
{
    public static class Equations
    {
        public static double R { get; set; } = 1;

        public static Func<double, double, Point3D> Tilt(Func<double, double, Point3D> func)
        {
            return (u, v) =>
            {
                Point3D p = func(v, 1 - u);
                return new Point3D(p.X, -p.Z, p.Y);
            };
        }
        
        public static Point3D Flat(double u, double v) => new(
            0, 1 - 2 * u, 1 - 2 * v
        );

        public static Point3D Globe(double u, double v) => new(
            Sin(PI * v) * Cos(Tau * u),
            Sin(PI * v) * Sin(Tau * u),
            Cos(PI * v)
        );

        public static Point3D Cylinder(double u, double v) => new(
            R * Cos(Tau * u),
            R * Sin(Tau * u),
            1 - 2 * v
        );

        public static Point3D Moebius(double u, double v) => new(
            (R + (1 - 2 * v) * Sin(PI * u)) * Cos(Tau * u),
            (R + (1 - 2 * v) * Sin(PI * u)) * Sin(Tau * u),
            (1 - 2 * v) * Cos(PI * u)
        );

        public static Point3D Torus(double u, double v) => new(
            (R - 0.5 * Cos(Tau * v)) * Cos(Tau * u),
            (R - 0.5 * Cos(Tau * v)) * Sin(Tau * u),
            0.5 * Sin(Tau * v)
        );

        public static Point3D ProjectivePlaneNo(double u, double v) => new(
            Sqrt(1 + u * u + v * v) * u,
            Sqrt(1 + u * u + v * v) * v,
            0
        );

        public static Point3D ProjectivePlane(double u, double v)
        {
            double x = 2 * u - 1;
            double y = 1 - 2 * v;

            double xc = x * Sqrt(1 - y * y / 2);
            double yc = y * Sqrt(1 - x * x / 2);

            double theta = Sqrt(xc * xc + yc * yc) * PI / 2;
            double phi = Atan2(yc, xc);

            return new Point3D(
                (Cos(theta) * Cos(theta) - Cos(phi) * Cos(phi) * Sin(theta) * Sin(theta)),
                Cos(phi) * Sin(2 * theta),
                0.5 * Sin(phi) * Sin(2 * theta)
            );
        }

        public static Point3D Bicone(double u, double v)
        {
            double h = u - v;
            if (Abs(h) == 1) return new Point3D(0, 0, h);
            double phi = PI * (u + v - 1) / (1 - Abs(h));
            return new Point3D(-(1 - Abs(h)) * Cos(phi), -(1 - Abs(h)) * Sin(phi), h);
        }

        public static Point3D SphereL(double u, double v)
        {
            double h = u - v;
            if (Abs(h) == 1) return new Point3D(0, 0, h);
            double phi = PI * (u + v - 1) / (1 - Abs(h));
            double theta = h * PI / 2;
            return new Point3D(-Cos(theta) * Cos(phi), -Cos(theta) * Sin(phi), Sin(theta));
        }

        public static Point3D SphereL2(double u, double v)
        {
            double h = u - v;
            double y = Abs(h);
            double theta = h * PI / 2;
            if (y == 1) return new Point3D(0, 0, h);
            double x = u + v - 1;
            double phi = PI * x;
            if (y != 0) phi += PI * Sign(x) * y * Pow(Abs(x) / (1 - y), 1 / y + 1);
            return new Point3D(-Cos(theta) * Cos(phi), -Cos(theta) * Sin(phi), Sin(theta));
        }

        public static Point3D SphereL3(double u, double v)
        {
            double h = u - v;
            if (Abs(h) == 1) return new Point3D(0, 0, h);
            double theta = h * PI / 2;
            double g = Abs(u + v - 1);
            double phi = PI * Sign(u + v - 1);
            if (!(u == 0 || v == 0 || u == 1 || v == 1))
                phi *= 1 - Pow(Pow(1 - g, 1 / g) - Pow(Abs(h), 1 / g), g);
            return new Point3D(-Cos(theta) * Cos(phi), -Cos(theta) * Sin(phi), Sin(theta));
        }

        public static Point3D Pillow(double u, double v) => new(
            Cos(Tau * u) * 0.5,
            Sin(Tau * u) * Sqrt(v * (1 - v)),
            1 - 2 * v
        );

        static readonly double a = 1, b = 0.5;
        static readonly double c = 0.3, d = 0.5;
        public static Point3D Klein(double u, double v)
        {
            double phi = Tau * u;
            double theta = Tau * v;

            Point alpha = new Point(
                -a * Cos(phi),
                b * Sin(phi) * (1 - Cos(phi))
            );
            double r = c - d * (2 * u - 1) * Sqrt(u * (1 - u));
            Vector n;
            if (phi == 0) n = new Vector(0, 1);
            else if (phi == Tau) n = new Vector(0, -1);
            else
            {
                n = new Vector(
                    b * (Cos(2 * phi) - Cos(phi)),
                    a * Sin(phi)
                );
                n.Normalize();
            }

            return new Point3D(
                r * Sin(theta + phi / 2),
                alpha.Y + r * n.Y * Cos(theta + phi / 2),
                alpha.X + r * n.X * Cos(theta + phi / 2)
            );
        }
    }
}
