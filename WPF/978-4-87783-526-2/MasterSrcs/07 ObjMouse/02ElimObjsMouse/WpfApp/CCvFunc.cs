using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

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
        public void DoCvFunction(string fn, List<Rectangle> ListRect)
        {
            using (Mat mask = new(mSrc!.Size(), MatType.CV_8UC1, new Scalar(0)))
            {
                foreach (Rectangle r in ListRect)
                {
                    int x = (int)Canvas.GetLeft(r);
                    int y = (int)Canvas.GetTop(r);
                    Point p0 = new(x, y);
                    Point p1 = new(x + r.Width, y + r.Height);
                    Cv2.Rectangle(mask, p0, p1, new Scalar(255), -1, LineTypes.AntiAlias);
                }
                mDst = new Mat();
                Cv2.Inpaint(mSrc, mask, mDst, 1, InpaintMethod.Telea);
            }
            Cv2.ImShow(fn, mDst);
        }

    }
}
