using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _3D_visualizer
{
    internal class Renderer
    {
        private static double WHeight;
        private static double WWidth;
        private static Canvas canvas;


        public static void Load(Canvas cnvs) 
        {
            canvas = cnvs;
            WHeight = canvas.ActualHeight;
            WWidth = canvas.ActualWidth;
        }

        public static void AddLine(float[,] p1, float[,] p2)
        {
            Line line = new Line();
            line.X1 = p1[0, 0] + WWidth/2;
            line.Y1 = p1[1, 0] + WHeight / 2;
            line.X2 = p2[0, 0] + WWidth / 2;
            line.Y2 = p2[1, 0] + WHeight / 2;

            line.StrokeThickness = 2;
            line.Stroke = Brushes.Black;

            try
            {
                canvas.Children.Add(line);
            }
            catch { }
        }

        public static void RemoveLine(Line line) 
        {
            try
            {
                canvas.Children.Remove(line);
            }
            catch {}
        }
        public static void AddPoint(float[,] point) 
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = 5;
            rectangle.Width = 5;
            rectangle.Fill = Brushes.Black;

            Canvas.SetLeft(rectangle, point[0,0] - (rectangle.Height/2) + WWidth/2);
            Canvas.SetTop(rectangle, point[1,0] - (rectangle.Width / 2) + WHeight/2);

            try
            {
                canvas.Children.Add(rectangle);
            }
            catch { }
        }

        public static void AddOriginPoint(float[,] point) 
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Height = 5;
            rectangle.Width = 5;
            rectangle.Fill = Brushes.Red;

            Canvas.SetLeft(rectangle, point[0,0] - (rectangle.Height/2) +WWidth/2);
            Canvas.SetTop(rectangle, point[1,0] - (rectangle.Width / 2)+WHeight/2);

            try
            {
                canvas.Children.Add(rectangle);
            }
            catch { }
        }

        public static void RemovePoint(Rectangle point) 
        {
            try
            {
                canvas.Children.Remove(point);
            }
            catch {}
        }

        public static void ClearCanvas() => canvas.Children.Clear();
    }
}
