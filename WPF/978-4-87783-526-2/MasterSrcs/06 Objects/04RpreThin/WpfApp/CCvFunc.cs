using OpenCvSharp;
using OpenCvSharp.XImgProc;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        public float Scale { get; set; }    // scaling

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        public void DoCvFunction(string fn, int pattern)
        {
            if (mSrc == null)
                return;

            mDst = new Mat();

            switch (pattern)
            {
                case 0:
                    using (Mat gray = new())
                    using (Mat mask = new())
                    {
                        Cv2.CvtColor(mSrc, gray, ColorConversionCodes.BGR2GRAY);
                        Cv2.EqualizeHist(gray, mask);
                        Cv2.Threshold(mask, mask, 253, 1, ThresholdTypes.Binary);
                        Cv2.Inpaint(mSrc, mask, mDst, 3, InpaintMethod.Telea);
                    }
                    break;

                case 1:
                    using (var gray = new Mat())
                    {
                        Cv2.CvtColor(mSrc, gray, ColorConversionCodes.BGR2GRAY);
                        CvXImgProc.Thinning(gray, mDst, ThinningTypes.ZHANGSUEN);
                        break;
                    }
            }
            Mat dispDst = new();
            Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
            Cv2.ImShow(fn, dispDst);
        }
    }
}
