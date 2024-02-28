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

        private void Common_Click(object sender, RoutedEventArgs e)
        {
            if (iMat == null)
                return;

            oMat = iMat.Clone();

            switch (((MenuItem)sender).Name)
            {
                case "Lines":
                    int x0 = oMat.Width / 4;
                    int x1 = oMat.Width * 3 / 4;
                    int y0 = oMat.Height / 4;
                    int y1 = oMat.Height * 3 / 4;
                    Cv2.Line(oMat, x0, y0, x1, y1, new Scalar(0, 0, 255), 3, LineTypes.Link4);
                    Cv2.Line(oMat, x1, y0, x0, y1, new Scalar(255, 0, 0), 3, LineTypes.Link4);
                    break;

                case "Circles":
                    OpenCvSharp.Point center = new(oMat.Width / 2, oMat.Height / 2);
                    Cv2.Circle(oMat, center, oMat.Height / 3, new Scalar(0, 255, 0), 3);
                    Cv2.Circle(oMat, center, oMat.Height / 6, new Scalar(255, 255, 0), -1);
                    break;

                case "Rectangle":
                    OpenCvSharp.Point p0 = new(oMat.Width / 8, oMat.Height / 8);
                    OpenCvSharp.Point p1 = new (oMat.Width * 7 / 8, oMat.Height * 7 / 8);
                    Cv2.Rectangle(oMat, p0, p1, new Scalar(0, 255, 0), 5, LineTypes.Link8);
                    OpenCvSharp.Point p2 = new (oMat.Width * 2 / 8, oMat.Height * 2 / 8);
                    OpenCvSharp.Point p3 = new (oMat.Width * 6 / 8, oMat.Height * 6 / 8);
                    Cv2.Rectangle(oMat, p2, p3, new Scalar(0, 255, 255), 4, LineTypes.AntiAlias);
                    break;

                case "Ecllipse":
                    OpenCvSharp.Point Ecenter = new (oMat.Width / 2, oMat.Height / 2);
                    OpenCvSharp.Size sz = new (oMat.Width / 2, oMat.Height / 2);
                    Cv2.Ellipse(oMat, Ecenter, sz, 0, 0, 360,
                                                new Scalar(255, 0, 0), 3, LineTypes.Link4);
                    sz.Width -= 20;
                    sz.Height -= 50;
                    Cv2.Ellipse(oMat, Ecenter, sz, 15, 10, 300,
                                                new Scalar(255, 255, 0), 2, LineTypes.Link4);
                    break;

                case "String":
                    OpenCvSharp.Point p = new (50, oMat.Height / 2 - 50);
                    Cv2.PutText(oMat, "Hello OpenCV", p, HersheyFonts.HersheyTriplex,
                                    0.8, new Scalar(250, 200, 200), 2, LineTypes.AntiAlias);
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
            Image.Source = BitmapSourceConverter.ToBitmapSource(oMat);
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
