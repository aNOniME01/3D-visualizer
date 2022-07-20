using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _3D_visualizer
{
    internal class Point3D
    {
        public int VertIndex { get; private set; }

        public Vector3 DefLocation { get; private set; }
        public Vector3 Location { get; private set; }

        public Vector3 QuadrantAngle { get; private set; }
        public Vector3 Rotation { get; private set; }

        public Rectangle Body { get; private set; }
        public bool IsInfoDisplayed { get; private set; }


        #region Constructor
        public Point3D(float x, float y, float z, Point3D origin,int index)
        {
            this.VertIndex = index;

            DefLocation = new Vector3(x, y, z);
            Location = new Vector3(DefLocation.X, DefLocation.Y, DefLocation.Z);


            float X, Y, Z;
            float xAngle = ToPositiveDegree((float)(Math.Atan2(y - origin.Location.Y, z - origin.Location.Z) * (180 / Math.PI)));
            if (xAngle <= 90) X = 0;
            else if (xAngle < 180) X = 270;
            else if (xAngle <= 270) X = 180;
            else X = 90;

            float yAngle = ToPositiveDegree((float)(Math.Atan2(x - origin.Location.X, z - origin.Location.Z) * (180 / Math.PI)));
            if (yAngle <= 90) Y = 0;
            else if (yAngle < 180) Y = 270;
            else if (yAngle <= 270) Y = 180;
            else Y = 90;

            float zAngle = ToPositiveDegree(Flip((float)(Math.Atan2(y - origin.Location.Y, x - origin.Location.X) * (180 / Math.PI))));
            if (zAngle <= 90) Z = 270;
            else if (zAngle < 180) Z = 0;
            else if (zAngle <= 270) Z = 90;
            else Z = 180;

            QuadrantAngle = new Vector3(X, Y, Z);
            Rotation = new Vector3(X, Y, Z);

            Body = new Rectangle();
            Body.Height = 5;
            Body.Width = 5;
            Body.Fill = Brushes.LimeGreen;

            IsInfoDisplayed = false;

        }
        public Point3D(float x, float y, float z)
        {
            VertIndex = 0;

            DefLocation = new Vector3(x, y, z);
            Location = new Vector3(DefLocation.X, DefLocation.Y, DefLocation.Z);


            float X, Y, Z;

            X = 0;
            Y = 0;
            Z = 0;

            QuadrantAngle = new Vector3(X, Y, Z);
            Rotation = new Vector3(X, Y, Z);

            Body = new Rectangle();
            Body.Height = 5;
            Body.Width = 5;
            Body.Fill = Brushes.Red;

            IsInfoDisplayed = false;
        }
        #endregion

        #region Location
        public void ChangeLocation(Vector3 loc)
        {
            Location = new Vector3(loc.X,loc.Y,loc.Z);
        }
        #endregion

        #region Rotation
        public void ChangeXRotation1(Point3D origin)
        {
            Rotation = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);

            float X, Y, Z;
            X = Location.X;
            Y = DistanceBetween(origin.Location.Y,Location.Y) * (float)Math.Cos(AngleToRadians(Rotation.X));
            Z = DistanceBetween(origin.Location.Z,Location.Z) * (float)Math.Sin(AngleToRadians(Rotation.X));

            Location = new Vector3(X, Y, Z);
        }
        public void ChangeXRotation(Point3D origin)
        {
            Rotation = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);

            float disY, disZ;
            disY = DistanceBetween(origin.Location.Y, Location.Y);
            disZ = DistanceBetween(origin.Location.Z, Location.Z);

            float sinX, cosX;
            sinX = (float)Math.Sin(AngleToRadians(Rotation.X));
            cosX = (float)Math.Cos(AngleToRadians(Rotation.X));

            float X, Y, Z;
            X = Location.X;
            Y = disY * cosX - disZ * sinX;
            Z = disZ * cosX + disY * sinX;

            Location = new Vector3(X, Y, Z);
        }
        public void ChangeYRotation1(Point3D origin)
        {
            Rotation = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);
            
            float X, Y, Z;
            X = DistanceBetween(origin.Location.X, Location.X) * (float)Math.Cos(AngleToRadians(Rotation.Y));
            Y = Location.Y;
            Z = DistanceBetween(origin.Location.Z, Location.Z) * (float)Math.Sin(AngleToRadians(Rotation.Y));
            
            Location = new Vector3(X, Y, Z);
        }
        public void ChangeYRotation(Point3D origin)
        {
            Rotation = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);
            
            float disX, disZ;
            disX = DistanceBetween(origin.Location.X, Location.X);
            disZ = DistanceBetween(origin.Location.Z, Location.Z);

            float sinY, cosY;
            sinY = (float)Math.Sin(AngleToRadians(Rotation.Y));
            cosY = (float)Math.Cos(AngleToRadians(Rotation.Y));

            float X, Y, Z;
            X = disZ * cosY - disX * sinY;
            Y = Location.Y;
            Z = disX * cosY + disZ * sinY;
            
            Location = new Vector3(X, Y, Z);
        }
        public void ChangeZRotation1(Point3D origin)
        {
            
            Rotation = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);
            
            float X, Y, Z;
            X = DistanceBetween(origin.Location.X, Location.X) * (float)Math.Sin(AngleToRadians(Rotation.Z));
            Y = DistanceBetween(origin.Location.Y, Location.Y) * (float)Math.Cos(AngleToRadians(Rotation.Z));
            Z = Location.Z;
            Location = new Vector3(X, Y, Z);
        }
        public void ChangeZRotation(Point3D origin)
        {
            
            Rotation = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);
           
            float disX, disY;
            disX = DistanceBetween(origin.Location.X, Location.X);
            disY = DistanceBetween(origin.Location.Y, Location.Y);

            float sinZ, cosZ;
            sinZ = (float)Math.Sin(AngleToRadians(Rotation.Z));
            cosZ = (float)Math.Cos(AngleToRadians(Rotation.Z));

            float X, Y, Z;
            X = disX * cosZ + disY * sinZ;
            Y = disY * cosZ - disX * sinZ;
            Z = Location.Z;
            Location = new Vector3(X, Y, Z);
        }
        public void ChangeRotation(Vector3 rot)
        {
            Rotation = new Vector3(rot.X, rot.Y, rot.Z);
        }
        #endregion

        #region Operations
        public static float DistanceBetween(Vector3 p1, Vector3 p2) => (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2));
        public static float DistanceBetween(float p1x, float p1y, float p2x, float p2y) => (float)Math.Sqrt(Math.Pow(p2x - p1x, 2) + Math.Pow(p2y - p1y, 2));
        public static float DistanceBetween(float p1, float p2) => (float)Math.Sqrt(Math.Pow(p2 - p1, 2));

        private float Flip(float x) => x < 0 ? -(180 + x) : 180 - x;
        private float ToPositiveDegree(float x) => x < 0 ? 360 + x : x;
        private double AngleToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        #endregion

        #region Set
        public void SetIsInfoDisplayed(bool isInfoDisplayed)
        {
            IsInfoDisplayed = isInfoDisplayed;
            Logics.Refresh();
        }
        #endregion
    }
}