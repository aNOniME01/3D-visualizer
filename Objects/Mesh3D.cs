using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;

namespace _3D_visualizer.Objects
{
    internal class Mesh3D
    {
        public Point3D Origin { get; private set; }
        public List<Point3D> Vertecies { get; private set; }
        public List<int[]> Lines { get; private set; }
        public List<Face3D> Faces { get; private set; }
        public List<Vector3> FaceNormals { get; private set; }

        public Vector3 Scale { get; private set; }


        #region Constructor

        public Mesh3D(string loc, Camera mainCam)
        {
            Vertecies = new List<Point3D>();
            Lines = new List<int[]>();

            Faces = new List<Face3D>();
            FaceNormals = new List<Vector3>();

            Scale = new Vector3(1, 1, 1);

            if (loc.Split('.').Last() == "obj")
            {
                ReadInFromObj(loc, mainCam);
            }
            else
            {
                ReadInFromTxt(loc, mainCam);
            }

            OrderByX();
        }
        private void ReadInFromObj(string loc, Camera mainCam)
        {
            StreamReader sr = File.OpenText(loc);

            int index = 0;

            while (!sr.EndOfStream)
            {
                string[] hlpr = sr.ReadLine().Trim().Split(' ');

                Origin = new Point3D(0, 0, 0, mainCam);
                if (hlpr[0] != "")
                {
                    if (hlpr[0] == "v")
                    {
                        float X = 0;
                        float Y = 0;
                        float Z = 0;
                        int i = 1;
                        bool isStay = true;
                        while (isStay && i < hlpr.Length)
                        {
                            if (hlpr[i] != "")
                            {
                                X = float.Parse(hlpr[i],CultureInfo.InvariantCulture);
                                Y = float.Parse(hlpr[i + 1],CultureInfo.InvariantCulture);
                                Z = float.Parse(hlpr[i + 2], CultureInfo.InvariantCulture);
                                isStay = false;
                            }
                            i++;
                        }
                        Vertecies.Add(new Point3D(Z, X, Y, Origin, index, mainCam));
                        index++;
                    }
                    else if (hlpr[0] == "f")
                    {
                        List<Point3D> face = new List<Point3D>();

                        for (int i = 1; i < hlpr.Length; i++)
                        {
                            if (i == hlpr.Length - 1)
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

                            face.Add(Vertecies[Convert.ToInt32(hlpr[i].Split('/')[0]) - 1]);

                        }

                        Faces.Add(new Face3D(face));

                    }
                    else if (hlpr[0] == "l")
                    {
                        Lines.Add(new int[2] { Convert.ToInt32(hlpr[1]) - 1, Convert.ToInt32(hlpr[2]) - 1 });
                    }
                    else if (hlpr[0] == "vn")
                    {
                        FaceNormals.Add(new Vector3(float.Parse(hlpr[1], CultureInfo.InvariantCulture), float.Parse(hlpr[2], CultureInfo.InvariantCulture), float.Parse(hlpr[3], CultureInfo.InvariantCulture)));
                    }
                }
            }
        }
        private void ReadInFromTxt(string loc, Camera mainCam)
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
                        Origin = new Point3D(Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]), Convert.ToInt32(hlpr[2]), mainCam);
                    }
                    else if (hlpr[0][0] == 'P')
                    {
                        hlpr[0] = hlpr[0].Trim('P');
                        Vertecies.Add(new Point3D(Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]), Convert.ToInt32(hlpr[2]), Origin, index, mainCam));
                        index++;
                    }
                    else if (hlpr[0][0] == 'L')
                    {
                        hlpr[0] = hlpr[0].Trim('L');
                        Lines.Add(new int[2] { Convert.ToInt32(hlpr[0]), Convert.ToInt32(hlpr[1]) });
                    }
                }
            }
            sr.Close();

        }
        #endregion

        #region Checks
        private bool SameLineExist(int[] line)
        {
            foreach (var item in Lines)
            {
                if (item[0] == line[0] && item[1] == line[1] || item[0] == line[1] && item[1] == line[0]) return true;
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
                if (X != 0) vertex.ChangeXRotation(Origin);
                if (Y != 0) vertex.ChangeYRotation(Origin);
                if (Z != 0) vertex.ChangeZRotation(Origin);
            }

        }

        public void RotateX(float degree)
        {
            Origin.SetRotation(new Vector3(degree, Origin.Rotation.Y, Origin.Rotation.Z));

            ScaleMesh();

            foreach (var vertex in Vertecies)
            {
                vertex.ChangeXRotation2(Origin);
            }

            OrderByX();
        }

        public void RotateY(float degree)
        {
            Origin.SetRotation(new Vector3(Origin.Rotation.X, degree, Origin.Rotation.Z));

            ScaleMesh();

            foreach (var vertex in Vertecies)
            {
                vertex.ChangeYRotation(Origin);
            }

            OrderByX();
        }

        public void RotateZ(float degree)
        {
            Origin.SetRotation(new Vector3(Origin.Rotation.X, Origin.Rotation.Y, degree));

            ScaleMesh();

            foreach (var vertex in Vertecies)
            {
                vertex.ChangeZRotation(Origin);
            }

            OrderByX();
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
                Point3D vert = Vertecies[i];

                float xDistance = Point3D.DistanceBetween(Origin.Location.X, vert.DefLocation.X);
                float yDistance = Point3D.DistanceBetween(Origin.Location.Y, vert.DefLocation.Y);
                float zDistance = Point3D.DistanceBetween(Origin.Location.Z, vert.DefLocation.Z);
                float xScale, yScale, zScale;

                if (vert.DefLocation.X < Origin.Location.X) xScale = -(xDistance * Scale.X - xDistance);
                else xScale = xDistance * Scale.X - xDistance;

                if (vert.DefLocation.Y < Origin.Location.Y) yScale = -(yDistance * Scale.Y - yDistance);
                else yScale = yDistance * Scale.Y - yDistance;

                if (vert.DefLocation.Z < Origin.Location.Z) zScale = -(zDistance * Scale.Z - zDistance);
                else zScale = zDistance * Scale.Z - zDistance;

                xScale = (vert.Location.X - (vert.DefLocation.X + xScale)) * -1;
                yScale = (vert.Location.Y - (vert.DefLocation.Y + yScale)) * -1;
                zScale = (vert.Location.Z - (vert.DefLocation.Z + zScale)) * -1;

                Vertecies[i].ChangeLocation(new Vector3(Vertecies[i].Location.X + xScale, Vertecies[i].Location.Y + yScale, Vertecies[i].Location.Z + zScale));
            }

        }
        #endregion


        #region Operations

        private void OrderByX()
        {
            foreach (var face in Faces) face.GetAvgX();

            var clonedList = new List<Face3D>(Faces.Count);
            var normalList = new List<Vector3>(FaceNormals.Count);

            for (int i = 0; i < Faces.Count; i++)
            {
                var currentIndex = i;

                while (currentIndex > 0 && clonedList[currentIndex - 1].AvgX < Faces[i].AvgX)
                {
                    currentIndex--;
                }

                clonedList.Insert(currentIndex, Faces[i]);
                //normalList.Insert(currentIndex, FaceNormals[i]);
            }

            Faces = clonedList;
            FaceNormals = normalList;
        }

        #endregion

        public void Refresh()
        {
            foreach (var vert in Vertecies)
            {
                vert.ProjectTo2D();
            }
        }
    }
}
