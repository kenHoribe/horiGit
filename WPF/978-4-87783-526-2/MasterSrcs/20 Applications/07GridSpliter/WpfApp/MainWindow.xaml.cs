using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using CCvLibrary;

namespace WpfApp
{
    public class Img
    {
        public string? FullUrl { get; set; }
        public string? Url { get; set; }
        public string? Result { get; set; }
        public string? judge { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CCvFunc ccvfunc = new();
        private List<String> mSrcName = new();

        public ObservableCollection<Img> Imgs { get; } = new ObservableCollection<Img>();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        // 比較先の画像
        private void LImages_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(DataFormats.FileDrop) is not string[] fn)
                    return;

                foreach (var file in fn)
                {
                    if (Directory.Exists(file))
                        continue;

                    var _img = new Img
                    {
                        FullUrl = file,
                        Url = Path.GetFileName(file),
                        Result = "----",
                        judge = "No"
                    };
                    Imgs.Add(_img);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 比較元の画像
        private void Image0_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(DataFormats.FileDrop) is not string[] fn)
                    return;

                BitmapSource? bmpSrc;
                (_, bmpSrc) = ccvfunc.OpenFileCv(fn[0]);
                if (bmpSrc == null)
                    return;

                Image0.Source = bmpSrc;
                StatusBarText.Content = System.IO.Path.GetFileName(fn[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // 探す
        private void MenuItem_Find_Click(object sender, RoutedEventArgs e)
        {
            if (Imgs.Count <= 0 || Image0.Source == null)
                return;

            double min = Double.MaxValue;
            int minIndex = -1;
            foreach (var it in Imgs)
            {
                it.judge = "No";
                double result = ccvfunc.DoCvFunction(it.FullUrl!);
                if (result < 0d)
                {
                    it.Result = "not same size";
                }
                else
                {
                    it.Result = result.ToString("0.0");
                    if (result < min)
                    {
                        min = result;
                        minIndex = Imgs.IndexOf(it);
                    }
                }
            }
            if (minIndex >= 0)
                Imgs[minIndex].judge = "Yes";

            //表示更新
            LImages.ClearValue(ItemsControl.ItemsSourceProperty);
            LImages.ItemsSource = Imgs;
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // remove selected items
        private void MenuItem_Remove_Click(object sender, RoutedEventArgs e)
        {
            if (LImages.SelectedItem != null)
            {
                List<Img> removeList = new List<Img>();
                foreach (Img it in LImages.SelectedItems)
                {
                    removeList.Add(it);
                }
                foreach (Img it in removeList)
                {
                    Imgs.Remove(it);
                }
            }
        }

        // clear all
        private void MenuItem_ClearAll_Click(object sender, RoutedEventArgs e)
        {
            Imgs.Clear();
        }
    }
}
