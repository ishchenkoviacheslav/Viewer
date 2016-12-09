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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Image> allPictures = new List<Image>();
        //List<Image> allControls = new List<Image>();
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
            List<string> allNamePictures = Directory.GetFiles(System.Environment.CurrentDirectory, "*.png").ToList();
            foreach (string imgPath in allNamePictures)
            {
                Image NewImage = new Image();
                BitmapImage bitImg = new BitmapImage();
                bitImg.BeginInit();
                bitImg.UriSource = new Uri(imgPath);
                bitImg.EndInit();
                NewImage.Stretch = Stretch.Fill;
                NewImage.Source = bitImg;
                NewImage.Height = 150;
                allPictures.Add(NewImage);
            }

            foreach (Image img in allPictures)
            {
                MainGrid.
            }
            //добавление изначально имеющихся контролов, остальные будут добавлятся по необходимости
            //allControls.Add(image1_1);
            //allControls.Add(image1_2);
            //allControls.Add(image1_3);
            //allControls.Add(image1_4);
            
            //заполняем Image контролы, картинками из коллекции.
            //foreach (Image img in allPictures)
            //{
            //    //if(image1_1.Source == null)
            //    //{
            //    //    image1_1.Source = img.Source;
            //    //    continue;
            //    //}
            //    //if (image1_2.Source == null)
            //    //{
            //    //    image1_2.Source = img.Source;
            //    //    continue;
            //    //}
            //    //if (image1_3.Source == null)
            //    //{
            //    //    image1_3.Source = img.Source;
            //    //    continue;
            //    //}
            //    //if (image1_4.Source == null)
            //    //{
            //    //    image1_4.Source = img.Source;
            //    //    continue;
            //    //}
            //    //else
            //    //{
            //        allControls.Add(img);
            //    //}
            //}
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
