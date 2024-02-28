using System;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using OpenCvSharp.Extensions;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        private readonly List<List<OpenCvSharp.Point>> mPsrc;   // perspective source
        private readonly List<List<OpenCvSharp.Point>> mPdst;   // perspective destination
        private int mPersWidth, mPersHeight;                    // pars先のサイズ
        public float Scale { get; set; }                        // scaling

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
            mPsrc = new List<List<OpenCvSharp.Point>>           // perspective source
                            { new List<OpenCvSharp.Point>() };
            mPdst = new List<List<OpenCvSharp.Point>>           // perspective destination
                            { new List<OpenCvSharp.Point>() };
        }

        //----------------------------------------------------------------
        // ファイルを開く
        //    base.OpenFileCvをoverride、座標クリアを追加
        public override (string?, BitmapSource?) OpenFileCv(string? fname)
        {
            mPsrc[0].Clear();               //座標クリア
            return base.OpenFileCv(fname);  //元のメソッドを呼ぶ
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public void DoCvFunction(string fn, string size)
        {
            if (mSrc == null)
                return;

            if (mPsrc[0].Count < 4)
            {
                System.Windows.MessageBox.Show("マウスで座標を指定してください",
                                    "エラー", System.Windows.MessageBoxButton.OK);

                return;
            }

            char[] delimitter = { 'X', 'x' }; // delimitter
            string[] resolutions = size.Split(delimitter);

            mPersWidth  = Convert.ToInt32(resolutions[0]);
            mPersHeight = Convert.ToInt32(resolutions[1]);

            Mat mDst = DoPers(mPersWidth, mPersHeight, mPsrc);
            Mat dispDst = new();
            Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
            Cv2.ImShow(fn, dispDst);
        }

        // Do pers
        //  PointなどWindowsとOpenCVで競合するものがあるので注意すること。
        private Mat DoPers(int mPersWidth, int mPersHeight,
                                            List<List<OpenCvSharp.Point>> mPsrc)
        {
            mDst = new Mat(mPersHeight, mPersWidth, mSrc!.Type());

            Point2f[] pSrc = new Point2f[4];    // perspective source
            for (int i = 0; i < mPsrc[0].Count; i++)
            {
                pSrc[i] = (Point2f)mPsrc[0][i];
            }

            Point2f[] pDst = new Point2f[] {  // perspective destination
                new Point2f(0.0f, 0.0f),
                new Point2f(0.0f, (float)(mPersHeight - 1)),
                new Point2f((float)(mPersWidth - 1), (float)(mPersHeight - 1)),
                new Point2f((float)(mPersWidth - 1), 0.0f)
                };
            Mat persMatrix = Cv2.GetPerspectiveTransform(pSrc, pDst);
            Cv2.WarpPerspective(mSrc, mDst, persMatrix, mDst.Size(),
                                                    InterpolationFlags.Cubic);
            return mDst;
        }

        // get current bitmap
        //    window point to opencv pointへ
        public BitmapSource MouseDown(System.Windows.Point pt)
        {
            OpenCvSharp.Point p = new(pt.X / Scale, pt.Y / Scale);

            if (mPsrc[0].Count < 4)
            {
                mPsrc[0].Add(new OpenCvSharp.Point(p.X, p.Y));
            }
            else
            {
                var tgt = 0;
                var distance = Math.Sqrt((Math.Pow(p.X - mPsrc[0][tgt].X, 2)
                                    + Math.Pow(p.Y - mPsrc[0][tgt].Y, 2)));
                for (var i = 1; i < mPsrc[0].Count; i++)
                {
                    var NextDistance = Math.Sqrt((Math.Pow(p.X - mPsrc[0][i].X, 2)
                                        + Math.Pow(p.Y - mPsrc[0][i].Y, 2)));
                    if (distance > NextDistance)
                    {
                        distance = NextDistance;
                        tgt = i;
                    }
                }
                mPsrc[0][tgt] = new OpenCvSharp.Point(p.X, p.Y);
            }
            Bitmap bmp =GetRectsOnBmp(mSrc!);
            return BitmapSourceConverter.ToBitmapSource(bmp);
        }

        // clears the coordinates specified by the mouse
        public BitmapSource Clear()
        {
            mPsrc[0].Clear();
            return BitmapSourceConverter.ToBitmapSource(GetRectsOnBmp(mSrc!));
        }

        // image has been resized to show the original image
        public BitmapSource Resized(float scale)
        {
            Scale = scale;
            return BitmapSourceConverter.ToBitmapSource(GetRectsOnBmp(mSrc!));
        }

        // get current bitmap
        private System.Drawing.Bitmap GetRectsOnBmp(Mat img)
        {
            Bitmap bmp;
            using (Mat dst = img.Clone())
            {
                if (mPsrc[0].Count > 0)
                {
                    foreach (var point in mPsrc.First())
                    {
                        Cv2.Circle(dst, point, 3, Scalar.Red, -1);
                    }
                    if (mPsrc[0].Count == 4)
                    {
                        SortSrcPoints(mPsrc[0]);
                        Cv2.Polylines(dst, mPsrc, true, Scalar.Red);
                    }
                }
                bmp = BitmapConverter.ToBitmap(dst);
            }
            return bmp;
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
        private static void SortSrcPoints(List<OpenCvSharp.Point> p)
        {
            p.Sort((a, b) => a.X - b.X);    // sort by X

            if (p[0].Y > p[1].Y)            // Sort the first two by Y
                (p[0], p[1]) = (p[1], p[0]);
            if (p[2].Y < p[3].Y)            // Reverse sort the first two by Y
                (p[2], p[3]) = (p[3], p[2]);
        }

        // Rotate the image 90 degrees to the right, and show it
        public void Rotated(string fn)
        {
            if (mPsrc != null && mPdst != null && mSrc != null && mDst != null &&
                mPsrc[0].Count == 4)
            {
                (mPsrc[0][0], mPsrc[0][1], mPsrc[0][2], mPsrc[0][3]) =
                            (mPsrc[0][1], mPsrc[0][2], mPsrc[0][3], mPsrc[0][0]);

                Mat mDst = DoPers(mPersWidth, mPersHeight, mPsrc);
                Mat dispDst = new();
                Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
                Cv2.ImShow(fn, dispDst);
            }
        }

    }
}
