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
            Logics.Refresh("test.txt", (int)RotXSlider.Value);
            tester.Text = $"{RotXSlider.Value}";
        }

        private void persp_Checked(object sender, RoutedEventArgs e)
        {
            Logics.PerspectiveTo(true);
            Logics.Refresh("test.txt", (int)RotXSlider.Value);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Logics.PerspectiveTo(false);
            Logics.Refresh("test.txt", (int)RotXSlider.Value);
        }
    }
}
