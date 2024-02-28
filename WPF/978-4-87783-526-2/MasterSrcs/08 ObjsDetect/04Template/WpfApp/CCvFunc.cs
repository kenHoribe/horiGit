using System.Windows.Media.Imaging;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

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
        public BitmapSource DoCvFunction(string fn, string template)
        {
            using (Mat templImg = Cv2.ImRead(template))
            {
                // Template maching
                Mat result = new Mat();
                Cv2.MatchTemplate(mSrc!, templImg, result, TemplateMatchModes.CCoeffNormed);

                //Cv2.ImWrite("reslt.png", result * 255);   // save map

                // result
                Mat mDst = mSrc!.Clone();
                result.MinMaxLoc(out _, out double maxVal, out _, out Point maxLoc);
                if (maxVal > .8)
                {
                    mDst.Rectangle(maxLoc,
                        new Point(maxLoc.X + templImg.Cols, maxLoc.Y + templImg.Rows), 
                                                                            Scalar.Red);
                }
                return BitmapSourceConverter.ToBitmapSource(mDst);
            }
        }

        // Templateの表示
        public void ShowTemplate(string fn, string fname)
        {
            Mat TMat = Cv2.ImRead(fname);
            Cv2.ImShow(fn, TMat);
        }

        // Templateを破棄
        public void DesoryTemplate()
        {
            Cv2.DestroyAllWindows();
        }

    }
}
