﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Renderer.Load(canvas);
            //Logics.Load("test.txt");
            //Logics.Load("rect.txt");
            Logics.Load("cube.obj");
            //Logics.Load("uvsphere.obj");
            //Logics.Load("circle.obj");
            //Logics.Load("testsphere.obj");
            //Logics.Load("testsphere2.obj");
            //Logics.Load("icosphere.obj");
            //Logics.Load("hghrsicosphere.obj");
            //Logics.Load("monkey.obj");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsLoaded)
            {
                Renderer.Load(canvas);
                Logics.Refresh();
            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = Mouse.GetPosition(Logics.GetOrigin().Body);
            Mesh3D mesh = Logics.GetMesh();
            bool isMouseOver = false;
            foreach (var vertex in mesh.Vertecies)
            {
                float x, y;
                if (Logics.GetIsPerspective())
                {
                    x = vertex.Location.X;
                    y = vertex.Location.Y;
                }
                else
                {
                    x = vertex.Location.X;
                    y = vertex.Location.Y;
                }

                float dis = Point3D.DistanceBetween(x, y, (float)mousePoint.X, (float)mousePoint.Y);
                if (dis < 10)
                {
                    //Sets the vertexinfo to true at the vertex
                    isMouseOver = true;
                }
            }
            Renderer.SetVertexInfo(isMouseOver);
            testerText.Text = $"{mousePoint.X},{mousePoint.Y}";
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
