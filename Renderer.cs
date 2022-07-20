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

        private static bool VertexInfo;


        public static void Load(Canvas cnvs) 
        {
            canvas = cnvs;
            WHeight = canvas.ActualHeight;
            WWidth = canvas.ActualWidth;
        }

        public static void AddLine(Vector3 p1, Vector3 p2)
        {
            Line line = new Line();
            line.X1 = p1.Y + WWidth/2;
            line.Y1 = WHeight - (p1.Z + WHeight / 2);
            line.X2 = p2.Y + WWidth / 2;
            line.Y2 = WHeight - (p2.Z + WHeight / 2);

            line.StrokeThickness = 1;
            line.Stroke = Brushes.Lime;

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
        public static void AddPoint(Point3D point,int num) 
        {

            Vector3 projected = Logics.ProjectTo2D(point);
            Rectangle rectangle = new Rectangle();
            rectangle.Height = 5;
            rectangle.Width = 5;
            rectangle.Fill = Brushes.LimeGreen;

            Canvas.SetLeft(rectangle, projected.Y - (rectangle.Width/2) + WWidth/2);
            Canvas.SetBottom(rectangle, projected.Z - (rectangle.Height / 2) + WHeight/2);

            try
            {
                canvas.Children.Add(rectangle);
            }
            catch { }
            if (VertexInfo) AddVertexInfo(point,num,projected,Brushes.Red);
        }
        private static void AddVertexInfo(Point3D point,int num, Vector3 projected,Brush color)
        {
            TextBlock vertexInfo = new TextBlock();
            vertexInfo.Text = $"{num}\n" /* +
                $"{point.Location.X.ToString("0.0")},{point.Location.Y.ToString("0.0")},{point.Location.Z.ToString("0.0")} \n" +
                $" {point.Rotation.X.ToString("0.0")},{point.Rotation.Y.ToString("0.0")},{point.Rotation.Z.ToString("0.0")}"*/;
            vertexInfo.FontSize = 8;
            vertexInfo.Foreground = color;

            Canvas.SetLeft(vertexInfo, projected.Y + WWidth / 2);
            Canvas.SetBottom(vertexInfo, projected.Z + WHeight / 2);

            canvas.Children.Add(vertexInfo);
        }

        public static void AddMeshInfo(Mesh3D mesh)
        {
            TextBlock meshInfo = new TextBlock();
            meshInfo.Text = $"vertex count: {mesh.Vertecies.Count} \n" +
                $"line count: {mesh.Lines.Count}";
            meshInfo.FontSize = 10;
            meshInfo.Foreground = Brushes.Lime;

            Canvas.SetLeft(meshInfo,2);
            Canvas.SetBottom(meshInfo,WHeight-28);
            canvas.Children.Add(meshInfo);
        }

        public static void AddOriginPoint(Point3D point) 
        {
            Vector3 projected = Logics.ProjectTo2D(point);
            Rectangle rectangle = new Rectangle();
            rectangle.Height = 5;
            rectangle.Width = 5;
            rectangle.Fill = Brushes.Red;

            Canvas.SetLeft(rectangle, projected.Y - (rectangle.Width / 2) + WWidth / 2);
            Canvas.SetBottom(rectangle, projected.Z - (rectangle.Height / 2) + WHeight / 2);

            try
            {
                canvas.Children.Add(rectangle);
            }
            catch { }
            if (VertexInfo) AddVertexInfo(point,0, projected, Brushes.Green);
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

        public static void SetVertexInfo(bool bolean) => VertexInfo = bolean;
    }
}
