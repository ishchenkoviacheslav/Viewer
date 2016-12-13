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
                NewImage.Tag = System.IO.Path.GetFileName(imgPath);
                NewImage.Stretch = Stretch.Uniform;
                NewImage.Source = bitImg;
                NewImage.Height = 150;
                NewImage.Width = 200;
                NewImage.MinHeight = 150;
                NewImage.Margin = new Thickness(2, 2, 2, 2);
                NewImage.MouseDown += NewImage_MouseDown;
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

        private void NewImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount==2)
            {
                Image img = sender as Image;
                
                //освободжаем место в окне
                FirstGrid.Children.Clear();
                FirstGrid.ColumnDefinitions.Add(new ColumnDefinition());
                FirstGrid.ColumnDefinitions.Add(new ColumnDefinition() { MaxWidth = 100 });
                FirstGrid.RowDefinitions.Add(new RowDefinition());
                FirstGrid.RowDefinitions.Add(new RowDefinition());

                Image bigCopy = new Image();
                BitmapImage bitImg = new BitmapImage();
                bitImg.BeginInit();
                bitImg.UriSource = ((BitmapImage)img.Source).UriSource;
                bitImg.EndInit();
                bigCopy.Tag = System.IO.Path.GetFileName((string)img.Tag);
                bigCopy.StretchDirection = StretchDirection.Both;
                bigCopy.Source = bitImg;
               
                

                Grid.SetColumn(bigCopy, 0);
                Grid.SetRowSpan(bigCopy, 2);
                FirstGrid.Children.Add(bigCopy);

                Button up = new Button() { Content = "NEXT" };
                up.Click += Up_Click; ;
                Grid.SetColumn(up, 1);
                Grid.SetRow(up, 0);
                FirstGrid.Children.Add(up);

                Button down = new Button() { Content = "PREV" };
                down.Click += Down_Click; ;
                Grid.SetColumn(down, 1);
                Grid.SetRow(down, 1);
                FirstGrid.Children.Add(down);

                this.KeyDown += BigCopy_KeyDown;

                this.Drop -= MainGrid_Drop;
            }
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
                Title = "down";
        }

        private void Up_Click(object sender, RoutedEventArgs e)
        {
                Title = "up";
        }

        private void BigCopy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                Title = "down";
            }
            if (e.Key == Key.Up)
            {
                Title = "up";
            }
            //вернуть общий вид всех изображений
            if(e.Key == Key.Escape)
            {
                this.KeyDown -= BigCopy_KeyDown;
                FirstGrid.RowDefinitions.Clear();
                FirstGrid.ColumnDefinitions.Clear();
                FirstGrid.Children.Clear();
                FirstGrid.Children.Add(myScroll);
                this.Drop += MainGrid_Drop;
            }
        }

        private void MainGrid_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string str in files)
                {
                    //сделать проверку попадания одинаковых картинок
                    Image NewImage = new Image();
                    BitmapImage bitImg = new BitmapImage();
                    bitImg.BeginInit();
                    bitImg.UriSource = new Uri(str);
                    bitImg.EndInit();
                    NewImage.Tag = System.IO.Path.GetFileName(str);
                    NewImage.Stretch = Stretch.Uniform;
                    NewImage.Source = bitImg;
                    NewImage.Height = 150;
                    NewImage.Width = 200;
                    NewImage.MinHeight = 150;
                    NewImage.Margin = new Thickness(2, 2, 2, 2);


                    //добавляем все кроме повторяющихся
                    bool copy = false;
                    foreach (Image img in allPictures)
                    {
                        if (((string)img.Tag) == ((string)NewImage.Tag))
                        {
                            copy = true;
                        }
                    }
                    //если это не копия то добавить
                    if (copy == false)
                    {
                        allPictures.Add(NewImage);
                        //allPictures = allPictures.Distinct(new ImageComparer()).ToList();
                        MainGrid.Children.Add(NewImage);
                        Grid.SetRow(NewImage, Position.ROW);
                        Grid.SetColumn(NewImage, Position.COLUMN);
                        Position.NextPosition(MainGrid);
                    }
                    else
                    {
                        MessageBox.Show(string.Format("Изображение {0} уже имеется", (string)NewImage.Tag));
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка добавления");
                return;
            }
        }

        //if(e.Data.GetDataPresent(DataFormats.Bitmap))
        //{
        //    textBox.Text = "BITMAP";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Rtf))
        //{
        //    textBox.Text = "RTF";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Text))
        //{
        //    textBox.Text = "TEXT";
        //}
        //if (e.Data.GetDataPresent(DataFormats.CommaSeparatedValue))
        //{
        //    textBox.Text = "CommaSeparatedValue";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Dib))
        //{
        //    textBox.Text = "Dib";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Dif))
        //{
        //    textBox.Text = "Dif";
        //}
        //if(e.Data.GetDataPresent(DataFormats.EnhancedMetafile))
        //{
        //    textBox.Text = "EnhancedMetafile";
        //}
        //if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //{
        //    textBox.Text = "FileDrop";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Html))
        //{
        //    textBox.Text = "Html";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Locale))
        //{
        //    textBox.Text = "Locale";
        //}
        //if (e.Data.GetDataPresent(DataFormats.MetafilePicture))
        //{
        //    textBox.Text = "MetafilePicture";
        //}
        //if (e.Data.GetDataPresent(DataFormats.OemText))
        //{
        //    textBox.Text = "OemText";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Palette))
        //{
        //    textBox.Text = "Palette";
        //}
        //if (e.Data.GetDataPresent(DataFormats.PenData))
        //{
        //    textBox.Text = "PenData";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Riff))
        //{
        //    textBox.Text = "Riff";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Serializable))
        //{
        //    textBox.Text = "Serializable";
        //}
        //if (e.Data.GetDataPresent(DataFormats.StringFormat))
        //{
        //    textBox.Text = "StringFormat";
        //}
        //if (e.Data.GetDataPresent(DataFormats.SymbolicLink))
        //{
        //    textBox.Text = "SymbolicLink";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Tiff))
        //{
        //    textBox.Text = "Tiff";
        //}
        //if (e.Data.GetDataPresent(DataFormats.UnicodeText))
        //{
        //    textBox.Text = "UnicodeText";
        //}
        //if (e.Data.GetDataPresent(DataFormats.WaveAudio))
        //{
        //    textBox.Text = "WaveAudio";
        //}
        //if (e.Data.GetDataPresent(DataFormats.Xaml))
        //{
        //    textBox.Text = "XAML";
        //}
        //if (e.Data.GetDataPresent(DataFormats.XamlPackage))
        //{
        //    textBox.Text = "XamlPackage";
        //}
        //foreach (string item in e.Data.GetFormats(true))
        //{
        //    textBox.Text += item + "\n";
        //}
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

