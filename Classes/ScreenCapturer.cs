using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItsNotMe.Classes
{
    public class ScreenCapturer
    {
        public Bitmap CapturerArea(int x, int y, int x2, int y2)
        {
            int Width = x2 - x;
            int Height = y2 - y;

            Bitmap bmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bmp))
                g.CopyFromScreen(x, y, 0, 0, bmp.Size);

            return bmp;
        }
    }
}
