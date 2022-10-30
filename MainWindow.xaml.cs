using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _3D_visualizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string loc = "test.txt";
        private static DispatcherTimer spinUpdate;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            spinUpdate = new DispatcherTimer();
            spinUpdate.Interval = TimeSpan.FromMilliseconds(20);
            spinUpdate.Tick += spin_Tick;

            //load obj selector
            DirectoryInfo dirInfo = new DirectoryInfo(System.IO.Path.GetFullPath("."));
            FileInfo[] files = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                if (file.Extension == ".obj" || file.Extension == ".txt")
                {
                    //Get object name
                    string objectName = "";
                    StreamReader sr = File.OpenText(file.FullName);
                    while (!sr.EndOfStream && objectName == "")
                    {
                        string[] hlpr = sr.ReadLine().Trim().Split(' ');
                        if (hlpr[0] == "o")
                        {
                            string[] name = hlpr[1].Split('_');
                            if (name.Length != 1 && name[1][0] == '(') objectName = name[0] + name[1];
                            else objectName = name[0];
                        }
                    }
                    sr.Close();

                    ComboBoxItem item = new ComboBoxItem();
                    item.Content = objectName;
                    item.Tag = file.Name;

                    objSelector.Items.Add(item);
                }
            }

            disp.Height = Height;
            disp.Width = Width - 150;
            Renderer.Load(disp, grd);
            
            StartVisualizer();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsLoaded)
            {
                Renderer.Load(disp,grd);
                Logics.Refresh();
            }
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (infoDisplay.IsChecked == true)
            {
                Point mousePoint = Mouse.GetPosition(disp);

                Mesh3D mesh = Logics.GetMesh();
                int index = 0;
                bool done = false;

                while (!done && index < mesh.Vertecies.Count)
                {

                }

                foreach (var vertex in mesh.Vertecies)
                {
                    float y, z;
                    if (Logics.GetIsPerspective())
                    {
                        y = (float)vertex.ProjectedLocation.X;
                        z = (float)vertex.ProjectedLocation.Y;
                    }
                    else
                    {
                        y = vertex.Location.Y;
                        z = vertex.Location.Z;
                    }

                    float dis = Point3D.DistanceBetween(y, z, (float)mousePoint.X, (float)mousePoint.Y * -1);
                    if (dis < 10) vertex.SetIsInfoDisplayed(true);
                    else vertex.SetIsInfoDisplayed(false);
                }
                testerText.Text = $"{mousePoint.X},{mousePoint.Y}";
            }
        }

        private void objSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)objSelector.SelectedItem;
            loc = typeItem.Tag.ToString();
            StartVisualizer();
        }

        private void DragOver(object sender, DragEventArgs e)
        {
            if (CheckDrop())
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
        }

        #endregion

        #region Display

        private void StartVisualizer()
        {
            Logics.Load(loc);

            vertexDisplay.IsChecked = false;
            infoDisplay.IsChecked = false;

            lineDisplay.IsChecked = true;

            faceDisplay.IsChecked = false;

            perspCheck.IsChecked = false;

            spinCheck.IsChecked = false;

            try
            {
                spinUpdate.Stop();
            }
            catch { }

            RotXSlider.Value = 0;
            RotYSlider.Value = 0;
            RotZSlider.Value = 0;
            if (loc.Split('.').Last() == "obj") ScaleSlider.Value = 10;
        }

        private void vertexDisplay_Checked(object sender, RoutedEventArgs e) => Logics.DisplayVerts(true);

        private void vertexDisplay_Unchecked(object sender, RoutedEventArgs e) => Logics.DisplayVerts(false);

        private void lineDisplay_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) Logics.DisplayLines(true);
        }

        private void lineDisplay_Unchecked(object sender, RoutedEventArgs e) => Logics.DisplayLines(false);

        private void faceDisplay_Checked(object sender, RoutedEventArgs e) => Logics.DisplayFaces(true);

        private void faceDisplay_Unchecked(object sender, RoutedEventArgs e) => Logics.DisplayFaces(false);

        #endregion

        #region Rotation
        private void RotXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                Xrot.Text = $"{RotXSlider.Value}";
                Logics.RotateMesh((float)RotXSlider.Value, null, null);
                Logics.Refresh();
            }
        }

        private void RotYSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                Yrot.Text = $"{RotYSlider.Value}";
                Logics.RotateMesh(null, (float)RotYSlider.Value, null);
                Logics.Refresh();
            }

        }

        private void RotZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded)
            {
                Zrot.Text = $"{RotZSlider.Value}";
                Logics.RotateMesh(null, null, (float)RotZSlider.Value);
                Logics.Refresh();
            }

        }

        private void spinCheck_Checked(object sender, RoutedEventArgs e) => spinUpdate.Start();

        private void spinCheck_Unchecked(object sender, RoutedEventArgs e) => spinUpdate.Stop();

        void spin_Tick(object sender, EventArgs e)
        {
            if (RotZSlider.Value == 360) RotZSlider.Value = 0;
            else RotZSlider.Value += 2;
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

        #region Cheks

        private static bool CheckDrop()
        {
            return true;
        }

        #endregion

        #region Operations

        private void SortByDistance(ref List<Point3D> vertecies)
        {
            for (int i = 0; i < vertecies.Count; i++)
            {
                for (int j = i+1; j < vertecies.Count-1; j++)
                {
                    if (vertecies[i].Location.X < vertecies[j].Location.X)
                    {
                        Point3D hlpr = vertecies[i];
                        vertecies[i] = vertecies[j];
                        vertecies[j] = hlpr;
                    }
                }
            }
        }

        #endregion

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Renderer.SaveImage();
        }
    }
}
