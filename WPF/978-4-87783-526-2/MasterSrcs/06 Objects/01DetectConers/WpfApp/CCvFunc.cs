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

            mDst = mSrc.Clone();

            switch (pattern)
            {
                case 0:
                    using (var gray = new Mat())
                    {
                        Cv2.CvtColor(mSrc, gray, ColorConversionCodes.BGR2GRAY);

                        const int maxCorners = 50, blockSize = 3;
                        const double qualityLevel = 0.01, minDistance = 20.0, k = 0.04;
                        const bool useHarrisDetector = false;
                        Point2f[] corners = Cv2.GoodFeaturesToTrack(gray, maxCorners, qualityLevel,
                                            minDistance, new Mat(), blockSize, useHarrisDetector, k);
                        foreach (Point2f it in corners)
                        {
                            Cv2.Circle(mDst, (OpenCvSharp.Point)it, 4, Scalar.Blue, 2);
                        }
                    }
                    break;

                case 1:
                    using (var gray = new Mat())
                    {
                        Cv2.CvtColor(mSrc, gray, ColorConversionCodes.RGB2GRAY);
                        Cv2.Threshold(gray, gray, 128, 255, ThresholdTypes.Binary);

                        Cv2.FindContours(gray, out Point[][] contours,      //輪郭検出
                            out HierarchyIndex[] hierarchy, RetrievalModes.Tree,
                                            ContourApproximationModes.ApproxSimple);

                        for (int i = 0; i < contours.Length; i++)
                        {
                            Cv2.DrawContours(mDst, contours, i, Scalar.Green,
                                                    2, LineTypes.Link8, hierarchy, 0);
                        }
                    }
                    break;
            }
            Cv2.ImShow(fn, mDst);
        }
    }
}
