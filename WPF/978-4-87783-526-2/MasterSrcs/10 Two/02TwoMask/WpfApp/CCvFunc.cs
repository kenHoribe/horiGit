using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        private Mat? MatSrc1;
        private Mat? MatSrc2;

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public void ReadAndSizeChk(List<String> srcNames)
        {
            MatSrc1 = Cv2.ImRead(srcNames[0]);
            MatSrc2 = Cv2.ImRead(srcNames[1]);

            if (MatSrc1.Width != MatSrc2.Width || MatSrc1.Height != MatSrc2.Height)
            {
                throw new ArgumentException("Image size is different.");
            }
            if (MatSrc1.Width < 1 || MatSrc1.Height < 1)
            {
                throw new ArgumentException("Image size is too small.");
            }
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public BitmapSource DoCvFunction(List<String> srcNames, int mode)
        {
            if (MatSrc1 == null || MatSrc2 == null ||
                MatSrc1!.Empty() || MatSrc2!.Empty())
                return null!;

            mDst = MatSrc1.Clone();
            using (Mat mask = new Mat(MatSrc1.Size(), MatType.CV_8UC1, new Scalar(255)))
            {
                Point center = new(mask.Width / 2, mask.Height / 2);
                Cv2.Circle(mask, center, mask.Height / 4, new Scalar(0), -1);

                switch (mode)
                {
                    case 0:
                        Cv2.Add(MatSrc1, MatSrc2, mDst, mask);
                        break;
                    case 1:
                        Cv2.Subtract(MatSrc1, MatSrc2, mDst, mask);
                        break;
                    case 2:
                        Cv2.BitwiseAnd(MatSrc1, MatSrc2, mDst, mask);
                        break;
                    case 3:
                        Cv2.BitwiseOr(MatSrc1, MatSrc2, mDst, mask);
                        break;
                    case 4:
                        Cv2.BitwiseXor(MatSrc1, MatSrc2, mDst, mask);
                        break;
                }
            }
            Cv2.ImShow("src1", MatSrc1);
            Cv2.ImShow("src2", MatSrc2);
            return BitmapSourceConverter.ToBitmapSource(mDst);
        }

    }
}
