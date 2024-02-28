using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private Mat? iMat, oMat;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "全てのファイル (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                iMat = new Mat(dialog.FileName);
                Image.Source = BitmapSourceConverter.ToBitmapSource(iMat);
                SizeToContent = SizeToContent.WidthAndHeight;
            }
        }

        private void MenuItem_Common_Click(object sender, RoutedEventArgs e)
        {
            if (iMat == null)
                return;

            oMat = new();

            switch(((MenuItem)sender).Name)
            {
                case "Negative":                // Negative
                    Cv2.BitwiseNot(iMat, oMat);
                    break;

                case "Grayscale":               // Grayscale
                    Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);
                    break;

                case "EqualizeHist":            // 輝度平滑化
                    Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);
                    Cv2.EqualizeHist(oMat, oMat);
                    break;

                case "Threshold":               // 閾値処理
                    Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);
                    Cv2.Threshold(oMat, oMat, 80.0, 210.0, ThresholdTypes.Binary);
                    break;

                default:
                    return;
            }
            Image.Source = BitmapSourceConverter.ToBitmapSource(oMat);
        }

        private void MenuItem_Undo_Click(object sender, RoutedEventArgs e)
        {
            if (iMat == null)
                return;

            oMat = iMat.Clone();
            Image.Source = BitmapSourceConverter.ToBitmapSource(oMat);      // Undo
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
