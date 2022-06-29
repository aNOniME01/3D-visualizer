using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _3D_visualizer
{
    internal class Logics
    {
        private static bool IsPerspective;

        private static Camera MainCam;
        private static Mesh3D Mesh;

        public static void Load(string loc)
        {
            IsPerspective = false;

            MainCam = new Camera(-10, 0,0);

            Mesh = new Mesh3D(loc);

            ScaleMesh(1);

            Refresh();
        }
        public static void Refresh()
        {
            Renderer.ClearCanvas();
            foreach (var vertex in Mesh.Vertecies)
            {
                Renderer.AddPoint(vertex);
            }
            foreach (var line in Mesh.Lines)
            {
                Renderer.AddLine(ProjectTo2D(Mesh.Vertecies[line[0]].Vertex), ProjectTo2D(Mesh.Vertecies[line[1]].Vertex));
            }
            Renderer.AddOriginPoint(ProjectTo2D(Mesh.Origin));
        }

        public static Vector3 ProjectTo2D(Vector3 point)
        {
            float F = point.X - MainCam.Location.X;
            if (point.X == 0 && point.Y == 0 && point.Z == 0) return point;
            return new Vector3(point.X,
                                (point.Y - MainCam.Location.Y) * (F/ point.X),
                                (point.Z - MainCam.Location.X) * (F / point.X));
        }

        public static void PerspectiveTo(bool isPerspective)
        { 
            IsPerspective = isPerspective;
        }

        public static void ScaleMesh(float scale)
        {
            Mesh.SetScale(scale, scale, scale);
        }

        public static void RotateMesh(float x,float y, float z)
        {
            Mesh.Rotate(x,y,z);
            Refresh();
        }

    }
}
