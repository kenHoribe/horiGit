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
            using (mDst = new())
            {
                if (_Tgt.Width != mSrc!.Width || _Tgt.Height != mSrc!.Height)
                    return -1d;

                Cv2.Absdiff(mSrc, _Tgt, mDst);
                double result = Cv2.Sum(mDst)[0];
                return result;
            }
        }

    }
}
