using System.Collections.Generic;

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
        // OpenCVを使用して処理
        public void DoCvFunction()
        {
            if (mSrc == null)
                return;

            //CalcHist
            mDst = new Mat(400, 256*2, MatType.CV_8UC3, Scalar.White);
            var hist = new Mat[3];
            Scalar[] color = new Scalar[] { Scalar.Blue, Scalar.Green, Scalar.Red };
            int[] _hdims = { 256 };
            Rangef[] _ranges = { new Rangef(0, 256), };
            for (int ch = 0; ch < hist.Length; ch++)
            {
                hist[ch] = new Mat();
                Cv2.CalcHist(new Mat[] { mSrc }, new int[] { ch },
                                    null, hist[ch], 1, _hdims, _ranges);
                Cv2.Normalize(hist[ch], hist[ch], 0, mDst.Height, NormTypes.MinMax);
                DrawHist(mDst, hist[ch], color[ch]);
            }
            Cv2.ImShow("ヒストグラム", mDst);
        }

        // draw
        private void DrawHist(Mat histMat, Mat hist, Scalar color)
        {
            List<List<Point>> LLPoint = new List<List<Point>>();
            List<Point> LPoint = new List<Point>();

            for (int i = 0; i < 256; i++)
            {
                float v = hist.At<float>(i, 0);
                var _bin = histMat.Width / 256;
                LPoint.Add(new Point(i * _bin, histMat.Height - v - 1));
            }
            LLPoint.Add(LPoint);
            mDst!.Polylines(LLPoint, false, color);
        }
    }
}
