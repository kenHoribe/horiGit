using System;
using System.Windows.Media.Imaging;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

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

        public BitmapSource? DoCvFunction(int mode, int max, int blockSize)
        {
            if (mSrc == null ||  mSrc!.Empty())
                return null!;

            var type = ThresholdTypes.Binary;

            mDst = new Mat();
            switch (mode)
            {
                case 0:
                    type = ThresholdTypes.Binary;
                    break;
                case 1:
                    type = ThresholdTypes.BinaryInv;
                    break;
            }
            using ( var Grayscale = new Mat() )
            {
                Cv2.CvtColor(mSrc, Grayscale, ColorConversionCodes.BGR2GRAY);
                Cv2.AdaptiveThreshold(Grayscale, mDst, max,
                    AdaptiveThresholdTypes.GaussianC, type, blockSize, 20);
            }
            Mat _Dst = new();
            Cv2.Resize(mDst, _Dst, new OpenCvSharp.Size(), Scale, Scale);
            return BitmapSourceConverter.ToBitmapSource(_Dst);
        }
    }
}
