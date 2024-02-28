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
                Mat iMat = new Mat(dialog.FileName);
                Image.Source = BitmapSourceConverter.ToBitmapSource(iMat);
                SizeToContent = SizeToContent.WidthAndHeight;
            }
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
