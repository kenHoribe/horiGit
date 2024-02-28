using System;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace CCvLibrary
{
    public class CCv
    {
        protected Mat? mSrc, mDst;

        //----------------------------------------------------------------
        //コンストラクタ
        public CCv()
        {
        }

        //----------------------------------------------------------------
        // 読み込みファイル名を取得、
        //                ダイアログを使用して読み込みファイルを選択させる
        public string? GetReadFile(
            string filter = "画像ファイル(*.jpg,*.bmp,*.png)|*.jpg;*.bmp;*.png|"
                                            + "すべてのファイル(*.*)|*.*")
        {
            string? fname = null;

            var dialog = new OpenFileDialog
            {
                Filter = filter,
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
                fname = dialog.FileName;

            return fname;
        }

        //----------------------------------------------------------------
        // 書き込みファイル名を取得、
        //               ダイアログを使用して読み込みファイルを選択させる
        public string? GetWriteFile(
            string filter = "画像ファイル(*.jpg,*.bmp,*.png)|*.jpg;*.bmp;*.png|"
                                            + "すべてのファイル(*.*)|*.*")
        {
            string? fname = null;

            var dialog = new SaveFileDialog
            {
                Filter = filter,
                FilterIndex = 1
            };

            if (dialog.ShowDialog() == true)
                fname = dialog.FileName;

            return fname;
        }

        //----------------------------------------------------------------
        // ファイルを開く、ファイル名が指定されていない場合は、
        //               ダイアログを使用して読み込みファイルを選択させる
        public virtual (string?, BitmapSource?) OpenFileCv(string? fname)
        {
            BitmapSource? bmpSrc = null;
            string? newfname = fname;

            if (fname == null)
            {
                newfname = GetReadFile();
            }

            if (newfname != null)
            {
                Mat img = Cv2.ImRead(newfname);
                if (!img.Empty())
                {
                    mSrc = img;
                    bmpSrc = BitmapSourceConverter.ToBitmapSource(mSrc);
                }
            }
            return (newfname, bmpSrc);
        }


        //----------------------------------------------------------------
        // 名前を付けてファイルを保存
        public void SaveAS()
        {
            string? fname = GetWriteFile();
            if (mDst != null && fname != null)
            {
                Cv2.ImWrite(fname, mDst);    // OpenCV
            }
        }

        //----------------------------------------------------------------
        // 読み込んだ画像を返す
        public BitmapSource? GetOrgBitmapSource()
        {
            if (mSrc == null)
                return null;

            return BitmapSourceConverter.ToBitmapSource(mSrc);
        }

        //----------------------------------------------------------------
        // converts the [List<System.Windows.Shapes.Rectangle>] to [List<OpenCvSharp.Rect>].
        //    assume that the input ListRect is already normalized.
        public List<OpenCvSharp.Rect> Rectangle2OpenCvRect(List<Rectangle> ListRect)
        {
            List<Rect> cvRect = new();
            foreach (var r in ListRect)
            {
                int x = (int)Canvas.GetLeft(r);
                int y = (int)Canvas.GetTop(r);
                int width = (int)r.Width;
                int height = (int)r.Height;
                cvRect.Add(new OpenCvSharp.Rect(x, y, width, height));
            }
            return cvRect;
        }

        //----------------------------------------------------------------
        // create cos k mat
        public Mat CreateCosMat(int rows, int cols)
        {
            Mat mat = new(rows, cols, MatType.CV_8UC3, new Scalar(0));
            OpenCvSharp.Point center = new(cols / 2, rows / 2);
            double radius = Math.Sqrt(Math.Pow(center.X, 2) + Math.Pow(center.Y, 2));
            for (int y = 0; y < mat.Rows; y++)
            {
                for (int x = 0; x < mat.Cols; x++)
                {
                    // distance from center
                    double distance = Math.Sqrt(Math.Pow(center.X - x, 2)
                                                        + Math.Pow(center.Y - y, 2));
                    // radius=π, current radian
                    double radian = (distance / radius) * (double)Math.PI;
                    // cosθ, normalize -1.0～1.0 to 0～1.0
                    double Y = (Math.Cos(radian) + 1.0) / 2.0;
                    // normalize (Y) 0～1.0 to 0.0～255.0
                    mat.At<Vec3b>(y, x)[0] =
                        mat.At<Vec3b>(y, x)[1] =
                            mat.At<Vec3b>(y, x)[2] = (byte)(Y * 255.0f);
                }
            }
            return mat;
        }

        //----------------------------------------------------------------
        // mulMask
        public Mat MulMat(Mat mat, Mat table)
        {
            Mat mat1 = new();
            Mat dst = mat1;
            using (Mat mat32f = new(), dst32f = new())
            {
                Mat table32f = new();

                mat.ConvertTo(mat32f, MatType.CV_32FC3);
                table.ConvertTo(table32f, MatType.CV_32FC3);
                table32f /= 255.0f;
                Cv2.Multiply(mat32f, table32f, dst32f);
                dst32f.ConvertTo(dst, MatType.CV_8UC3);
            }
            return dst;
        }


        //---------------------------------------------------------
        // Size change by Gausian
        //
        //  dst:      Mat
        //  ListRect: areas
        // toSmall:   0:to Big, 1:to small
        //
        protected Mat DoChgObjsGausian(Mat dst, List<Rect> ListRect, int toSmall)
        {
            foreach (Rect rr in ListRect)
            {
                if (rr.Width == 0 || rr.Height == 0)
                    continue;                       //skip if area is 0

                Rect rect = new(rr.X, rr.Y, rr.Width, rr.Height);
                Mat obj = new(dst, rect);           // set roi

                Mat mapX = new(obj.Size(), MatType.CV_32FC1); // map x cord. mat
                Mat mapY = new(obj.Size(), MatType.CV_32FC1); // map y cord. mat

                float cx = obj.Cols / 2.0f;             // center cord.
                float cy = obj.Rows / 2.0f;

                for (int y = 0; y < obj.Rows; y++)      // calc src cord.
                {
                    for (int x = 0; x < obj.Cols; x++)
                    {
                        float dx = x - cx;              // x cord. form center
                        float dy = y - cy;              // y cord. form center
                        double r = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));  // distabce

                        // ガウス関数、 u: 0, a: 1, sigma: obj.Cols / 8
                        float gauss = Gaussf((float)r, 1.0f, 0.0f, obj.Cols / 8.0f);

                        if (toSmall == 0)               // 変換座標の計算
                        {
                            mapX.At<float>(y, x) = cx + (dx / (gauss + 1.0f));
                            mapY.At<float>(y, x) = cy + (dy / (gauss + 1.0f));
                        }
                        else
                        {
                            mapX.At<float>(y, x) = cx + (dx * (gauss + 1.0f));
                            mapY.At<float>(y, x) = cy + (dy * (gauss + 1.0f));
                        }
                    }
                }
                Cv2.Remap(obj, obj, mapX, mapY, InterpolationFlags.Cubic, BorderTypes.Replicate);
            }
            return dst;
        }

        //---------------------------------------------------------
        // gaussf
        private float Gaussf(float x, float a, float mu, float sigma)
        {
            return a * (float)Math.Exp(-Math.Pow((x - mu), 2) / (2 * Math.Pow(sigma, 2)));
        }

        //----------------------------------------------------------------
        // Size change by Cos Table
        //
        //  src:      source Mat
        //  dst:      destination Mat
        //  ListRect: areas
        //  scale:    scale
        //
        protected Mat DoChgObjs(Mat src, Mat dst, List<Rect> ListRect, float scale)
        {
            List<Mat> srcobjs = new(), dstobjs = new();

            foreach (Rect r in ListRect)
            {
                if (r.Width == 0 || r.Height == 0)
                    continue;                       //skip if area is 0

                if (scale > 1.0f)
                {   // to Big

                    //入力切り出し
                    Rect srcrect = new(r.X, r.Y, r.Width, r.Height);
                    Mat srcroi = new(src, srcrect);
                    srcobjs.Add(srcroi);

                    //出力切り出し、少し大きくする
                    int deltaW = (int)(r.Width * (scale - 1.0f)) / 2;
                    int deltaH = (int)(r.Height * (scale - 1.0f)) / 2;

                    Rect dstrect = new(r.X - deltaW, r.Y - deltaH,
                                    r.Width + deltaW * 2, r.Height + deltaH * 2);
                    Rect cliprect = ClipIt(dst.Size(), dstrect);
                    Mat dstroi = new(dst, cliprect);
                    dstobjs.Add(dstroi);
                }
                else
                {   // to Small
                    //入力切り出し、少し大きくする
                    int deltaW = (int)(r.Width * (1.0f - scale)) / 2;
                    int deltaH = (int)(r.Height * (1.0f - scale)) / 2;

                    Rect srcrect = new(r.X - deltaW, r.Y - deltaH,
                            r.Width + deltaW * 2, r.Height + deltaH * 2);
                    Rect cliprect = ClipIt(dst.Size(), srcrect);
                    Mat srcroi = new(src, cliprect);
                    srcobjs.Add(srcroi);

                    //出力切り出し
                    Rect dstrect = new(r.X, r.Y, r.Width, r.Height);
                    Mat dstroi = new(dst, dstrect);
                    dstobjs.Add(dstroi);
                }
            }

            // 大きさを合わせる
            for (int i = 0; i < srcobjs.Count; i++)
            {
                Cv2.Resize(srcobjs[i], srcobjs[i],
                            new OpenCvSharp.Size(dstobjs[i].Cols, dstobjs[i].Rows));
            }

            // マージ、重みづけ加算
            for (int i = 0; i < srcobjs.Count; i++)
            {
                Mat weightMat = CreateCosMat(srcobjs[i].Rows, srcobjs[i].Cols);
                Mat iWeightMat = Scalar.All(255) - weightMat;
                Mat srcWeight = MulMat(srcobjs[i], weightMat);
                Mat dstWeight = MulMat(dstobjs[i], iWeightMat);
                Cv2.Add(dstWeight, srcWeight, dstobjs[i]);
            }
            return dst;
        }

        //---------------------------------------------------------
        // clip Rect
        private OpenCvSharp.Rect ClipIt(OpenCvSharp.Size size,
                                                        OpenCvSharp.Rect rect)
        {
            OpenCvSharp.Rect clip = rect;

            clip.Width = rect.X < 0 ? rect.Width + rect.X : clip.Width;
            clip.X = rect.X < 0 ? 0 : clip.X;

            clip.Height = rect.Y < 0 ? rect.Height + rect.Y : clip.Height;
            clip.Y = rect.Y < 0 ? 0 : clip.Y;

            clip.Width = (clip.X + rect.Width) >= size.Width ?
                                        size.Width - clip.X : clip.Width;

            clip.Height = clip.Y + rect.Height >= size.Height ?
                                        size.Height - clip.Y : clip.Height;

            return clip;
        }
    }
}
