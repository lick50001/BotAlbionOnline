using ItsNotMe.Classes;
using ItsNotMe.Pages;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        string pathFile;
        ScreenCapturer screenCapturer = new ScreenCapturer();
        OcrEngine ocr = new OcrEngine();
        HotKeyManager hotKey = new HotKeyManager();
        OverlayWindow overlay = new OverlayWindow();
        private string _lastRecognized = "";

        private System.Windows.Threading.DispatcherTimer _colorCheckTimer;

        public MainWindow()
        {
            InitializeComponent();

            _colorCheckTimer = new System.Windows.Threading.DispatcherTimer();
            _colorCheckTimer.Interval = TimeSpan.FromMilliseconds(200);
            _colorCheckTimer.Tick += ColorTimer_Tick;
            _colorCheckTimer.Start();

            this.Loaded += (s, e) =>
            {
                //hotKey.Setup(this);
                //hotKey.OnHotKeyPressed = Tesseract;

                overlay.Show();
                overlay.Left = 430;
                overlay.Top = 560;
            };
        }

        private void ColorTimer_Tick(object sender, EventArgs e)
        {
            System.Drawing.Color color = GetColorAt(1474, 323);

            bool isTargetColor = Math.Abs(color.R - 214) < 15 &&
                         Math.Abs(color.G - 168) < 15 &&
                         Math.Abs(color.B - 128) < 15;

            if (isTargetColor)
            {
                if (string.IsNullOrEmpty(_lastRecognized))
                {
                    using (Bitmap SS = screenCapturer.CapturerArea(1260, 359, 1332, 382))
                    {
                        using (Bitmap SS2 = screenCapturer.CapturerArea(1260, 359, 1332, 382))
                        {
                            int SellOrder = Convert.ToInt32(ocr.Recognize(SS));

                            int BuyOrder = Convert.ToInt32(ocr.Recognize(SS2));



                            overlay.UpdateValue(_lastRecognized);
                        }
                    }

                        
                }
            }
            else
            {
                overlay.UpdateValue("");
            }
        }

        //public void Tesseract()
        //{
        //    using (Bitmap SS = screenCapturer.CapturerArea(1261, 363, 1326, 380))
        //    {
        //        string recognizedText = ocr.Recognize(SS);

        //        overlay.UpdateValue(recognizedText);
        //    }
        //}

        public System.Drawing.Color GetColorAt(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            byte r = (byte)(pixel & 0x000000FF);
            byte g = (byte)((pixel & 0x0000FF00) >> 8);
            byte b = (byte)((pixel & 0x00FF0000) >> 16);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        protected override void OnClosed(EventArgs e)
        {
            hotKey.Cleanup(); // Обязательно выключаем хук при закрытии
            base.OnClosed(e);
        }
    }
}
