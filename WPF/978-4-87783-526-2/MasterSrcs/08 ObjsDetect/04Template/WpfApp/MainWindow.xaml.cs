using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private string? mTemplate;

        public MainWindow()
        {
            InitializeComponent();
        }

        // open image file
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
                SizeToContent = SizeToContent.WidthAndHeight;
            }
        }

        // open image file
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

        // get Template file name
        private void MenuItem_Open_Template_Click(object sender, RoutedEventArgs e)
        {
            mTemplate = ccvfunc.GetReadFile();
            DetectorText.Content = Path.GetFileName(mTemplate);
            if (mTemplate == null)
                ccvfunc.DesoryTemplate();
            else
                ccvfunc.ShowTemplate("Template", mTemplate);
        }

        // drop file
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

        // save as
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

        // do detect
        private void MenuItem_Exec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Image.Source == null)
                {
                    MessageBox.Show("画像が読み込まれていない", "エラー", MessageBoxButton.OK);
                    return;
                }
                if (mTemplate == null)
                {
                    MessageBox.Show("テンプレートが読み込まれていない", "エラー", MessageBoxButton.OK);
                    return;
                }

                Cursor = Cursors.Wait;
                Image.Source = ccvfunc.DoCvFunction("結果", mTemplate);
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

        // close
        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
