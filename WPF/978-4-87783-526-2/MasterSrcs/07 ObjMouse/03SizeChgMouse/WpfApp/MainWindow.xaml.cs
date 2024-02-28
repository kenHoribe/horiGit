using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using CCvLibrary;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CCvFunc ccvfunc = new();
        private Point mStartPoint;

        /// 描画中のオブジェクト
        private Rectangle? currentRect = null;
        readonly List<Rectangle> mRectglObjsList = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        // open file
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

                DrawCanvas.Width = bmpSrc.Width;
                DrawCanvas.Height = bmpSrc.Height;

                RemoveChildrenRectangle();
            }
        }

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

        private void MenuItem_Common_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Image.Source == null)
                    return;

                Cursor = Cursors.Wait;

                ccvfunc.DoCvFunction("結果", mRectglObjsList, ScaleText.Text);
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

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // mouse buton down
        private void DrawCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Image.Source == null)
                    return;

                if (e.RightButton == MouseButtonState.Pressed)
                {
                    RemoveRectangle(new Point(e.GetPosition(DrawCanvas).X,
                                                   e.GetPosition(DrawCanvas).Y));
                }
                else
                {
                    if (e.LeftButton != MouseButtonState.Pressed)
                        return;

                    DrawCanvas.CaptureMouse();
                    DrawCanvas.MouseMove += new MouseEventHandler(DrawCanvas_MouseMove);
                    DrawCanvas.MouseUp += new MouseButtonEventHandler(DrawCanvas_MouseUp);

                    // maintains the coordinates of mouse clicks
                    mStartPoint = e.GetPosition(Image);

                    // creation of drawing objects
                    currentRect = new Rectangle
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1
                    };
                    Canvas.SetLeft(currentRect, mStartPoint.X);
                    Canvas.SetTop(currentRect, mStartPoint.Y);

                    // adding Rectanble objects to the Canvas
                    DrawCanvas.Children.Add(currentRect);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DrawCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (currentRect == null)
                return;

            // normalize object coord.
            System.Windows.Point mousePoint = e.GetPosition(DrawCanvas);
            double x = Math.Min(mousePoint.X, mStartPoint.X);
            x = Math.Max(x, 0);
            double y = Math.Min(mousePoint.Y, mStartPoint.Y);
            y = Math.Max(y, 0);
            double width = Math.Max(mousePoint.X, mStartPoint.X) - x;
            double height = Math.Max(mousePoint.Y, mStartPoint.Y) - y;

            width = x + width > DrawCanvas.Width ? DrawCanvas.Width - x: width;
            height = y + height > DrawCanvas.Height ? DrawCanvas.Height - y : height;

            // update object pos & size
            currentRect.Width = width;
            currentRect.Height = height;
            Canvas.SetLeft(currentRect, x);
            Canvas.SetTop(currentRect, y);
        }

        private void DrawCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (currentRect != null)
            {
                System.Windows.Point current = e.GetPosition(DrawCanvas);
                var width = Math.Abs(current.X - mStartPoint.X);
                var height = Math.Abs(current.Y - mStartPoint.Y);

                if (width < 5 || height < 5) // ignore narrow range designations
                {
                    DrawCanvas.Children.Remove(currentRect);
                }
                else
                {
                    mRectglObjsList.Add(currentRect);
                }
                currentRect = null;     // 描画中オブジェクトの参照を削除
            }

            DrawCanvas.MouseMove -= new MouseEventHandler(DrawCanvas_MouseMove);
            DrawCanvas.MouseUp -= new MouseButtonEventHandler(DrawCanvas_MouseUp);
            DrawCanvas.ReleaseMouseCapture();
        }

        // Undo, remove last one
        private void Button_Undo_Click(object sender, RoutedEventArgs e)
        {
            if (mRectglObjsList.Count == 0)
                return;

            DrawCanvas.Children.Remove(mRectglObjsList.Last());
            mRectglObjsList.Remove(mRectglObjsList.Last());
        }

        // remove target Rectangle
        public void RemoveRectangle(System.Windows.Point pt)
        {
            foreach (Rectangle r in mRectglObjsList)
            {
                if (pt.X >= Canvas.GetLeft(r) &&
                    pt.X <= Canvas.GetLeft(r) + r.Width &&
                    pt.Y >= Canvas.GetTop(r) &&
                    pt.Y <= Canvas.GetTop(r) + r.Height)
                {
                    mRectglObjsList.Remove(r);
                    DrawCanvas.Children.Remove(r);
                    return;
                }
            }
        }

        // remove all children Rectangle
        public void RemoveChildrenRectangle()
        {
            foreach (Rectangle r in mRectglObjsList)
            {
                DrawCanvas.Children.Remove(r);
            }
            mRectglObjsList.Clear();
        }

    }
}
