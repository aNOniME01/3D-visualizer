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

        private static float[,] Perspective;
        private static float[,] Fov;
        private static float[,] RotX;
        private static float[,] RotY;
        private static float[,] RotZ;
        private static float[,] Loc;
        public static void Load(string loc)
        {
            IsPerspective = false;

            MainCam = new Camera(0,0,0);

            Mesh = new Mesh3D(loc);

            LoadMatrixes();

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
        public static void Refresh(string loc, int degree)
        {
            //Mesh = new Mesh3D(loc);
            Mesh.RotateX(degree);
            LoadMatrixes();
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

        private static void LoadMatrixes()
        {
            Fov = new float[,]
            {
                {(MainCam.FocalLength * MainCam.ImageRes.X)  / (2* MainCam.SenzorSize.X),MainCam.Skew,0,0 },
                {0,(MainCam.FocalLength * MainCam.ImageRes.Y) /  (2* MainCam.SenzorSize.Y),0,0},
                {0,-1,1,0 },
                {0,0,0,1 }
            };

            RotX = new float[,]
            {
                {1,0,0,0},
                {0, (float)Math.Cos(Mesh.Rotation.X), -(float)Math.Sin(Mesh.Rotation.X),0 },
                {0,(float)Math.Sin(Mesh.Rotation.X),(float)Math.Cos(Mesh.Rotation.X),0 },
                {0,0,0,1 }
            };

            RotY = new float[,]
            {
                {(float)Math.Cos(Mesh.Rotation.Y), -(float)Math.Sin(Mesh.Rotation.Y),0,0},
                {0,1,0,0},
                {-(float)Math.Sin(Mesh.Rotation.Y),(float)Math.Cos(Mesh.Rotation.Y),1,0 },
                {0,0,0,1}
            };

            RotZ = new float[,]
            {
                {(float)Math.Cos(Mesh.Rotation.Z), -(float)Math.Sin(Mesh.Rotation.Z),0,0 },
                {(float)Math.Sin(Mesh.Rotation.Z),(float)Math.Cos(Mesh.Rotation.Z),0,0},
                {0,0,1,0 },
                {0,0,0,1 }
            };

            Loc = new float[,]
            {
                {1,0,0,-MainCam.Location.X},
                {0,1,0,-MainCam.Location.Y},
                {0,0,1,-MainCam.Location.Z},
                {0,0,0,1 }
            };
        }

        private static float[,] ProjectTo2D(float[,] point)
        {
            float[,] projectionMatrix = MultiplyMatrix(MultiplyMatrix(MultiplyMatrix(MultiplyMatrix(MultiplyMatrix(Fov, RotX), RotY), RotZ),Loc), point);
            if (IsPerspective)
            {
                Perspective = new float[,]
                {
                    {1/point[2,0],0,0,0},
                    {0,1/point[2,0],0,0 },
                    {0,0,1,0 },
                    {0,0,0,1 }
                };

                projectionMatrix = MultiplyMatrix(Perspective, projectionMatrix);
            }
            //float[,] projectionMatrix = MultiplyMatrix(Fov, RotX);
            //projectionMatrix = MultiplyMatrix(projectionMatrix, RotY);
            //projectionMatrix = MultiplyMatrix(projectionMatrix, RotZ);
            //projectionMatrix = MultiplyMatrix(projectionMatrix, Loc);
            //projectionMatrix = MultiplyMatrix(projectionMatrix, point);
            return projectionMatrix;
        }

        public static float[,] MultiplyMatrix(float[,] A, float[,] B)
        {
            int rA = A.GetLength(0);
            int cA = A.GetLength(1);
            int rB = B.GetLength(0);
            int cB = B.GetLength(1);
            float temp;
            float[,]? kHasil = null;
            if (cA == rB)
            {
                kHasil = new float[rA, cB];
                for (int i = 0; i < rA; i++)
                {
                    for (int j = 0; j < cB; j++)
                    {
                        temp = 0;
                        for (int k = 0; k < cA; k++)
                        {
                            temp += A[i, k] * B[k, j];
                        }
                        kHasil[i, j] = temp;
                    }
                }
            }
            return kHasil;
        }

        public static void PerspectiveTo(bool isPerspective)
        { 
            IsPerspective = isPerspective;
        }

    }
}
