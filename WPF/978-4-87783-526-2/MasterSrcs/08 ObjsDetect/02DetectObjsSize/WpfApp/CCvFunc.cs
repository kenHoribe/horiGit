using OpenCvSharp;
using System.Collections.Generic;

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
        public void DoCvFunction(string fn, string nDetector, int toSmall)
        {
            using (Mat gray = new())
            using (Mat equalize = new())
            using (var objDetector = new CascadeClassifier(nDetector)) // create detector
            {
                Cv2.CvtColor(mSrc!, gray, ColorConversionCodes.BGR2GRAY);
                Cv2.EqualizeHist(gray, equalize);

                Rect[] objs = objDetector.DetectMultiScale(equalize, 1.2, 2,
                                HaarDetectionTypes.ScaleImage, new Size(30, 30));

                float scale = toSmall == 0 ? 1.3f : .8f;
                mDst = mSrc!.Clone();
                List<OpenCvSharp.Rect> listRect = Array2OpenCvRect(objs);
                DoChgObjs(mSrc, mDst, listRect, scale);

                // 検出位置を表示したければコメントアウトを外す
                //foreach (var it in objs)
                //{
                //    Cv2.Rectangle(mDst, new OpenCvSharp.Point(it.X, it.Y),
                //                new OpenCvSharp.Point(it.X + it.Width, it.Y + it.Height),
                //                            Scalar.Red, 1, LineTypes.AntiAlias);
                //}
            }
            Cv2.ImShow(fn, mDst);
        }

        // Rect[] to List<OpenCvSharp.Rect>
        private static List<OpenCvSharp.Rect> Array2OpenCvRect(Rect[] ArrObjs)
        {
            List<Rect> cvRect = new();
            foreach (var it in ArrObjs)
            {
                cvRect.Add(it);
            }
            return cvRect;
        }

        // get xml file name
        public string GetXmlFileName(string filter)
        {
            string? fileName = GetReadFile(filter);
            return fileName!;
        }
    }
}
