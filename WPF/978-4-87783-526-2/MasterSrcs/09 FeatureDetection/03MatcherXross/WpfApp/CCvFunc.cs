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
            Mat[] srcs = new Mat[2];
            for (int i = 0; i < 2; i++)
            {
                srcs[i] = Cv2.ImRead(srcNames[i]);
            }

            //using (var detector = ORB.Create())
            using (var detector = AKAZE.Create())
            using (var descriptors1 = new Mat())
            using (var descriptors2 = new Mat())
            {
                //特徴量の検出と特徴量ベクトルの計算
                detector.DetectAndCompute(srcs[0], null, out KeyPoint[] keypoints1, descriptors1);
                detector.DetectAndCompute(srcs[1], null, out KeyPoint[] keypoints2, descriptors2);

                //マッチング方法
                DescriptorMatcher matcher = DescriptorMatcher.Create("BruteForce");

                //特徴量ベクトル同士のマッチング結果を配列へ格納
                DMatch[] fwdMatches = matcher.Match(descriptors1, descriptors2);
                DMatch[] bckMatches = matcher.Match(descriptors2, descriptors1);
                List<DMatch> lMatches = new List<DMatch>();
                for (int i = 0; i < fwdMatches.Length; i++)
                {
                    DMatch forward = fwdMatches[i];
                    DMatch bckward = bckMatches[forward.TrainIdx];
                    if (bckward.TrainIdx == forward.QueryIdx)
                        lMatches.Add(forward);
                }

                // draw matches
                DMatch[] matches = lMatches.ToArray();
                mDst = new Mat();
                Cv2.DrawMatches(srcs[0], keypoints1, srcs[1], keypoints2, matches, mDst);
            }
            Mat dispDst = new();
            Cv2.Resize(mDst, dispDst, new OpenCvSharp.Size(), Scale, Scale);
            return BitmapSourceConverter.ToBitmapSource(dispDst);
        }

    }
}
