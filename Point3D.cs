using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;

namespace _3D_visualizer
{
    internal class Point3D
    {
        public int VertIndex { get; private set; }

        public Vector3 DefLocation { get; private set; }
        public Vector3 Location { get; private set; }
        public Point ProjectedLocation { get; private set; }

        public Vector3 QuadrantAngle { get; private set; }
        public Vector3 Rotation { get; private set; }

        public bool IsInfoDisplayed { get; private set; }

        private Camera MainCam;


        #region Constructor

        public Point3D(float x, float y, float z, Point3D origin,int index, Camera mainCam)
        {
            MainCam = mainCam;

            this.VertIndex = index;

            DefLocation = new Vector3(x, y, z);
            Location = new Vector3(DefLocation.X, DefLocation.Y, DefLocation.Z);
            ProjectTo2D();

            QuadrantAngle = CalculateQuadrantAngle(origin);
            Rotation = new Vector3(QuadrantAngle.X, QuadrantAngle.Y, QuadrantAngle.Z);

            IsInfoDisplayed = false;
            MainCam = mainCam;
        }
        public Point3D(float x, float y, float z,Camera mainCam)
        {
            MainCam = mainCam;

            VertIndex = 0;

            DefLocation = new Vector3(x, y, z);
            Location = new Vector3(DefLocation.X, DefLocation.Y, DefLocation.Z);
            ProjectTo2D();

            QuadrantAngle = new Vector3(0, 0, 0);
            Rotation = new Vector3(0, 0, 0);

            IsInfoDisplayed = false;
        }
        #endregion

        #region Location

        public void ChangeLocation(Vector3 loc)
        {
            Location = new Vector3(loc.X,loc.Y,loc.Z);
            ProjectTo2D();
        }

        #endregion

        #region Rotation

        public void ChangeXRotation(Point3D origin)
        {
            float rX = origin.Rotation.X;
            if (QuadrantAngle.X == 90 || QuadrantAngle.X == 270) rX *= -1;
            Rotation = new Vector3(QuadrantAngle.X + rX, Rotation.Y, Rotation.Z);

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

            if (QuadrantAngle.X == 90 || QuadrantAngle.X == 270)
            {
                float hlpr = Y;
                Y = Z* -1;
                Z = hlpr* -1;
            }

            Location = new Vector3(X, Y, Z);
            ProjectTo2D();
        }

        public void ChangeXRotation2(Point3D origin)
        {
            float rX = origin.Rotation.X;
            if (QuadrantAngle.X == 90 || QuadrantAngle.X == 270) rX *= -1;
            Rotation = new Vector3(rX, Rotation.Y, Rotation.Z);

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

            //if (QuadrantAngle.X == 90 || QuadrantAngle.X == 270)
            //{
            //    float hlpr = Y;
            //    Y = Z* -1;
            //    Z = hlpr* -1;
            //}

            Location = new Vector3(X, Y, Z);
            ProjectTo2D();
        }

        public void ChangeYRotation(Point3D origin)
        {
            float rY = origin.Rotation.Y;
            if (QuadrantAngle.Y == 180|| QuadrantAngle.Y == 0) rY *= -1;
            Rotation = new Vector3(Rotation.X, QuadrantAngle.Y + rY, Rotation.Z);
            
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

            if (QuadrantAngle.Y == 180 || QuadrantAngle.Y == 0)
            {
                float hlpr = X;
                X = Z;
                Z = hlpr;
            }

            Location = new Vector3(X, Y, Z);
            ProjectTo2D();
        }

        public void ChangeZRotation(Point3D origin)
        {
            float rZ = origin.Rotation.Z;
            if (QuadrantAngle.Z == 270 || QuadrantAngle.Z == 90) rZ *= -1;
            Rotation = new Vector3(Rotation.X, Rotation.Y, QuadrantAngle.Z + rZ);;
           
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

            if (QuadrantAngle.Z == 270 || QuadrantAngle.Z == 90)
            {
                float hlpr = X * -1;
                X = Y * -1;
                Y = hlpr;
            }

            Location = new Vector3(X, Y, Z);
            ProjectTo2D();
        }

        public void SetRotation(Vector3 rot)
        {
            Rotation = new Vector3(rot.X, rot.Y, rot.Z);
        }

        #endregion

        #region Projection

        public void ProjectTo2D()
        {
            if (MainCam.Perspective)
            {
                float Y, Z;
                Y = Location.Y;
                Z = Location.Z;

                float F = MainCam.FocalLength / (MainCam.FocalLength + Location.X);

                float yHlpr = Location.Y * F;
                if (yHlpr < float.MaxValue && yHlpr > float.MinValue) Y = yHlpr;
                float zHlpr = Location.Z * F;
                if (zHlpr < float.MaxValue && zHlpr > float.MinValue) Z = zHlpr;

                ProjectedLocation = new Point((int)Y, (int)Z);

            }
            else
            {
                ProjectedLocation = new Point((int)Location.Y, (int)Location.Z);
            }
        }

        #endregion

        #region Operations
        public static float DistanceBetween(Vector3 p1, Vector3 p2) => (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2));
        public static float DistanceBetween(float p1x, float p1y, float p2x, float p2y) => (float)Math.Sqrt(Math.Pow(p2x - p1x, 2) + Math.Pow(p2y - p1y, 2));
        public static float DistanceBetween(float p1, float p2) => (float)Math.Sqrt(Math.Pow(p2 - p1, 2));

        private float Flip(float x) => x < 0 ? -(180 + x) : 180 - x;
        private float ToPositiveDegree(float x) => x < 0 ? 360 + x : Math.Abs(x);
        private double AngleToRadians(double angle) => (Math.PI / 180) * angle;

        private Vector3 CalculateQuadrantAngle(Point3D origin)
        {
            float X, Y, Z;
            float xAngle = ToPositiveDegree((float)(Math.Atan2(Location.Y - origin.Location.Y, Location.Z - origin.Location.Z) * (180 / Math.PI)));
            if (xAngle <= 90) X = 0;
            else if (xAngle < 180) X = 270;
            else if (xAngle <= 270) X = 180;
            else X = 90;

            float yAngle = ToPositiveDegree((float)(Math.Atan2(Location.X - origin.Location.X, Location.Z - origin.Location.Z) * (180 / Math.PI)));
            if (yAngle <= 90) Y = 0;
            else if (yAngle < 180) Y = 270;
            else if (yAngle <= 270) Y = 180;
            else Y = 90;

            float zAngle = ToPositiveDegree(Flip((float)(Math.Atan2(Location.Y - origin.Location.Y, Location.X - origin.Location.X) * (180 / Math.PI))));
            if (zAngle <= 90) Z = 0;
            else if (zAngle < 180) Z = 270;
            else if (zAngle <= 270) Z = 180;
            else Z = 90;

            return new Vector3(X, Y, Z);

        }

        #endregion

        #region Set

        public void SetIsInfoDisplayed(bool isInfoDisplayed)
        {
            IsInfoDisplayed = isInfoDisplayed;
            Logics.Refresh();
        }

        public void SetQuadrantAngle(Point3D origin)
        {
            Vector3 newAngle = new Vector3(QuadrantAngle.X + origin.Rotation.X, QuadrantAngle.Y + origin.Rotation.Y, QuadrantAngle.Z + origin.Rotation.Z);
            QuadrantAngle = newAngle;
        }

        #endregion
    }
}