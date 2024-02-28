using System.Collections.Generic;

using OpenCvSharp;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        private readonly Dictionary<string, HistCompMethods> MethodsDict = new()
        {
                { "Correl",         HistCompMethods.Correl          },
                { "Chisqr",         HistCompMethods.Chisqr          },
                { "Intersect",      HistCompMethods.Intersect       },
                { "Bhattacharyya",  HistCompMethods.Bhattacharyya   },
                { "Hellinger",      HistCompMethods.Hellinger       }
          };

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public double DoCvFunction(string fn, string Methods)
        {
            using (Mat _mSrcGray = mSrc!.CvtColor(ColorConversionCodes.RGB2GRAY))
            using (Mat _TgtGray = Cv2.ImRead(fn, ImreadModes.Grayscale))
            {
                if (_TgtGray.Width == 0 || _TgtGray.Height == 0)
                    return -1d;

                using (Mat _RTgtGray = _TgtGray.Resize(mSrc!.Size(), 0, 0, InterpolationFlags.Cubic))
                using (Mat SrcHist = new Mat())
                using (Mat TgtHist = new Mat())
                {
                    Cv2.CalcHist(new Mat[] { _RTgtGray }, new int[] { 0 }, null, SrcHist, 1,
                                        new int[] { 256 }, new Rangef[] { new Rangef(0, 256) });
                    Cv2.CalcHist(new Mat[] { _mSrcGray }, new int[] { 0 }, null, TgtHist, 1,
                                        new int[] { 256 }, new Rangef[] { new Rangef(0, 256) });

                    double result = Cv2.CompareHist(SrcHist, TgtHist, MethodsDict[Methods]);
                    return result;
                }
            }
        }
    }
}
