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

            MainCam = new Camera(0,0,-10);

            Mesh = new Mesh3D(loc);

            Renderer.AddOriginPoint(ProjectTo2D(Mesh.Origin));
            foreach (var vertex in Mesh.Vertecies)
            {
                Renderer.AddPoint(ProjectTo2D(vertex));
            }
            foreach (var line in Mesh.Lines)
            {
                Renderer.AddLine(ProjectTo2D(Mesh.Vertecies[line[0]]), ProjectTo2D(Mesh.Vertecies[line[1]]));
            }
        }
        public static void Refresh(string loc)
        {
            //Mesh = new Mesh3D(loc);
            Renderer.ClearCanvas();
            Renderer.AddOriginPoint(ProjectTo2D(Mesh.Origin));
            foreach (var vertex in Mesh.Vertecies)
            {
                Renderer.AddPoint(ProjectTo2D(vertex));
            }
            foreach (var line in Mesh.Lines)
            {
                Renderer.AddLine(ProjectTo2D(Mesh.Vertecies[line[0]]), ProjectTo2D(Mesh.Vertecies[line[1]]));
            }
        }

        private static Vector3 ProjectTo2D(Vector3 point)
        {
            float F = point.Z - MainCam.Location.Z;
            return new Vector3((point.X - MainCam.Location.X) * (F/point.Z) + MainCam.Location.X,
                                (point.Y - MainCam.Location.Y) * (F/ point.Z) + MainCam.Location.Y,
                                point.Z);
        }

        public static void PerspectiveTo(bool isPerspective)
        { 
            IsPerspective = isPerspective;
        }

        public static void ScaleMesh(float scale)
        {
            Mesh.ScaleMesh(scale, scale, scale);
        }

        public static void RotateMesh(int degree)
        {
            Mesh.RotateX(degree);
        }

    }
}
