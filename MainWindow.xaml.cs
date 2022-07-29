using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3D_visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static string loc = "test.txt";
        //private static string loc = "rect.txt";
        //private static string loc = "cube.obj";
        //private static string loc = "uvsphere.obj";
        //private static string loc = "circle.obj";
        //private static string loc = "testsphere.obj";
        //private static string loc = "testsphere2.obj";
        //private static string loc = "testsphereY.obj";
        //private static string loc = "testsphereZ.obj";
        private static string loc = "icosphere.obj";
        //private static string loc = "hghrsicosphere.obj";
        //private static string loc = "monkey.obj";


        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Renderer.Load(canvas);
            Logics.Load(loc);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsLoaded)
            {
                Renderer.Load(canvas);
                Logics.Refresh();
            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (infoDisplay.IsChecked == true)
            {
                Point mousePoint = Mouse.GetPosition(Logics.GetOrigin().Body);
                Mesh3D mesh = Logics.GetMesh();
                foreach (var vertex in mesh.Vertecies)
                {
                    float y, z;
                    if (Logics.GetIsPerspective())
                    {
                        Vector3 projected = Logics.ProjectTo2D(vertex);
                        y = projected.Y;
                        z = projected.Z;
                    }
                    else
                    {
                        y = vertex.Location.Y;
                        z = vertex.Location.Z;
                    }

                    float dis = Point3D.DistanceBetween(y, z, (float)mousePoint.X, (float)mousePoint.Y);
                    if (dis < 10) vertex.SetIsInfoDisplayed(true);
                    else vertex.SetIsInfoDisplayed(false);
                }
                testerText.Text = $"{mousePoint.X},{mousePoint.Y}";
            }
        }
        #endregion

        #region Rotation
        private void RotXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                Xrot.Text = $"{RotXSlider.Value}";
                Logics.RotateMesh((int)RotXSlider.Value,null, null);
                Logics.Refresh();
            }
        }

        private void RotYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                Yrot.Text = $"{RotYSlider.Value}";
                Logics.RotateMesh(null, (int)RotYSlider.Value, null);
                Logics.Refresh();
            }

        }

        private void RotZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                Zrot.Text = $"{RotZSlider.Value}";
                Logics.RotateMesh(null, null, (int)RotZSlider.Value);
                Logics.Refresh();
            }

        }
        #endregion

        #region Scale
        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                scalerText.Text = $"{ScaleSlider.Value}";
                Logics.ScaleMesh((float)ScaleSlider.Value);
                Logics.Refresh();
            }
        }
        #endregion

        #region Perspective
        private void persp_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) Logics.PerspectiveTo(true);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) Logics.PerspectiveTo(false);
        }
        #endregion

        #region VertexInfo
        private void CheckBox_Checked(object sender, RoutedEventArgs e) => Renderer.SetVertexInfo(true);

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e) => Renderer.SetVertexInfo(false);
        #endregion

    }
}
