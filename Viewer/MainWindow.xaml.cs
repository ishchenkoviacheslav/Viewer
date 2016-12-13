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
                bigCopy.Tag = (string)img.Tag;
                bigCopy.StretchDirection = StretchDirection.Both;
                bigCopy.Source = bitImg;
               
                

                Grid.SetColumn(bigCopy, 0);
                Grid.SetRowSpan(bigCopy, 2);
                FirstGrid.Children.Add(bigCopy);

                Button prev = new Button() { Content = "PREV" };
                prev.Click += Prev_Click; ;
                Grid.SetColumn(prev, 1);
                Grid.SetRow(prev, 1);
                FirstGrid.Children.Add(prev);

                Button next = new Button() { Content = "NEXT" };
                next.Click += Next_Click;
                Grid.SetColumn(next, 1);
                Grid.SetRow(next, 0);
                FirstGrid.Children.Add(next);

                this.KeyDown += BigCopy_KeyDown;

                this.Drop -= MainGrid_Drop;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            Image currImg = FirstGrid.Children.OfType<Image>().ToList().FirstOrDefault();
            for (int i = 0; i < allPictures.Count; i++)
            {
                //найти в списке нужное изображение
                if((string)currImg.Tag == (string)allPictures[i].Tag)
                {
                    //проверка на первое и последнее изобр.
                    if(i< (allPictures.Count-1) )
                    {
                        Image bigCopy = new Image();
                        BitmapImage bitImg = new BitmapImage();
                        bitImg.BeginInit();
                        bitImg.UriSource = ((BitmapImage)allPictures[i+1].Source).UriSource;
                        bitImg.EndInit();
                        bigCopy.Tag = (string)allPictures[i+1].Tag;
                        bigCopy.StretchDirection = StretchDirection.Both;
                        bigCopy.Source = bitImg;

                        Grid.SetColumn(bigCopy, 0);
                        Grid.SetRowSpan(bigCopy, 2);
                        FirstGrid.Children.RemoveAt(0);
                        FirstGrid.Children.Insert(0,bigCopy);

                        return;
                    }
                }
            }
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            Image currImg = FirstGrid.Children.OfType<Image>().ToList().FirstOrDefault();
            for (int i = 0; i < allPictures.Count; i++)
            {
                //найти в списке нужное изображение
                if ((string)currImg.Tag == (string)allPictures[i].Tag)
                {
                    //проверка на первое и последнее изобр.
                    if (i>0)
                    {
                        Image bigCopy = new Image();
                        BitmapImage bitImg = new BitmapImage();
                        bitImg.BeginInit();
                        bitImg.UriSource = ((BitmapImage)allPictures[i - 1].Source).UriSource;
                        bitImg.EndInit();
                        bigCopy.Tag = (string)allPictures[i - 1].Tag;
                        bigCopy.StretchDirection = StretchDirection.Both;
                        bigCopy.Source = bitImg;

                        Grid.SetColumn(bigCopy, 0);
                        Grid.SetRowSpan(bigCopy, 2);
                        FirstGrid.Children.RemoveAt(0);
                        FirstGrid.Children.Insert(0, bigCopy);

                        return;
                    }
                }
            }
        }

        private void BigCopy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                Image currImg = FirstGrid.Children.OfType<Image>().ToList().FirstOrDefault();
                for (int i = 0; i < allPictures.Count; i++)
                {
                    //найти в списке нужное изображение
                    if ((string)currImg.Tag == (string)allPictures[i].Tag)
                    {
                        //проверка на первое и последнее изобр.
                        if (i > 0)
                        {
                            Image bigCopy = new Image();
                            BitmapImage bitImg = new BitmapImage();
                            bitImg.BeginInit();
                            bitImg.UriSource = ((BitmapImage)allPictures[i - 1].Source).UriSource;
                            bitImg.EndInit();
                            bigCopy.Tag = (string)allPictures[i - 1].Tag;
                            bigCopy.StretchDirection = StretchDirection.Both;
                            bigCopy.Source = bitImg;

                            Grid.SetColumn(bigCopy, 0);
                            Grid.SetRowSpan(bigCopy, 2);
                            FirstGrid.Children.RemoveAt(0);
                            FirstGrid.Children.Insert(0, bigCopy);

                            return;
                        }
                    }
                }
            }

            if (e.Key == Key.Up)
            {
                Image currImg = FirstGrid.Children.OfType<Image>().ToList().FirstOrDefault();
                for (int i = 0; i < allPictures.Count; i++)
                {
                    //найти в списке нужное изображение
                    if ((string)currImg.Tag == (string)allPictures[i].Tag)
                    {
                        //проверка на первое и последнее изобр.
                        if (i < (allPictures.Count - 1))
                        {
                            Image bigCopy = new Image();
                            BitmapImage bitImg = new BitmapImage();
                            bitImg.BeginInit();
                            bitImg.UriSource = ((BitmapImage)allPictures[i + 1].Source).UriSource;
                            bitImg.EndInit();
                            bigCopy.Tag = (string)allPictures[i + 1].Tag;
                            bigCopy.StretchDirection = StretchDirection.Both;
                            bigCopy.Source = bitImg;

                            Grid.SetColumn(bigCopy, 0);
                            Grid.SetRowSpan(bigCopy, 2);
                            FirstGrid.Children.RemoveAt(0);
                            FirstGrid.Children.Insert(0, bigCopy);

                            return;
                        }
                    }
                }
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
                    NewImage.MouseDown += NewImage_MouseDown;

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
    }
}

