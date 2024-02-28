using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using CCvLibrary;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CCvFunc ccvfunc = new();
        private readonly List<String> mSrcName = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DockPanel_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] fnames = (string[])e.Data.GetData(
                                        DataFormats.FileDrop, false);

                mSrcName.Clear();
                for (int i = 0; i < fnames.Length; i++)
                {
                    mSrcName.Add(fnames[i]);
                }
                Image.Source = ccvfunc.DoCvFunction(mSrcName);
                StatusBarText.Content = "OK";
            }
            catch (Exception ex)
            {
                StatusBarText.Content = "NG";
                Image.Source = null;
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
