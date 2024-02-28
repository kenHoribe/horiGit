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

            mDst = new Mat();
            using (Mat weightMat = CreateCosMat(MatSrc1.Rows, MatSrc1.Cols))
            using (Mat iWeightMat = new Scalar(255, 255, 255) - weightMat)
            using (Mat intSrc1 = MulMat(MatSrc1, weightMat))
            using (Mat intSrc2 = MulMat(MatSrc2, iWeightMat))
            {
                //Cv2.ImShow("intSrc1", intSrc1);
                //Cv2.ImShow("intSrc2", intSrc2);
                switch (mode)
                {
                    case 0:
                        Cv2.Add(intSrc1, intSrc2, mDst);
                        break;
                    case 1:
                        Cv2.Subtract(intSrc1, intSrc2, mDst);
                        break;
                    case 2:
                        Cv2.BitwiseAnd(intSrc1, intSrc2, mDst);
                        break;
                    case 3:
                        Cv2.BitwiseOr(intSrc1, intSrc2, mDst);
                        break;
                    case 4:
                        Cv2.BitwiseXor(intSrc1, intSrc2, mDst);
                        break;
                }
            }
            Cv2.ImShow("src1", MatSrc1);
            Cv2.ImShow("src2", MatSrc2);
            return BitmapSourceConverter.ToBitmapSource(mDst);
        }

    }
}
