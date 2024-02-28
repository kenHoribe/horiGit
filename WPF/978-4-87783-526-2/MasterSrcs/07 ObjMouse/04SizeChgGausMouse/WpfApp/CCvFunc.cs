using System;
using System.Collections.Generic;
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
        public void DoCvFunction(string fn, List<Rectangle> ListRect, int toSmall)
        {
            List<Rect> cvRect = Rectangle2OpenCvRect(ListRect);

            if (mSrc is not null)
            {
                mDst = mSrc.Clone();
                DoChgObjsGausian(mDst, cvRect, toSmall);
                Cv2.ImShow(fn, mDst);
            }
        }

    }
}
