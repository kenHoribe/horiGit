using System;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        public float Scale { get; set; } = 1.0f;   // scaling

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public BitmapSource DoCvFunction(List<String> srcNames)
        {
            var mats = new List<Mat>();
            foreach (var itr in srcNames)
            {
                Mat img = new Mat();
                img = Cv2.ImRead(itr);
                mats.Add(img);
            }
            var stitcher = Stitcher.Create(Stitcher.Mode.Panorama);
            mDst = new Mat();
            _ = stitcher.Stitch(mats, mDst);

            Mat dispDst = new();
            Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
            return BitmapSourceConverter.ToBitmapSource(dispDst);
        }

    }
}
