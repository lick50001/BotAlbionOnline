using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ItsNotMe.Classes
{
    public class OcrEngine
    {
        private readonly TesseractEngine _engine;

        public OcrEngine() =>
            _engine = new TesseractEngine(@"./tessdata", "rus", EngineMode.Default);

        public string Recognize(Bitmap bitmap)
        {
            using (Mat src = bitmap.ToMat())
            using (Mat gray = new Mat())
            using (Mat resized = new Mat())
            using (Mat binary = new Mat())
            {
                // 1. В серый
                Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

                // 2. Увеличение
                Cv2.Resize(gray, resized, new OpenCvSharp.Size(gray.Width * 2, gray.Height * 2), 0, 0, InterpolationFlags.Linear);

                // 3. Бинаризация
                Cv2.Threshold(resized, binary, 150, 255, ThresholdTypes.BinaryInv);

                // 🔥 СОХРАНЯЕМ обработанное изображение для отладки
                string debugPath = @"C:\Users\Anonim\Desktop\ItsNotMe\TestImg\debug_";
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

                using (Bitmap debugBmp = binary.ToBitmap())
                {
                    string path = $@"C:\Users\Anonim\Desktop\ItsNotMe\TestImg\debug.png";
                    debugBmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                }

                // 4. Распознавание
                using (Bitmap bitmapForTesseract = binary.ToBitmap())
                using (var img = PixConverter.ToPix(bitmapForTesseract))
                {
                    using (var page = _engine.Process(img, PageSegMode.SingleLine))
                    {
                        return page.GetText()?.Trim();
                    }
                }
            }
        }
    }
}
