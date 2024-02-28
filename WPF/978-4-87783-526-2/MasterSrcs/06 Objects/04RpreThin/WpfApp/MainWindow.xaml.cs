using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

                Image.Source = bmpSrc;
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

        private void MenuItem_Common_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Image.Source == null)
                    return;

                Cursor = Cursors.Wait;

                MenuItem mi = (MenuItem)sender;
                ccvfunc.DoCvFunction("結果", Int32.Parse((string)mi.Tag));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void CmbDMode_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Image == null || Image.Source == null)      // 読み込んでいるか
                    return;

                Stretch[] mode = { Stretch.Uniform, Stretch.None };
                ScrollBarVisibility[] scv = new ScrollBarVisibility[] {
                    ScrollBarVisibility.Disabled, ScrollBarVisibility.Visible };

                Image.Source = ccvfunc.GetOrgBitmapSource();

                Image.Stretch = mode[cDMode.SelectedIndex];
                Scroll.HorizontalScrollBarVisibility =
                    Scroll.VerticalScrollBarVisibility = scv[cDMode.SelectedIndex];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ccvfunc is not null)
                ccvfunc.Scale = (float)Image.ActualHeight / (float)Image.Source.Height;
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
