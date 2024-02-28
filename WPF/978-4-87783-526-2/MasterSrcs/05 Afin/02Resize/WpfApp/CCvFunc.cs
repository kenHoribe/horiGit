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
        public void DoCvFunction(string fn, float scale)
        {
            if (mSrc == null)
                return;

            mDst = new Mat();
            Cv2.Resize(mSrc, mDst, new OpenCvSharp.Size(), scale, scale);
            Cv2.ImShow(fn, mDst);
        }
    }
}
