using System;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Renderer.Load(canvas);
            Logics.Load("test.txt");
        }

        private void RotXSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded == true)
            {
                tester.Text = $"{RotXSlider.Value}";
                Logics.RotateMesh((int)RotXSlider.Value);
                Logics.Refresh("test.txt");
            }
        }

        private void persp_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded == true)
            {
                Logics.PerspectiveTo(true);
                Logics.Refresh("test.txt");
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded == true)
            {
                Logics.PerspectiveTo(false);
                Logics.Refresh("test.txt");
            }
        }

        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsLoaded == true)
            {
                tester.Text = $"{ScaleSlider}";
                Logics.ScaleMesh((float)ScaleSlider.Value);
                Logics.Refresh("test.txt");
            }
        }
    }
}
