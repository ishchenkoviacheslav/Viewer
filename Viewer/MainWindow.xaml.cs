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

namespace Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AllowDrop = true;
            MainGrid.AllowDrop = true;
            MainGrid.Drop += MainGrid_Drop;
            this.Drop += MainGrid_Drop;
        }

        private void MainGrid_Drop(object sender, DragEventArgs e)
        {
            textBox.Text = sender.GetType().ToString() + "\n";
            foreach (string item in e.Data.GetFormats(true))
            {
                textBox.Text += item + "\n";
            }
            //путь картинки
            Image myImage = new Image();
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(@"C:\Users\user\Documents\visual studio 2015\Projects\Viewer\Viewer\bin\Debug\1.png");
            bi3.EndInit();
            myImage.Stretch = Stretch.Fill;
            myImage.Source = bi3;
            image1_1.Source = myImage.Source;
            
            //MainGrid.Background = Brushes.Gold;
            //this.Background = Brushes.ForestGreen;
            //this.Title = "Drop";
        }
    }
}
