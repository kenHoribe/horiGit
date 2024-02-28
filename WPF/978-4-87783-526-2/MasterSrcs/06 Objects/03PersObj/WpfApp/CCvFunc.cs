using System;

using OpenCvSharp;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        private Point2f[]? mPsrc;           // perspective source
        private Point2f[]? mPdst;           // perspective destination
        public float Scale { get; set; }    // scaling

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        public void DoCvFunction(string fn, string size)
        {
            if (mSrc == null)
                return;

            mDst = mSrc.Clone();
            char[] delimitter = { 'X', 'x' };   // delimitter
            string[] resolutions = size.Split(delimitter);

            int persWidth = Convert.ToInt32(resolutions[0]);
            int persHeight = Convert.ToInt32(resolutions[1]);

            Mat gray = new();
            Cv2.CvtColor(mSrc, gray, ColorConversionCodes.RGB2GRAY);
            Cv2.Threshold(gray, gray, 128, 255, ThresholdTypes.Binary);
            Cv2.FindContours(gray, out Point[][] contours,
                            out _, RetrievalModes.Tree,
                                ContourApproximationModes.ApproxTC89L1);

            Point[][] tmpContours = Array.Empty<Point[]>();
            for (int i = 0; i < contours.Length; i++)
            {
                double a = Cv2.ContourArea(contours[i], false);
                if (a > 50 * 50)        // only an area of 50 x 50 or more
                {
                    Point[] approx;     // contour to a straight line
                    approx = Cv2.ApproxPolyDP(contours[i],
                                        0.01 * Cv2.ArcLength(contours[i], true), true);
                    if (approx.Length == 4) // rectangle only
                    {
                        tmpContours = new Point[][] { approx };
                        break;          // only first one
                    }
                }
            }
            mDst = new Mat(persHeight, persWidth, mSrc.Type(), Scalar.LightCoral);
            if (tmpContours.Length == 0)
            {
                System.Windows.MessageBox.Show("オブジェクトが見つからない", "エラー",
                                                System.Windows.MessageBoxButton.OK);
                return;
            }

            mPsrc = new Point2f[4];     // perspective source
            for (int i = 0; i < mPsrc.Length; i++)
            {
                mPsrc[i] = (Point2f)tmpContours[0][i];
            }
            SortSrcPoints(mPsrc);

            mPdst = new Point2f[] {     // perspective destination
                new Point2f(0.0f, 0.0f),
                new Point2f(0.0f, (float)(persHeight - 1)),
                new Point2f((float)(persWidth - 1), (float)(persHeight - 1)),
                new Point2f((float)(persWidth - 1), 0.0f)
                };
            mDst = DoPers(mSrc, mDst, mPsrc, mPdst);

            Mat dispDst = new();
            Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
            Cv2.ImShow(fn, dispDst);
        }

        // Do pers
        private static Mat DoPers(Mat mSrc, Mat mDst, Point2f[] mPsrc, Point2f[] mPdst)
        {
            Mat dst = new();
            Mat persMatrix = Cv2.GetPerspectiveTransform(mPsrc, mPdst);
            Cv2.WarpPerspective(mSrc, dst, persMatrix, mDst.Size(), InterpolationFlags.Cubic);
            return dst;
        }

        // sort
        //     |
        //  0  |  3
        //     |
        // ----+-----
        //     |
        //  1  |  2
        //     |
        //
        private static void SortSrcPoints(Point2f[] points)
        {
            for (int j = 3; j > 0; j--)             // Sort by X
            {
                for (int i = 0; i < j; i++)
                {
                    if (points[i].X > points[i + 1].X)
                        (points[i], points[i + 1]) = (points[i + 1], points[i]);
                }
            }

            if (points[0].Y > points[1].Y)          // Sort the first two by Y
                (points[0], points[1]) = (points[1], points[0]);
            if (points[2].Y < points[3].Y)          // Reverse sort the first two by Y
                (points[2], points[3]) = (points[3], points[2]);
        }

        // rotate it to right 90 degree
        public void Rotated(string fn)
        {
            if (mPsrc != null && mPdst != null && mSrc != null && mDst != null)
            {
                (mPsrc[0], mPsrc[1], mPsrc[2], mPsrc[3]) =
                            (mPsrc[1], mPsrc[2], mPsrc[3], mPsrc[0]);

                mDst = DoPers(mSrc, mDst, mPsrc, mPdst);

                Mat dipDst = new();
                Cv2.Resize(mDst, dipDst, new OpenCvSharp.Size(), Scale, Scale);
                Cv2.ImShow(fn, dipDst);
            }
        }

    }
}
