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

namespace CornellRoom
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RayTracing rt;
        public MainWindow()
        {
            InitializeComponent();
            rt = new RayTracing(400, 400, Camera.Create(new Point(3, 2, 4), new Point(-1, .5, 0)), 4, 4, 2);
            rt.AddObj(new Plane()
            {
                norm = new Point(0, 1, 0),
                Offset = 0,
                surface = Surfaces.Shiny()
            });
            rt.AddObj(new Plane()
            {
                norm = new Point(1, 0, 1),
                Offset = 5,
                surface = Surfaces.fill(new Point(0, 0, 1))
            });
            rt.AddObj(new Plane()
            {
                norm = new Point(1, 0, -2),
                Offset = 10,
                surface = Surfaces.fill(new Point(0, 1, 1))
            });
            rt.AddObj(new Plane()
            {
                norm = new Point(-2, 0, 1),
                Offset = 7,
                surface = Surfaces.fill(new Point(1, 0, 0))
            });
            rt.AddObj(new Sphere()
            {
                center = new Point(0, 1, 0),
                r = 1,
                surface = Surfaces.fill(new Point(1, 0, 0))
            });
            rt.AddObj(new Sphere()
            {
                center = new Point(-1, .5, 1.5),
                r = .5,
                surface = Surfaces.Shiny(roughness: 50)
            });
            rt.AddLight(new Light()
            {
                position = new Point(-2, 2.5, 0),
                color = new Point(.49, .07, .07),
                intensity = 1
            });
            rt.AddLight(new Light()
            {
                position = new Point(1.5, 2.5, 1.5),
                color = new Point(.07, .07, .49),
                intensity = 1
            });
            rt.AddLight(new Light()
            {
                position = new Point(1.5, 2.5, -1.5),
                color = new Point(.07, .49, .071),
                intensity = 1
            });
            rt.AddLight(new Light()
            {
                position = new Point(0, 3.5, 0),
                color = new Point(.21, .21, .35),
                intensity = 1
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btmp = rt.calculate();
            var wnd = new ImageViewer(btmp);
            wnd.FillContent();
            wnd.Show();
        }
    }
}
