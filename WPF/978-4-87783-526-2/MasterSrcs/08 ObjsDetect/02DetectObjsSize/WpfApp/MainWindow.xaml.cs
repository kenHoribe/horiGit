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
        private string? mObjDetector;

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

        // get Detector file name
        private void MenuItem_Open_Detector_Click(object sender, RoutedEventArgs e)
        {
            string filter = "検出器(*.xml)|*.xml;|すべてのファイル(*.*)|*.*";
            string fname = ccvfunc.GetXmlFileName(filter);

            mObjDetector = fname;
            DetectorText.Content = Path.GetFileName(mObjDetector);
        }

        // drop file
        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(DataFormats.FileDrop) is not string[] fn)
                    return;

                if (Path.GetExtension(fn[0]) == ".xml")
                {
                    mObjDetector = fn[0];
                    DetectorText.Content = Path.GetFileName(mObjDetector);
                }
                else
                {
                    OpenFile(fn[0]);
                }
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
        private void MenuItem_Common_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Image.Source == null)
                {
                    MessageBox.Show("画像が読み込まれていない", "エラー", MessageBoxButton.OK);
                    return;
                }
                if (mObjDetector == null)
                {
                    MessageBox.Show("検出器が読み込まれていない", "エラー", MessageBoxButton.OK);
                    return;
                }

                Cursor = Cursors.Wait;

                int scale = 0;
                if (sender.GetType() == typeof(MenuItem))
                {
                    MenuItem _obj = (MenuItem)sender;
                    scale = int.Parse((string)(_obj.Tag));
                }
                else
                {
                    Button _obj = (Button)sender;
                    scale = int.Parse((string)(_obj.Tag));
                }
                ccvfunc.DoCvFunction("結果", mObjDetector, scale);
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
