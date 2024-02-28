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

        public void DoCvFunction(string fn, int width, int height, int dispHeight)
        {
            if (mSrc == null)
                return;

            int detects = 0;

            mDst = mSrc.Clone();

            using (Mat gray = new())
            {
                Cv2.CvtColor(mSrc, gray, ColorConversionCodes.RGB2GRAY);
                Cv2.Threshold(gray, gray, 128, 255, ThresholdTypes.Binary);

                Cv2.FindContours(gray, out Point[][] contours, out HierarchyIndex[] hierarchy,
                            RetrievalModes.Tree, ContourApproximationModes.ApproxTC89L1);

                mDst = mSrc.Clone();
                for (int i = 0; i < contours.Length; i++)
                {
                    Cv2.DrawContours(mDst, contours, i, Scalar.Green, 2);
                }

                for (int i = 0; i < contours.Length; i++)
                {
                    double a = Cv2.ContourArea(contours[i], false);
                    if (a > width * height)     // only an area of  width * height or more
                    {
                        Point[] approx;         // contour to a straight line
                        approx = Cv2.ApproxPolyDP(contours[i],
                                            0.01 * Cv2.ArcLength(contours[i], true), true);
                        if (approx.Length == 4) // rectangle only
                        {
                            detects++;
                            Point[][] tmpContours = new Point[][] { approx };

                            int maxLevel = 0;
                            Cv2.DrawContours(mDst, tmpContours, 0,
                                    Scalar.Red, 2, LineTypes.AntiAlias, hierarchy, maxLevel);
                        }
                    }
                }
            }
            float scale = (float)dispHeight / (float)mDst.Height;
            Mat dipDst = new();
            Cv2.Resize(mDst, dipDst, new OpenCvSharp.Size(), scale, scale);
            Cv2.ImShow(fn, dipDst);
        }
    }
}
