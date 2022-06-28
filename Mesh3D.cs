using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_visualizer
{
    internal class Mesh3D
    {
        public Vector3 Origin { get; private set; }
        public List<Vector3> DefVertecies { get; private set; }
        public List<Vector3> Vertecies { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Scale { get; private set; }
        public List<int[]> Lines { get; private set; }

        public Mesh3D(string loc)
        {
            DefVertecies = new List<Vector3>();
            Vertecies = new List<Vector3>();
            Lines = new List<int[]>();

            Rotation = new Vector3(0, 0, 0);
            Scale = new Vector3(1, 1, 1);

            StreamReader sr = File.OpenText(loc);

            while (!sr.EndOfStream)
            {
                string[] hlpr = sr.ReadLine().Trim().Split(';');

                if (hlpr[0] != "")
                {
                    if (hlpr[0][0] == 'O')
                    {
                        hlpr[0] = hlpr[0].Trim('O');
                        Origin = new Vector3(Convert.ToInt32(hlpr[0]) ,  Convert.ToInt32(hlpr[1]) , Convert.ToInt32(hlpr[2]));
                    }
                    else if (hlpr[0][0] == 'P')
                    {
                        hlpr[0] = hlpr[0].Trim('P');
                        DefVertecies.Add( new Vector3(Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]), Convert.ToInt32(hlpr[2])));
                    }
                    else if (hlpr[0][0] == 'L')
                    {
                        hlpr[0] = hlpr[0].Trim('L');
                        Lines.Add(new int[2] { Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]) });
                    }
                }
            }

            for (int i = 0; i < DefVertecies.Count; i++)
            {
                Vertecies.Add(new Vector3(DefVertecies[i].X, DefVertecies[i].Y, DefVertecies[i].Z));
            }
        }
        private float DistanceBetween(Vector2 p1, Vector2 p2) => (float) Math.Sqrt(Math.Pow(p1.X - p2.X,2) * Math.Pow(p1.X - p2.X,2));
        public void RotateX(int degree)
        {
            Rotation = new Vector3(degree,Rotation.Y,Rotation.Z);
        }

        public void ScaleMesh(float x ,float y,float z)
        {
            Scale = new Vector3(x,y,z);
            for (int i = 0; i < Vertecies.Count; i++)
            {
                Vertecies[i] = new Vector3(DefVertecies[i].X * Scale.X, DefVertecies[i].Y * Scale.Y, DefVertecies[i].Z * Scale.Z);
            }
        }

    }
}
