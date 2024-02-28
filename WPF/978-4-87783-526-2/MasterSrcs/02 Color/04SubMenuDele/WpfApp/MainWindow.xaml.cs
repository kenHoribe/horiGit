using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
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
        private readonly Dictionary<string, Action<object, RoutedEventArgs>> MIDeleDict;

        public MainWindow()
        {
            InitializeComponent();

            // メニュー項目名とメニュー処理デリゲートを辞書に登録
            MIDeleDict = new Dictionary<string, Action<object, RoutedEventArgs>>
            {
                { Negative.Name,     NegativeDele       },
                { Grayscale.Name,    GrayscaleDele      },
                { EqualizeHist.Name, EqualizeHist3Dele  },
                { Threshold.Name,    ThresholdDele      }
            };
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

            // メニュー項目名からメニュー処理デリゲートを取得
            MenuItem menuItem = (MenuItem)sender;
            if (!MIDeleDict.ContainsKey(menuItem.Name)) return;
            var menuItemDelegate = MIDeleDict[menuItem.Name];

            oMat = new();
            menuItemDelegate(sender, e);    // デリゲートを実行

            Image.Source = BitmapSourceConverter.ToBitmapSource(oMat);
        }

        // Negative
        private void NegativeDele(object sender, RoutedEventArgs e)
        {
            if (iMat is not null && oMat is not null)
            {
                Cv2.BitwiseNot(iMat, oMat);
            }
        }

        // Grayscale
        private void GrayscaleDele(object sender, RoutedEventArgs e)
        {
            if (iMat is not null && oMat is not null)
            {

                Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);
            }
        }

        // 輝度平滑化
        private void EqualizeHist3Dele(object sender, RoutedEventArgs e)
        {
            if (iMat is not null && oMat is not null)
            {
                Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(oMat, oMat);
            }
        }

        // 閾値処理
        private void ThresholdDele(object sender, RoutedEventArgs e)
        {
            if (iMat is not null && oMat is not null)
            {
                Cv2.CvtColor(iMat, oMat, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(oMat, oMat, 80.0, 210.0, ThresholdTypes.Binary);
            }
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
