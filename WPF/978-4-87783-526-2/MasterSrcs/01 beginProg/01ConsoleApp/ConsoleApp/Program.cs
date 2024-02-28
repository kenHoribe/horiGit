
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int rows = 200, cols = 300;
            OpenCvSharp.Mat img = new OpenCvSharp.Mat(rows, cols,
                                    OpenCvSharp.MatType.CV_8UC3, OpenCvSharp.Scalar.Cyan);

            img.Line(new OpenCvSharp.Point(10, 10),                 // pt1
                      new OpenCvSharp.Point(cols - 10, rows - 10),  // pt2
                                    OpenCvSharp.Scalar.Blue);       // color

            var wname = "begin";
            OpenCvSharp.Cv2.NamedWindow(wname, OpenCvSharp.WindowFlags.AutoSize);
            OpenCvSharp.Cv2.ImShow(wname, img);

            OpenCvSharp.Cv2.WaitKey();
        }
    }
}
