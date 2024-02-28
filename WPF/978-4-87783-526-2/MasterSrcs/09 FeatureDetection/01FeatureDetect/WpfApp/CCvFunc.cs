using OpenCvSharp;

#pragma warning disable CS8604 // Null 参照引数の可能性があります。

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

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public void DoCvFunction(string fn, string Algorithm)
        {
            using (var gray = new Mat())
            using (var descriptors = new Mat())
            {
                Cv2.CvtColor(mSrc, gray, ColorConversionCodes.BGR2GRAY);
                KeyPoint[]? keyPoints = null;
                switch (Algorithm)
                {
                    case "AKAZE":
                        {
                            var extractor = AKAZE.Create();
                            extractor.DetectAndCompute(gray, null, out keyPoints, descriptors);
                        }
                        break;
                    case "KAZE":
                        {
                            var extractor = KAZE.Create();
                            extractor.DetectAndCompute(gray, null, out keyPoints, descriptors);
                        }
                        break;
                    case "ORB":
                        {
                            var extractor = ORB.Create();
                            extractor.DetectAndCompute(gray, null, out keyPoints, descriptors);
                        }
                        break;
                    case "BRISK":
                        {
                            var extractor = BRISK.Create();
                            extractor.DetectAndCompute(gray, null, out keyPoints, descriptors);
                        }
                        break;
                }
                mDst = new Mat();
                Cv2.DrawKeypoints(mSrc, keyPoints, mDst);
                Mat dispDst = new();
                Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
                Cv2.ImShow(fn, dispDst);
            }
        }

    }
}
