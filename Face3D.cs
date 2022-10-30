using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace _3D_visualizer
{
    internal class Face3D
    {
        public List<Point3D> Points { get; private set; }
        
        public float AvgX { get; private set; }
        
        public Brush FColor { get; private set; }
        
        public bool IsVisible { get; private set; }

        private static Random rnd = new Random();

        public Face3D(List<Point3D> points)
        {
            Points = points;

            GetAvgX();

            Color color = Color.FromArgb((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256));
            FColor = new SolidBrush(color);

            IsVisible = true;
        }

        public void GetAvgX()
        {
            float avg = 0;
            foreach (Point3D p in Points) avg += p.Location.X;
            avg /= Points.Count;
            AvgX = avg;
        }


    }
}
