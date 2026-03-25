using ItsNotMe.Classes;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItsNotMe
{
    public partial class MainWindow : System.Windows.Window
    {
        string pathFile;
        ScreenCapturer screenCapturer = new ScreenCapturer();
        OcrEngine ocr = new OcrEngine();
        HotKeyManager hotKey = new HotKeyManager();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                hotKey.Setup(this);
                hotKey.OnHotKeyPressed = Tesseract;
            };
        }

        public void Tesseract()
        {
            using (Bitmap SS = screenCapturer.CapturerArea(520,  216, 700, 400))
            {
                pathFile = @"C:\Users\Anonim\Desktop\ItsNotMe\TestImg\Test.png";
                SS.Save(pathFile, ImageFormat.Png);

                string recognizedText = ocr.Recognize(SS);

                MessageBox.Show(recognizedText);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            hotKey.Cleanup(); // Обязательно выключаем хук при закрытии
            base.OnClosed(e);
        }
    }
}
