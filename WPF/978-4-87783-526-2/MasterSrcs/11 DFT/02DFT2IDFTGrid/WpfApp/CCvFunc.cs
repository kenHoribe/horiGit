using System.Windows.Media.Imaging;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace CCvLibrary
{
    public class CCvFunc : CCv
    {
        public float Scale { get; set; } = 1.0f;   // scaling
        private Mat? mIDFT = null, mDFT = null;

        //----------------------------------------------------------------
        //コンストラクタ
        public CCvFunc() : base()
        {
        }

        //----------------------------------------------------------------
        // OpenCVを使用して処理
        public (BitmapSource, BitmapSource) DoCvFunction()
        {
            Mat dft = mat2Dft(mSrc);            // image to DFT
            Mat dft8u = dft2dispMat(dft);       // DFT to display image
            Mat dispDft = swapDft(dft8u);       // swap: 1 <-> 4, 2 <-> 3
            Mat dispIdft = dft2idft8u(dft, mSrc);

            mDFT = dispDft.Clone();
            Cv2.Resize(mDFT, dispDft, new OpenCvSharp.Size(), Scale, Scale);
            mIDFT = dispIdft.Clone();
            Cv2.Resize(mIDFT, dispIdft, new OpenCvSharp.Size(), Scale, Scale);
            return (BitmapSourceConverter.ToBitmapSource(dispDft),
                BitmapSourceConverter.ToBitmapSource(dispIdft));
        }

        //----------------------------------------------------------------
        // SaveAsを作り、指定ファイル名にDFT,IDTを付けて2つを格納する
        // 同じ名前があると置き換えますかと警告が出るが、上書きされることはない。
        //----------------------------------------------------------------
        public void SaveAsDFTIDFT()
        {
            string? fname = GetWriteFile();
            if (fname != null)
            {
                string? path = System.IO.Path.GetDirectoryName(fname);

                string dftFileName = "DFT_" + System.IO.Path.GetFileName(fname);
                string dftName = System.IO.Path.Combine(path, dftFileName);

                string idftFileName = "IDFT_" + System.IO.Path.GetFileName(fname);
                string idftName = System.IO.Path.Combine(path, idftFileName);

                Cv2.ImWrite(dftName, mDFT);
                Cv2.ImWrite(idftName, mIDFT);
            }
        }

        //----------------------------------------------------------------
        // Mat -> DFT
        private Mat mat2Dft(Mat src)
        {
            using (var gray = new Mat())
            {
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                Mat srcReal = new Mat();

                int dftRows = Cv2.GetOptimalDFTSize(gray.Rows);
                int dftCols = Cv2.GetOptimalDFTSize(gray.Cols);

                // srcRealの左隅に入力の値が格納されている、サイズを
                // DFTへ最適化し拡張された部分は0で埋める。
                Cv2.CopyMakeBorder(gray, srcReal, 0, dftRows - gray.Rows,
                    0, dftCols - gray.Cols, BorderTypes.Constant, Scalar.All(0));

                // real と image(0)とマージして複素画像(行列)へ
                Mat f32 = new Mat();
                srcReal.ConvertTo(f32, MatType.CV_32F);
                Mat[] planes = { f32, Mat.Zeros(srcReal.Size(), MatType.CV_32F) };
                Mat complex = new Mat();
                Cv2.Merge(planes, complex);  // complexはrealとimaginaryを持ったMat

                // DFTの実行、結果は複素数、log(1 + sqrt(Re(DFT(I))^2 + Im(DFT(I))^2))
                Mat dft = new Mat();
                Cv2.Dft(complex, dft);          // dftはDFTの結果
                return dft;
            }
        }

        //---------------------------------------------------------------------
        // DFT convert to display image, フーリエ変換結果の可視化
        private Mat dft2dispMat(Mat complex)
        {
            Mat[] planes = new Mat[2];

            Cv2.Split(complex, out planes);     // planes[0] = real, [1] = imaginary

            // dst(x, y) = sqrt(pow(src1(x, y), 2) + pow(src2(x, y), 2))
            Mat magnitude = new Mat();
            Cv2.Magnitude(planes[0], planes[1], magnitude); // planes[0] = DFT magnitude

            magnitude += Scalar.All(1);         // 表示用に各ピクセル値に1.0を加算
            Cv2.Log(magnitude, magnitude);      // 対数へ

            Mat srcDFT = new Mat();
            Cv2.Normalize(magnitude, magnitude, 0, 1, NormTypes.MinMax);
            magnitude.ConvertTo(srcDFT, MatType.CV_8U, 255, 0); // 表示用DFT

            return srcDFT;
        }

        //---------------------------------------------------------------------
        // 事象入替、 Size of "src" must odd.
        //
        // swap: 1 <-> 4, 2 <-> 3
        //
        //              |                           |
        //         1    |    2                 4    |    3
        //              |                           |
        //   -----------+------------     ----------+------------
        //              |                           |
        //         3    |    4                 2    |    1
        //              |                           |
        //
        private Mat swapDft(Mat src)
        {
            Mat swap = src.Clone();

            int cx = swap.Cols / 2;     // center
            int cy = swap.Rows / 2;     // center

            Mat j1 = new Mat(swap, new Rect(0, 0, cx, cy));       // Top-Left
            Mat j2 = new Mat(swap, new Rect(cx, 0, cx, cy));      // Top-Right
            Mat j3 = new Mat(swap, new Rect(0, cy, cx, cy));      // Bottom-Left
            Mat j4 = new Mat(swap, new Rect(cx, cy, cx, cy));     // Bottom-Right

            Mat jPart = new Mat();
            j1.CopyTo(jPart);       // swap 1 <-> 4
            j4.CopyTo(j1);
            jPart.CopyTo(j4);
            j2.CopyTo(jPart);       // swap 2 <-> 3
            j3.CopyTo(j2);
            jPart.CopyTo(j3);

            return swap;
        }

        //---------------------------------------------------------------------
        // DFT to IDFT
        private Mat dft2idft8u(Mat dft, Mat src)
        {
            Mat temp = new Mat();
            Cv2.Idft(dft, temp);            // IDFT

            // 複素画像の実部と虚部を2枚の画像に分離する。
            Mat[] readImage = new Mat[2];
            Cv2.Split(temp, out readImage);         // [0]-> Real, [1]->imaginary

            // 実部について正規化を行う。入力画像のサイズはDFT用に
            // 拡大されているので、原画像の同サイズにROIを設定して縮小する。
            Mat idftRoi = new Mat(readImage[0], new Rect(0, 0, src.Cols, src.Rows));
            Mat idft = new Mat();
            Cv2.Normalize(idftRoi, idft, 0, 1, NormTypes.MinMax);
            idft.ConvertTo(idft, MatType.CV_8UC1, 255.0, 0);

            return idft;
        }

    }
}
