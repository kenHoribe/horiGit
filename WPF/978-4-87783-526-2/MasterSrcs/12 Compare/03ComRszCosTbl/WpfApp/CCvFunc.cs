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
        public double DoCvFunction(string fn)
        {
            using (Mat _Tgt = Cv2.ImRead(fn))
            {
                if (_Tgt.Width == 0 || _Tgt.Height == 0) //画像ファイルではない
                    return -1d;

                using (Mat _Rmat = _Tgt.Resize(mSrc!.Size(), 0, 0, InterpolationFlags.Cubic))
                using (Mat weightMat = CreateCosMat(mSrc.Rows, mSrc.Cols))
                using (Mat intSrc1 = MulMat(mSrc, weightMat))
                using (Mat intSrc2 = MulMat(_Rmat, weightMat))
                using (Mat mDst = new())
                {
                    Cv2.Absdiff(intSrc1, intSrc2, mDst);

                    double result = Cv2.Sum(mDst)[0];
                    return result;
                }
            }
        }

    }
}
