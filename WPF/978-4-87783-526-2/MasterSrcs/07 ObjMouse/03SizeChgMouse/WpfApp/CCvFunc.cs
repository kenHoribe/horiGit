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
        public void DoCvFunction(string fn, List<Rectangle> ListRect, string scaleText)
        {
            List<Rect> cvRect = Rectangle2OpenCvRect(ListRect);
            float scale = Convert.ToSingle(scaleText);

            if(mSrc is not null)
            {
                mDst = mSrc.Clone();
                DoChgObjs(mSrc, mDst, cvRect, scale);
                Cv2.ImShow(fn, mDst);
            }
        }

    }
}
