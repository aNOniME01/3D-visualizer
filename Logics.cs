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


        #region Logic
        public static void Load(string loc)
        {
            IsPerspective = false;

            MainCam = new Camera(-4, 0,0);

            Mesh = new Mesh3D(loc);

            if (loc.Split('.').Last() == "obj") ScaleMesh(10);
            else ScaleMesh(1);

            Refresh();
        }
        public static void Refresh()
        {
            Renderer.ClearCanvas();
            int num = 0;
            foreach (var vertex in Mesh.Vertecies)
            {
                num++;
                Renderer.AddPoint(vertex,num);
            }
            foreach (var line in Mesh.Lines)
            {
                Renderer.AddLine(ProjectTo2D(Mesh.Vertecies[line[0]]), ProjectTo2D(Mesh.Vertecies[line[1]]));
            }
            Renderer.AddOriginPoint(Mesh.Origin);
            Renderer.AddMeshInfo(Mesh);
        }
        #endregion

        #region Projection
        public static Vector3 ProjectTo2D(Point3D point)
        {
            if (IsPerspective)
            {
                float X, Y, Z;
                if (point.Location.X == 0 && point.Location.Y == 0 && point.Location.Z == 0)
                {
                    return point.Location;
                    //X = 1;
                    //Y = 1;
                    //Z = 1;
                }
                else
                {
                    X = point.Location.X;
                    Y = point.Location.Y;
                    Z = point.Location.Z;
                }

                float F = X - MainCam.Location.X;

                float yHlpr = ((Y - MainCam.Location.Y) * (F / X)) + MainCam.Location.X;
                if (yHlpr < float.MaxValue && yHlpr > float.MinValue) Y = yHlpr;
                float zHlpr = ((Z - MainCam.Location.Z) * (F / X)) + MainCam.Location.X;
                if (zHlpr < float.MaxValue && zHlpr > float.MinValue) Z = zHlpr;

                return new Vector3(X, Y, Z);
            }
            else
            {
                return new Vector3(point.Location.X, point.Location.Y, point.Location.Z);
            }
        }
        #endregion

        #region Scale
        public static void ScaleMesh(float scale)
        {
            Mesh.SetScale(scale, scale, scale);
        }
        #endregion

        #region Rotate
        public static void RotateMesh(float? x,float? y, float? z)
        {
            Mesh.Rotate(x,y,z);
            Refresh();
        }
        #endregion

        #region Get
        public static Point3D GetOrigin() => Mesh.Origin;
        public static Mesh3D GetMesh() => Mesh;
        public static bool GetIsPerspective() => IsPerspective;
        #endregion

        #region Set
        public static void PerspectiveTo(bool isPerspective)
        {
            IsPerspective = isPerspective;
            Refresh();
        }
        #endregion
    }
}
