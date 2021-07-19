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
using System.Windows.Shapes;

namespace CornellRoom
{
    /// <summary>
    /// Логика взаимодействия для ImageViewer.xaml
    /// </summary>
    public partial class ImageViewer : Window
    {
        System.Drawing.Bitmap btmp;
        public ImageViewer()
        {
            InitializeComponent();
        }
        public ImageViewer(System.Drawing.Bitmap btmp)
        {
            InitializeComponent();
            Set(btmp);
        }
       public void Set(System.Drawing.Bitmap btmp)
        {
            this.btmp = btmp;
        }
        public void FillContent()
        {
            if (btmp != null)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                btmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                image.Source = bi;
            }
        }
    }
}
