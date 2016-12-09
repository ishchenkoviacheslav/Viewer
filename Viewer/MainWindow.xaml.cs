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
using System.IO;

namespace Viewer
{
    /// <summary> Windows Photo Viewer supports images in BMP, JPEG, JPEG XR (formerly HD Photo), PNG, ICO, GIF and TIFF file formats.[7]
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Image> allPictures = new List<Image>();
        public MainWindow()
        {
            InitializeComponent();
            myInitialize();


            this.AllowDrop = true;
            MainGrid.AllowDrop = true;
            MainGrid.Drop += MainGrid_Drop;
            this.Drop += MainGrid_Drop;
        }

        private void myInitialize()
        {
            //изменить фильтр/паттерн поиска
           List<string> listBmp = Directory.GetFiles(System.Environment.CurrentDirectory, "*.bmp").ToList();
           List<string> listJpeg = Directory.GetFiles(System.Environment.CurrentDirectory, "*.jpeg").ToList();
           List<string> listJpg = Directory.GetFiles(System.Environment.CurrentDirectory, "*.jpg").ToList();
            List<string> listPng = Directory.GetFiles(System.Environment.CurrentDirectory, "*.png").ToList();
           List<string> listIco = Directory.GetFiles(System.Environment.CurrentDirectory, "*.ico").ToList();
           List<string> listGif = Directory.GetFiles(System.Environment.CurrentDirectory, "*.gif").ToList();
           List<string> listTiff = Directory.GetFiles(System.Environment.CurrentDirectory, "*.tiff").ToList();
            IEnumerable<string> allNamePictures = (((((listBmp.Concat(listJpeg)).Concat(listJpg)).Concat(listPng)).Concat(listIco)).Concat(listGif)).Concat(listTiff);
            foreach (string imgPath in allNamePictures)
            {
                Image NewImage = new Image();
                BitmapImage bitImg = new BitmapImage();
                bitImg.BeginInit();
                bitImg.UriSource = new Uri(imgPath);
                bitImg.EndInit();
                NewImage.Stretch = Stretch.Uniform;
                NewImage.Source = bitImg;
                NewImage.Height = 150;
                NewImage.Width = 200;
                NewImage.MinHeight = 150;
                NewImage.Margin = new Thickness(2,2,2,2);
                Grid.SetRow(NewImage, Position.ROW);
                Grid.SetColumn(NewImage, Position.COLUMN);
                Position.NextPosition(MainGrid);
                allPictures.Add(NewImage);
            }

            foreach (Image img in allPictures)
            {
                MainGrid.Children.Add(img);
            }
        }
        private void MainGrid_Drop(object sender, DragEventArgs e)
        {
            textBox.Text = sender.GetType().ToString() + "\n";
            foreach (string item in e.Data.GetFormats(true))
            {
                textBox.Text += item + "\n";
            }
            //путь картинки
            
           // image1_1.Source = myImage.Source;

            //MainGrid.Background = Brushes.Gold;
            //this.Background = Brushes.ForestGreen;
            //this.Title = "Drop";

            // Загрузить все файлы *.jpg и создать новую папку для модифицированных данных,
            //string[] files = Directory.GetFiles(@"C:\Users\Public\Pictures\Sample Pictures", "*.jpg", SearchOption.AllDirectories);
            //string newDir = @"C:\ModifiedPictures";
            //Directory.CreateDirectory(newDir);
            //// Обработать данные изображений в блокирующей манере.
            //foreach (string currentFile in files)
            //{
            //    string filename = System.IO.Path.GetFileName(currentFile);
            //    using (Bitmap bitmap = new Bitmap(currentFile))
            //    {
            //        bitmap.RotateFlip(RotateFlipType.Rotatel80FlipNone);
            //        bitmap.Save(Path.Combine(newDir, filename));
            //        // Вывести идентификатор потока, обрабатывающего текущее изображение.
            //        this.Text = string.Format("Processing {0} on thread {1}", filename,
            //        Thread.CurrentThread.ManagedThreadId);
            //    }
            //}

        }
    }
}
