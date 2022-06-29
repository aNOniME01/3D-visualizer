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
        public List<Point3D> Vertecies { get; private set; }
        public List<int[]> Lines { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Vector3 Scale { get; private set; }

        public Mesh3D(string loc)
        {
            Vertecies = new List<Point3D>();
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
                        Vertecies.Add(new Point3D(Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]), Convert.ToInt32(hlpr[2]),Origin));
                    }
                    else if (hlpr[0][0] == 'L')
                    {
                        hlpr[0] = hlpr[0].Trim('L');
                        Lines.Add(new int[2] { Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]) });
                    }
                }
            }

        }
        private float DistanceBetween(Vector3 p1, Vector3 p2) => (float) Math.Sqrt(Math.Pow(p2.X - p1.X,2) + Math.Pow(p2.Y - p1.Y,2) + Math.Pow(p2.Z - p1.Z,2));
        public void Rotate(float x, float y, float z)
        {
            Rotation = new Vector3(x,y,z);

            foreach (var vertex in Vertecies)
            {
                vertex.ChangeRotation(Rotation,Origin);
            }
        }

        public void SetScale(float x, float y, float z)
        {
            Scale = new Vector3(x, y, z);
            ScaleMesh();
        }

        private void ScaleMesh()
        {

            for (int i = 0; i < Vertecies.Count; i++)
            {
                float distance = DistanceBetween(Origin, Vertecies[i].DefVertex);
                float xScale, yScale, zScale;

                if (Vertecies[i].DefVertex.X < Origin.X) xScale = -(distance * Scale.X);
                else xScale = (distance * Scale.X);

                if (Vertecies[i].DefVertex.Y < Origin.Y) yScale = -(distance * Scale.Y);
                else yScale = (distance * Scale.Y);

                if (Vertecies[i].DefVertex.Z < Origin.Z) zScale = -(distance * Scale.Z);
                else zScale = (distance * Scale.Z);

                Vertecies[i].ChangeLocation(new Vector3(Vertecies[i].DefVertex.X + xScale, Vertecies[i].DefVertex.Y + yScale, Vertecies[i].DefVertex.Z + zScale));
            }

        }

    }
}
