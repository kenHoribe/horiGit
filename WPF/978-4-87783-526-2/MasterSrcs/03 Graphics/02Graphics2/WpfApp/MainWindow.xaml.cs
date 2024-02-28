using System.Collections.Generic;
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

            var xUnit = oMat.Width / 8;
            var yUnit = oMat.Height / 8;
            List<List<OpenCvSharp.Point>> LLPoint2D = new ()
            {
                new ()
                {
                    new (4*xUnit,1*yUnit),
                    new (7*xUnit,6*yUnit),
                    new (1*xUnit,6*yUnit)
                },
                new ()
                {
                    new (1*xUnit,2*yUnit),
                    new (7*xUnit,2*yUnit),
                    new (4*xUnit,7*yUnit)
                }
            };

            switch (((MenuItem)sender).Name)
            {
                case "Polylines":
                    LLPoint2D.RemoveAt(LLPoint2D.Count - 1);
                    Cv2.Polylines(oMat, LLPoint2D, true, Scalar.LimeGreen, 2);
                    break;

                case "Polylines2":
                    Cv2.Polylines(oMat, LLPoint2D, true, Scalar.Green, 3);
                    break;

                case "FillPoly":
                    LLPoint2D.RemoveAt(LLPoint2D.Count - 1);
                    Cv2.FillPoly(oMat, LLPoint2D, Scalar.HotPink);
                    break;

                case "FillPoly2":
                    Cv2.FillPoly(oMat, LLPoint2D, Scalar.Pink);
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
