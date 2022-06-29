using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3D_visualizer
{
    internal class Point3D
    {
        public Vector3 DefVertex { get; private set; }
        public Vector3 Vertex { get; private set; }
        public Vector3 DefRotation { get; private set; }
        public Vector3 Rotation { get; private set; }
        public Point3D(float x, float y, float z, Vector3 origin)
        {
            DefVertex = new Vector3(x, y, z);
            Vertex = new Vector3(DefVertex.X, DefVertex.Y, DefVertex.Z);

            float X, Y, Z;
            X = DistanceBetween(origin.Z, origin.Y, DefVertex.Z, DefVertex.Y) * (float)Math.Sin(x);
            Y = DistanceBetween(origin.Z, origin.X, DefVertex.Z, DefVertex.X) * (float)Math.Cos(y);
            Z = DistanceBetween(origin.X, origin.Y, DefVertex.X, DefVertex.Y) * (float)Math.Sin(z);

            DefRotation = new Vector3(X, Y, Z);
            Rotation = new Vector3(X, Y, Z);
        }
        private float DistanceBetween(Vector3 p1, Vector3 p2) => (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2));
        private float DistanceBetween(float p1x, float p1y, float p2x, float p2y) => (float)Math.Sqrt(Math.Pow(p2x - p1x, 2) + Math.Pow(p2y - p1y, 2));

        public void ChangeLocation(Vector3 loc)
        {
            Vertex = new Vector3(loc.X,loc.Y,loc.Z);
        }
        public void ChangeRotation(Vector3 rot, Vector3 origin)
        {
            Rotation = new Vector3(DefRotation.X + rot.X,DefRotation.Y + rot.Y,DefRotation.Z + rot.Z);

            float X, Y, Z;
            X = DistanceBetween(origin.X, origin.Y, DefVertex.X, DefVertex.Y) * (float)Math.Sin(Rotation.Z);
            Y = DistanceBetween(origin.X, origin.Y, DefVertex.X, DefVertex.Y) * (float)Math.Cos(Rotation.Z);
            Z = Vertex.Z;
            //if (rot.X != 0) 
            //{
            //    X = DistanceBetween(origin.Z, origin.Y, DefVertex.Z, DefVertex.Y) * (float)Math.Sin(Rotation.X); 
            //}
            //else X = Vertex.X;
            //if (rot.Y != 0) Y = DistanceBetween(origin.Z, origin.X, DefVertex.Z, DefVertex.X) * (float)Math.Cos(Rotation.Y);
            //else Y = Vertex.Y;
            //if (rot.Z != 0)
            //{
            //    X = DistanceBetween(origin.X,origin.Y,DefVertex.X,DefVertex.Y) * (float)Math.Sin(rot.Z);
            //    Y = DistanceBetween(origin.X, origin.Y, DefVertex.X, DefVertex.Y) * (float)Math.Cos(rot.Z);
            //    Z = Vertex.Z;
            //}
            //else Z = Vertex.Z;

            Vertex = new Vector3(X, Y, Z);
        }
    }
}