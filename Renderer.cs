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


        #region Load
        public static void Load(Canvas cnvs) 
        {
            canvas = cnvs;
            WHeight = canvas.ActualHeight;
            WWidth = canvas.ActualWidth;
        }
        #endregion

        #region Line
        public static void AddLine(Vector3 p1, Vector3 p2)
        {
            Line line = new Line();
            line.X1 = p1.Y + WWidth/2;
            line.Y1 = p1.Z + WHeight / 2;
            line.X2 = p2.Y + WWidth / 2;
            line.Y2 = p2.Z + WHeight / 2;

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
        #endregion

        #region Point
        public static void AddPoint(Point3D point,int num) 
        {

            Vector3 projected = Logics.ProjectTo2D(point);

            Canvas.SetLeft(point.Body, projected.Y - (point.Body.Width/2) + WWidth/2);
            Canvas.SetTop(point.Body, projected.Z - (point.Body.Height / 2) + WHeight/2);

            try
            {
                canvas.Children.Add(point.Body);
            }
            catch { }
            if (VertexInfo) AddVertexInfo(point,Brushes.Red);
        }

        public static void AddOriginPoint(Point3D point) 
        {
            Vector3 projected = Logics.ProjectTo2D(point);

            Canvas.SetLeft(point.Body, projected.Y - (point.Body.Width / 2) + WWidth / 2);
            Canvas.SetTop(point.Body, projected.Z - (point.Body.Height / 2) + WHeight / 2);

            try
            {
                canvas.Children.Add(point.Body);
            }
            catch { }
            if (VertexInfo) AddVertexInfo(point, Brushes.Green);
        }

        public static void RemovePoint(Rectangle point) 
        {
            try
            {
                canvas.Children.Remove(point);
            }
            catch {}
        }
        #endregion

        #region Info
        private static void AddVertexInfo(Point3D point, Brush color)
        {
            TextBlock vertexInfo = new TextBlock();
            
            /*if(point.Location.X <= 0)*/vertexInfo.Text = point.IsInfoDisplayed ? $"{point.VertIndex} \n {GetVertexInfo(point)}" 
                                                    : $"{point.VertIndex} \n";
            if (point.Location.X >= 0) vertexInfo.Foreground = Brushes.DarkRed;
            else vertexInfo.Foreground = color;

            Vector3 projected = Logics.ProjectTo2D(point);
            Canvas.SetLeft(vertexInfo, projected.Y + WWidth / 2);
            Canvas.SetTop(vertexInfo, projected.Z + WHeight / 2);

            canvas.Children.Add(vertexInfo);
        }

        private static string GetVertexInfo(Point3D point)
        {
            return $"Loc: x{point.Location.X.ToString("0.0")}, y{point.Location.Y.ToString("0.0")}, z{point.Location.Z.ToString("0.0")} \n" +
                $"Rot: x{point.Rotation.X.ToString("0.0")}, y{point.Rotation.Y.ToString("0.0")}, z{point.Rotation.Z.ToString("0.0")}";
        }

        public static void AddMeshInfo(Mesh3D mesh)
        {
            TextBlock meshInfo = new TextBlock();
            meshInfo.Text = $"vertex count: {mesh.Vertecies.Count} \n" +
                $"line count: {mesh.Lines.Count}";
            meshInfo.FontSize = 10;
            meshInfo.Foreground = Brushes.Lime;

            Canvas.SetLeft(meshInfo, 2);
            Canvas.SetBottom(meshInfo, WHeight - 28);
            canvas.Children.Add(meshInfo);
        }
        #endregion

        #region Operation
        public static void ClearCanvas() => canvas.Children.Clear();
        #endregion

        #region Set
        public static void SetVertexInfo(bool vertexInfo) 
        { 
            VertexInfo = vertexInfo;
            Logics.Refresh();
        }
        #endregion
    }
}
