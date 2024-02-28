using OpenCvSharp;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int rows = 200, cols = 300;
            Mat img = new Mat(rows, cols,
                                    MatType.CV_8UC3, Scalar.Cyan);

            img.Line(new OpenCvSharp.Point(10, 10),                 // pt1
                      new OpenCvSharp.Point(cols - 10, rows - 10),  // pt2
                                    Scalar.Blue);                   // color

            ClientSize = new System.Drawing.Size(cols, rows);
            using (Bitmap bmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(img))
            using (Graphics myGraphics = Graphics.FromHwnd(this.Handle))
            {
                myGraphics.DrawImage(bmp, 0, 0);
            }

        }
    }
}