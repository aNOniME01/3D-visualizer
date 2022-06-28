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
        public float[,] Origin { get; private set; }
        public List<float[,]> Vertecies { get; private set; }
        public Vector3 Rotation { get; private set; }
        public List<int[]> Lines { get; private set; }

        public Mesh3D(string loc)
        {
            Vertecies = new List<float[,]>();
            Lines = new List<int[]>();
            StreamReader sr = File.OpenText(loc);
            Rotation = new Vector3(0,0,0);
            while (!sr.EndOfStream)
            {
                string[] hlpr = sr.ReadLine().Trim().Split(';');

                if (hlpr[0] != "")
                {
                    if (hlpr[0][0] == 'O')
                    {
                        hlpr[0] = hlpr[0].Trim('O');
                        Origin = new float[4, 1] { { (float)Convert.ToInt32(hlpr[0]) }, { (float)Convert.ToInt32(hlpr[1]) }, { (float)Convert.ToInt32(hlpr[2]) }, { 1 } };
                    }
                    else if (hlpr[0][0] == 'P')
                    {
                        hlpr[0] = hlpr[0].Trim('P');
                        Vertecies.Add(new float[4, 1] { { (float)Convert.ToInt32(hlpr[0]) }, { (float)Convert.ToInt32(hlpr[1]) }, {(float)Convert.ToInt32(hlpr[2]) },{1}});
                    }
                    else if (hlpr[0][0] == 'L')
                    {
                        hlpr[0] = hlpr[0].Trim('L');
                        Lines.Add(new int[2] { Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]) });
                    }
                }
            }
        }
        private static float DistanceBetween(Vector2 p1, Vector2 p2) => (float) Math.Sqrt(Math.Pow(p1.X - p2.X,2) * Math.Pow(p1.X - p2.X,2));
        public void RotateX(int degree)
        {
            Rotation = new Vector3(degree,Rotation.Y,Rotation.Z);
        }

    }
}
