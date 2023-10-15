using System.Linq;
using _3D_visualizer.Objects;

namespace _3D_visualizer
{
    internal class Logics
    {
        private static bool displayVerts;
        private static bool displayLines;
        private static bool displayFaces;

        private static Camera MainCam;
        private static Mesh3D Mesh;


        #region Logic
        public static void LoadModel(string loc)
        {
            displayVerts = false;
            displayLines = true;
            displayFaces = false;

            MainCam = new Camera(-4, 0,0);

            Mesh = new Mesh3D(loc,MainCam);

            if (loc.Split('.').Last() == "obj") ScaleMesh(10);
            else ScaleMesh(1);

            Refresh();
            Renderer.AddMeshInfo(Mesh);
        }
        
        public static void Refresh()
        {
            Renderer.ClearCanvas();
            if (displayFaces)
            {
                foreach (var face in Mesh.Faces)
                {
                    Renderer.AddFace(face);
                }            
            }
            if (displayVerts)
            {
                int num = 0;
                foreach (var vertex in Mesh.Vertecies)
                {
                    num++;
                    Renderer.AddPoint(vertex);
                }
            }
            if (displayLines)
            {
                if (Mesh.Faces.Count == 0)
                {
                    foreach (var line in Mesh.Lines) Renderer.AddLine(new Point3D[] { Mesh.Vertecies[line[0]], Mesh.Vertecies[line[1]] });
                }
                else foreach (var face in Mesh.Faces) Renderer.AddLine(face);
                

            }
            Renderer.AddPoint(Mesh.Origin);
            Renderer.Render();
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
            if (x != null) Mesh.RotateX((float)x);
            else if (y != null) Mesh.RotateY((float)y);
            else Mesh.RotateZ((float)z);

            Refresh();
        }

        #endregion

        #region Move



        #endregion

        #region Get
        public static Point3D GetOrigin() => Mesh.Origin;
        public static Mesh3D GetMesh() => Mesh;
        public static bool GetIsPerspective() => MainCam.GetPerspective();
        #endregion

        #region Set

        public static void PerspectiveTo(bool isPerspective)
        {
            MainCam.SetPerspective(isPerspective);
            Mesh.Refresh();
            Refresh();
        }

        public static void DisplayVerts(bool dispVerts)
        {
            displayVerts = dispVerts;
            Refresh();
        }

        public static void DisplayLines(bool dispLines)
        {
            displayLines = dispLines;
            Refresh();
        }

        public static void DisplayFaces(bool dispFaces)
        {
            displayFaces = dispFaces;
            Refresh();
        }
        
        #endregion
    }
}
