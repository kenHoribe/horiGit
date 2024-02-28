using System.Windows;
using Microsoft.Win32;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //public partial class MainWindow : Window              //OpenCVと競合
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

        private void MenuItem_Effect_Click(object sender, RoutedEventArgs e)
        {
            if (iMat == null)
                return;

            oMat = new();
            Cv2.BitwiseNot(iMat, oMat);                                     // Negative
            //-------------
            //Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);      // Grayscale
            //-------------
            //Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);      // 輝度平滑化
            //Cv2.EqualizeHist(oMat, oMat);                                 // 輝度平滑化
            //-------------
            //Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);      // 閾値処理
            //Cv2.Threshold(oMat, oMat, 80.0, 210.0, ThresholdTypes.Binary);// 閾値処理
            //-------------
            Image.Source = BitmapSourceConverter.ToBitmapSource(oMat);
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
