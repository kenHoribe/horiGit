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

        // open file
        private void OpenFile(string? fname)
        {
            BitmapSource? bmpSrc;
            if (ccvfunc is not null)
            {
                string? nfname;
                (nfname, bmpSrc) = ccvfunc.OpenFileCv(fname);
                if (bmpSrc == null)
                    return;

                Image0.Source = bmpSrc;
                Image1.Source = ccvfunc.DoCvFunction();
                StatusBarText.Content = System.IO.Path.GetFileName(nfname);
            }
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFile(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(DataFormats.FileDrop) is not string[] fn)
                    return;

                OpenFile(fn[0]);
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
            if (ccvfunc is not null)
            {
                ccvfunc.Scale = (float)Image0.ActualHeight / (float)Image0.Source.Height;
                Image1.Source = ccvfunc.DoCvFunction();
            }
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
