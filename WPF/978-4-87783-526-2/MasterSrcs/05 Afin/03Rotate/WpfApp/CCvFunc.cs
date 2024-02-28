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
        // OpenCVを使用して処理&表示
        public void DoCvFunction(string fn, float angle)
        {
            if (mSrc == null)
                return;

            mDst = new Mat();
            Point2f center = new(mSrc.Cols / 2, mSrc.Rows / 2);
            Mat affineTrans = Cv2.GetRotationMatrix2D(center, angle, 1.0);
            Cv2.WarpAffine(mSrc, mDst, affineTrans, mSrc.Size(), InterpolationFlags.Cubic);
            Cv2.ImShow(fn, mDst);
        }
    }
}
