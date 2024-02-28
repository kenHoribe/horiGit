using System;
using System.Windows;
using System.Windows.Media.Imaging;

using CCvLibrary;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CCvFunc ccvfunc = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(DataFormats.FileDrop) is not string[] fn)
                    return;

                BitmapSource? bmpSrc;
                (_, bmpSrc) = ccvfunc.OpenFileCv(fn[0]);
                if (bmpSrc == null)
                    return;

                Image.Source = bmpSrc;
                Image_SizeChanged(sender, null!);   // for same size
                StatusBarText.Content = System.IO.Path.GetFileName(fn[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_SvaeAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ccvfunc.SaveAS();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Image.ActualHeight != 0)
            {
                ccvfunc.Scale = (float)Image.ActualHeight / (float)Image.Source.Height;
                ccvfunc.DoCvFunction("結果");
            }
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
