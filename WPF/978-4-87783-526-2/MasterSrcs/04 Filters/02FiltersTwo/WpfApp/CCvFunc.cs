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
        public void DoCvFunction(string function)
        {
            if (mSrc == null)
                return;

            mDst = new Mat();

            switch (function)
            {
                case "MenuBlur":
                    Cv2.Blur(mSrc, mDst, new OpenCvSharp.Size(5, 5));                // Blur
                    break;
                case "MenuGaussianBlur":
                    Cv2.GaussianBlur(mSrc, mDst, new OpenCvSharp.Size(5, 5), 10.0);  // GaussianBlur
                    break;
                case "MenuLaplacian":
                    Cv2.CvtColor(mSrc, mDst, ColorConversionCodes.BGR2GRAY);         // Laplacian
                    Cv2.Laplacian(mDst, mDst, 0);
                    break;
                case "MenuSobel":
                    Cv2.CvtColor(mSrc, mDst, ColorConversionCodes.BGR2GRAY);         // Sobel
                    Cv2.Sobel(mDst, mDst, -1, 0, 1);
                    break;
                case "MenuCanny":
                    Cv2.CvtColor(mSrc, mDst, ColorConversionCodes.BGR2GRAY);         // Canny
                    Cv2.Canny(mDst, mDst, 40.0, 150.0);
                    break;
                case "MenuDilate":
                    Cv2.Dilate(mSrc, mDst, new Mat());                               // Dilate
                    break;
                case "MenuErode":
                    Cv2.Erode(mSrc, mDst, new Mat());                                // Erode
                    break;
                case "MenuGamma":
                    double gamma = 2.0;                                             // Gamma
                    sbyte[] lut = new sbyte[256];
                    for (int i = 0; i <= 255; i++)
                    {
                        lut[i] = (sbyte)(System.Math.Pow((double)i / 255.0, 1 / gamma) * 255.0);
                    }
                    Mat lutMat = new Mat(1, 256, MatType.CV_8UC1, lut);
                    Cv2.LUT(mSrc, lutMat, mDst);
                    break;
                default:
                    mDst = mSrc.Clone();
                    break;
            }
            Cv2.ImShow("結果", mDst);
        }
    }
}
