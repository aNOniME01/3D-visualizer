using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using _3D_visualizer.Objects;

namespace _3D_visualizer
{
    internal class Renderer
    {
        private static double WHeight;
        private static double WWidth;

        private static System.Windows.Controls.Image image;
        private static Grid grid;
        private static TextBlock meshInfo;


        private static Graphics graphics;
        private static Bitmap bitmap;

        private static bool VertexInfo;

        private static double resMultiplyer = 1.5;


        #region Load
        public static void Load(System.Windows.Controls.Image img,Grid grd)
        {
            grid = grd;
            image = img;
            meshInfo = new TextBlock();
            grid.Children.Add(meshInfo);

            bitmap = new Bitmap((int)(img.Width * resMultiplyer), (int)(img.Height * resMultiplyer));
            graphics = Graphics.FromImage(bitmap);

            WHeight = bitmap.Height;
            WWidth = bitmap.Width;
        }

        #endregion

        #region Render

        public static void Render()
        {
            image.Source = BmToBmImage(bitmap);
        }

        #endregion

        #region Line
        public static void AddLine(Face3D face)
        {
            Point[] faceMesh = new Point[face.Points.Count];

            for (int i = 0; i < face.Points.Count; i++)
            {
                faceMesh[i] = (new Point((int)(face.Points[i].ProjectedLocation.X + WWidth / 2), (int)(WHeight - (face.Points[i].ProjectedLocation.Y + WHeight / 2) + 2)));
            }

            Pen pen = new Pen(Color.Lime);
            pen.Width = 1;
            graphics.DrawPolygon(pen, faceMesh);

        }
        public static void AddLine(Point3D[] points)
        {
            Point[] faceMesh = new Point[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                faceMesh[i] = (new Point((int)(points[i].ProjectedLocation.X + WWidth / 2), (int)(WHeight - (points[i].ProjectedLocation.Y + WHeight / 2) + 2)));
            }

            Pen pen = new Pen(Color.Lime);
            pen.Width = 1;
            graphics.DrawPolygon(pen, faceMesh);

        }

        #endregion

        #region Face

        public static void AddFace(Face3D face)
        {
            Point[] faceMesh = new Point[face.Points.Count];

            for (int i = 0; i < face.Points.Count; i++)
            {
                faceMesh[i] = (new Point((int)(face.Points[i].ProjectedLocation.X + WWidth / 2), (int)(WHeight - (face.Points[i].ProjectedLocation.Y + WHeight / 2) + 2)));
            } 

            graphics.FillPolygon(face.FColor,faceMesh);

        }

        #endregion

        #region Point
        public static void AddPoint(Point3D point)
        {
            Point projected = point.ProjectedLocation;

            int w = 3;
            int h = 3;
            int x = Convert.ToInt32(projected.X - (w / 2) + WWidth / 2);
            int y = Convert.ToInt32((image.Height * resMultiplyer) - (projected.Y - (h / 2) + WHeight / 2) );

            graphics.FillRectangle(Brushes.Lime, x, y, w, h);
        }

        #endregion

        #region Info
        private static void AddVertexInfo(Point3D point, System.Windows.Media.Brush color)
        {
            TextBlock vertexInfo = new TextBlock();

            /*if(point.Location.X <= 0)*/
            vertexInfo.Text = point.IsInfoDisplayed ? $"{point.VertIndex} \n {GetVertexInfo(point)}"
                       : $"{point.VertIndex} \n";
            if (point.Location.X <= 0) vertexInfo.Foreground = System.Windows.Media.Brushes.DarkRed;
            else vertexInfo.Foreground = color;

            Point projected = point.ProjectedLocation;
            Canvas.SetLeft(vertexInfo, projected.X + WWidth / 2 - 7);
            Canvas.SetBottom(vertexInfo, projected.Y + WHeight / 2 - 25);

            grid.Children.Add(vertexInfo);
        }

        private static string GetVertexInfo(Point3D point)
        {
            return $"Loc: x{point.Location.X.ToString("0.0")}, y{point.Location.Y.ToString("0.0")}, z{point.Location.Z.ToString("0.0")} \n" +
                $"Rot: x{point.Rotation.X.ToString("0.0")}, y{point.Rotation.Y.ToString("0.0")}, z{point.Rotation.Z.ToString("0.0")}";
        }

        public static void AddMeshInfo(Mesh3D mesh)
        {
            meshInfo.Text = $"vertex count: {mesh.Vertecies.Count} \n" +
                $"line count: {mesh.Lines.Count} \n" +
                $"face count: {mesh.Faces.Count}";
            meshInfo.FontSize = 10;
            meshInfo.Foreground = System.Windows.Media.Brushes.Lime;

            Canvas.SetLeft(meshInfo, 2);
            Canvas.SetBottom(meshInfo, WHeight - 40);
        }
        #endregion

        #region Operation

        public static void ClearCanvas()
        {
            bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            graphics = Graphics.FromImage(bitmap);
        }

        private static BitmapImage BmToBmImage(Bitmap bm)
        {
            using (var memory = new MemoryStream())
            {
                //Saves bitmap to memory
                bm.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;

                //Loads Bitmap from memory and saves it as BitmapImage
                var bmI = new BitmapImage();
                bmI.BeginInit();
                bmI.StreamSource = memory;
                bmI.CacheOption = BitmapCacheOption.OnLoad;
                bmI.EndInit();
                bmI.Freeze();

                return bmI;
            }
        }

        #endregion

        #region Set
        public static void SetVertexInfo(bool vertexInfo)
        {
            VertexInfo = vertexInfo;
            Logics.Refresh();
        }
        #endregion

        public static void SaveImage()
        {
            string date = Convert.ToString(DateTime.Now);
            date = date.Replace(':', '-');
            date = date.Replace('/', '-');
            date = date.Replace(' ', '_');
            bitmap.Save($"C:\\Users\\csong\\Downloads\\{date}.png", ImageFormat.Png);
        }

    }
}
