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
        public void DoCvFunction(string fn, string nDetector)
        {
            using (Mat gray = new())
            using (Mat equalize = new())
            using (var objDetector = new CascadeClassifier(nDetector)) // create detector
            {
                Cv2.CvtColor(mSrc!, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(gray, equalize);

                Rect[] objs = objDetector.DetectMultiScale(equalize, 1.2, 2,
                                HaarDetectionTypes.ScaleImage, new Size(30, 30));

                mDst = mSrc!.Clone();
                foreach (var it in objs)
                {
                    Cv2.Rectangle(mDst, new OpenCvSharp.Point(it.X, it.Y),
                                new OpenCvSharp.Point(it.X + it.Width, it.Y + it.Height),
                                            Scalar.Red, 2, LineTypes.AntiAlias);
                }
            }
            Cv2.ImShow(fn, mDst);
        }

        // get xml file name
        public string GetXmlFileName(string filter)
        {
            string? fileName = GetReadFile(filter);
            return fileName!;
        }
    }
}
