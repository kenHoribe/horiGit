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
        public void DoCvFunction(string fn, int function = 0)
        {
            if (mSrc == null)
                return;

            mDst = new Mat();

            switch (function)
            {
                case 0:
                    Cv2.Flip(mSrc, mDst, FlipMode.X);   // x軸反転（上下反転）
                    break;
                case 1:
                    Cv2.Flip(mSrc, mDst, FlipMode.Y);   // y軸反転（左右反転）
                    break;
                case 2:
                    Cv2.Flip(mSrc, mDst, FlipMode.XY);  // 両軸反転
                    break;
                default:
                    mDst = mSrc.Clone();
                    break;
            }
            Cv2.ImShow(fn, mDst);
        }
    }
}
