using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace TopologyChess
{
    public static class TopologyModel
    {
        public static MeshGeometry3D GenerateLattice(int n)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            double x, y;
            for (int i = 0; i <= n; i++)
            {
                for (int j = 0; j <= n; j++)
                {
                    x = (double)i / n;
                    y = (double)j / n;
                    mesh.Positions.Add(new Point3D(x, y, 0.0));
                    mesh.TextureCoordinates.Add(new Point(x, y));
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    mesh.TriangleIndices.Add((n + 1) * i + j);
                    mesh.TriangleIndices.Add((n + 1) * i + j + 1);
                    mesh.TriangleIndices.Add((n + 1) * (i + 1) + j + 1);

                    mesh.TriangleIndices.Add((n + 1) * i + j);
                    mesh.TriangleIndices.Add((n + 1) * (i + 1) + j);
                    mesh.TriangleIndices.Add((n + 1) * (i + 1) + j + 1);
                }
            }

            return mesh;
        }

        public static Point3DCollection GetBorder(MeshGeometry3D mesh)
        {
            Point3DCollection points = new Point3DCollection();
            int n = (int)Math.Sqrt(mesh.Positions.Count);
            for (int j = 0; j < n - 1; j++)
            {
                points.Add(mesh.Positions[j]);
            }
            for (int i = n - 1; i < (n * n) - 1; i += n)
            {
                points.Add(mesh.Positions[i]);
            }
            for (int j = (n * n) - 1; j > n * (n - 1); j--)
            {
                points.Add(mesh.Positions[j]);
            }
            for (int i = n * (n - 1); i >= 0; i -= n)
            {
                points.Add(mesh.Positions[i]);
            }
            return points;
        }

        public static void Transform(MeshGeometry3D mesh, Func<Point, Point3D> transform)
        {
            mesh.Positions = new Point3DCollection(mesh.TextureCoordinates.Select(transform));
        }
    }
}
