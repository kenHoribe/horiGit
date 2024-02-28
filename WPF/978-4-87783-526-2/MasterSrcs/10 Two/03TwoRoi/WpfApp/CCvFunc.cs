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

            Rect roi = new(MatSrc1.Cols / 8, MatSrc1.Rows / 8, 
                                    MatSrc1.Cols / 2, MatSrc1.Rows / 2);
            mDst = MatSrc1.Clone();
            using (Mat src1Roi = new Mat(MatSrc1, roi))
            using (Mat src2Roi = new Mat(MatSrc2, roi))
            using (Mat dstRoi = new Mat(mDst, roi))
            {
                switch (mode)
                {
                    case 0:
                        Cv2.Add(src1Roi, src2Roi, dstRoi);
                        break;
                    case 1:
                        Cv2.Subtract(src1Roi, src2Roi, dstRoi);
                        break;
                    case 2:
                        Cv2.BitwiseAnd(src1Roi, src2Roi, dstRoi);
                        break;
                    case 3:
                        Cv2.BitwiseOr(src1Roi, src2Roi, dstRoi);
                        break;
                    case 4:
                        Cv2.BitwiseXor(src1Roi, src2Roi, dstRoi);
                        break;
                }
            }
            Cv2.ImShow("src1", MatSrc1);
            Cv2.ImShow("src2", MatSrc2);
            return BitmapSourceConverter.ToBitmapSource(mDst);
        }

    }
}
