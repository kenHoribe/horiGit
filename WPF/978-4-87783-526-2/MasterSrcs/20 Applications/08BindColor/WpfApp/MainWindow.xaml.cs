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
        private readonly ViewModel vm;
        private Mat? iMat, oMat;

        public MainWindow()
        {
            InitializeComponent();

            vm = new ViewModel();
            DataContext = vm;
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
                vm.Img = BitmapSourceConverter.ToBitmapSource(iMat);
            }
        }

        private void MenuItem_Effect_Click(object sender, RoutedEventArgs e)
        {
            if (iMat == null)
                return;

            oMat = new();
            Cv2.BitwiseNot(iMat, oMat);                     // Negative
            vm.Img = BitmapSourceConverter.ToBitmapSource(oMat);
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
