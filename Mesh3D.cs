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
        public Point3D Origin { get; private set; }
        public List<Point3D> Vertecies { get; private set; }
        public List<int[]> Lines { get; private set; }
        public Vector3 Scale { get; private set; }


        #region Constructor
        public Mesh3D(string loc)
        {
            Vertecies = new List<Point3D>();
            Lines = new List<int[]>();

            Scale = new Vector3(1, 1, 1);

            if (loc.Split('.').Last() == "obj")
            {
                ReadInFromObj(loc);
            }
            else
            {
                ReadInFromTxt(loc);
            }
        }
        private void ReadInFromObj(string loc)
        {
            StreamReader sr = File.OpenText(loc);

            int index = 0;

            while (!sr.EndOfStream)
            {
                string[] hlpr = sr.ReadLine().Trim().Split(' ');

                Origin = new Point3D(0, 0, 0);
                if (hlpr[0] != "")
                {
                    if (hlpr[0] == "v")
                    {
                        Vertecies.Add(new Point3D((float)Convert.ToDouble(hlpr[1]), (float)Convert.ToDouble(hlpr[2]), (float)Convert.ToDouble(hlpr[3]), Origin,index));
                        index++;
                    }
                    else if (hlpr[0] == "f")
                    {
                        for (int i = 1; i < hlpr.Length; i++)
                        {
                            if (i == hlpr.Length-1)
                            {
                                int[] lnNew = new int[2] {  Convert.ToInt32(hlpr[1].Split('/').First()) - 1,
                                                            Convert.ToInt32(hlpr[i].Split('/').First()) - 1 };

                                if (!SameLineExist(lnNew)) Lines.Add(lnNew);
                            }
                            else
                            {
                                int[] lnNew = new int[2] {  Convert.ToInt32(hlpr[i].Split('/').First()) - 1,
                                                            Convert.ToInt32(hlpr[i + 1].Split('/').First()) - 1 };

                                if (!SameLineExist(lnNew)) Lines.Add(lnNew);                                
                            }
                        }
                    }
                    else if (hlpr[0] == "l")
                    {
                        Lines.Add( new int[2] {  Convert.ToInt32(hlpr[1]) - 1, Convert.ToInt32(hlpr[2]) - 1 });
                    }
                }
            }
        }
        private void ReadInFromTxt(string loc)
        {
            StreamReader sr = File.OpenText(loc);

            while (!sr.EndOfStream)
            {
                string[] hlpr = sr.ReadLine().Trim().Split(';');

                int index = 0;
                if (hlpr[0] != "")
                {
                    if (hlpr[0][0] == 'O')
                    {
                        hlpr[0] = hlpr[0].Trim('O');
                        Origin = new Point3D(Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]), Convert.ToInt32(hlpr[2]));
                    }
                    else if (hlpr[0][0] == 'P')
                    {
                        hlpr[0] = hlpr[0].Trim('P');
                        Vertecies.Add(new Point3D(Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]), Convert.ToInt32(hlpr[2]), Origin,index));
                        index++;
                    }
                    else if (hlpr[0][0] == 'L')
                    {
                        hlpr[0] = hlpr[0].Trim('L');
                        Lines.Add(new int[2] { Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]) });
                    }
                }
            }

        }
        #endregion

        #region Checks
        private bool SameLineExist(int[] line)
        {
            foreach (var item in Lines)
            {
                if ((item[0] == line[0] && item[1] == line[1]) || (item[0] == line[1] && item[1] == line[0])) return true;
            }
            return false;
        }
        #endregion

        #region Rotate
        public void Rotate(float? x, float? y, float? z)
        {
            float X, Y, Z;
            if (x != null) X = (float)x;
            else X = Origin.Rotation.X;
            if (y != null) Y = (float)y;
            else Y = Origin.Rotation.Y;
            if (z != null) Z = (float)z;
            else Z = Origin.Rotation.Z;

            Origin.SetRotation(new Vector3(X, Y, Z));

            ScaleMesh();

            foreach (var vertex in Vertecies)
            {
                if(X != 0) vertex.ChangeXRotation(Origin);
                if(Y != 0) vertex.ChangeYRotation(Origin);
                if(Z != 0) vertex.ChangeZRotation(Origin);
                //vertex.ChangeYRoitation(Origin);
                //vertex.ChangeZRoitation(Origin);
                //if(Origin.Rotation.X != 0)vertex.ChangeXRotation1(Origin);
                //if (Origin.Rotation.Y != 0) vertex.ChangeYRoitation1(Origin);
                //if (Origin.Rotation.Z != 0) vertex.ChangeZRoitation1(Origin);
            }
        }
        #endregion

        #region Scale
        public void SetScale(float x, float y, float z)
        {
            Scale = new Vector3(x, y, z);
            ScaleMesh();
        }

        private void ScaleMesh()
        {

            for (int i = 0; i < Vertecies.Count; i++)
            {
                float xDistance = Point3D.DistanceBetween(Origin.Location.X, Vertecies[i].DefLocation.X);
                float yDistance = Point3D.DistanceBetween(Origin.Location.Y, Vertecies[i].DefLocation.Y);
                float zDistance = Point3D.DistanceBetween(Origin.Location.Z, Vertecies[i].DefLocation.Z);
                float xScale, yScale, zScale;

                if (Vertecies[i].DefLocation.X < Origin.Location.X) xScale = -(xDistance * Scale.X-xDistance);
                else xScale = (xDistance * Scale.X-xDistance);

                if (Vertecies[i].DefLocation.Y < Origin.Location.Y) yScale = -(yDistance * Scale.Y-yDistance);
                else yScale = (yDistance * Scale.Y-yDistance);

                if (Vertecies[i].DefLocation.Z < Origin.Location.Z) zScale = -(zDistance * Scale.Z-zDistance);
                else zScale = (zDistance * Scale.Z-zDistance);

                Vertecies[i].ChangeLocation(new Vector3(Vertecies[i].DefLocation.X + xScale, Vertecies[i].DefLocation.Y + yScale, Vertecies[i].DefLocation.Z + zScale));
            }

        }
        #endregion
    }
}
