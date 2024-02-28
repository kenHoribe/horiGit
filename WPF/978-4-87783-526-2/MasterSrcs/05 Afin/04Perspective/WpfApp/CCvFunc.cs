using OpenCvSharp;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理&表示
        public void DoCvFunction(string fn, int pattern)
        {
            if (mSrc == null)
                return;

            float x0 = (float)(mSrc.Cols / 4);
            float x1 = (float)((mSrc.Cols / 4) * 3);
            float y0 = (float)(mSrc.Rows / 4);
            float y1 = (float)((mSrc.Rows / 4) * 3);

            float xMergin = mSrc.Cols / 10;
            float yMergin = mSrc.Rows / 10;

            Point2f[] srcPoint = new Point2f[] {new Point2f(x0, y0),
                                                new Point2f(x0, y1),
                                                new Point2f(x1, y1),
                                                new Point2f(x1, y0)
                    };
            Point2f[] dstPoint = new Point2f[4];

            switch (pattern)
            {
                case 0:
                    dstPoint[0] = new Point2f(x0 + xMergin, y0 + yMergin);
                    dstPoint[1] = srcPoint[1];
                    dstPoint[2] = srcPoint[2];
                    dstPoint[3] = new Point2f(x1 - xMergin, y0 + yMergin);
                    break;

                case 1:
                    dstPoint[0] = srcPoint[0];
                    dstPoint[1] = new Point2f(x0 + xMergin, y1 - yMergin);
                    dstPoint[2] = new Point2f(x1 - xMergin, y1 - yMergin);
                    dstPoint[3] = srcPoint[3];
                    break;

                case 2:
                    dstPoint[0] = srcPoint[0];
                    dstPoint[1] = new Point2f(x0 + xMergin, y1 - yMergin);
                    dstPoint[2] = srcPoint[2];
                    dstPoint[3] = new Point2f(x1 - xMergin, y0 + yMergin);
                    break;
            }
            Mat perspectiveMmat = Cv2.GetPerspectiveTransform(srcPoint, dstPoint);
            mDst = new Mat();
            Cv2.WarpPerspective(mSrc, mDst, perspectiveMmat, mSrc.Size(), InterpolationFlags.Cubic);
            Cv2.ImShow(fn, mDst);
        }
    }
}
