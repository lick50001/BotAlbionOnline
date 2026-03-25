using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ItsNotMe.Pages
{
    public partial class OverlayWindow : Window
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public OverlayWindow()
        {
            InitializeComponent();
            this.SourceInitialized += OverlayWindow_SourceInitialized;
        }

        private void OverlayWindow_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            // WS_EX_TRANSPARENT — клики проходят сквозь окно
            // WS_EX_TOOLWINDOW — скрывает окно из списка Alt+Tab
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);
        }

        public void UpdateValue(string text)
        {
            ValueText.Text = text;
        }
    }
}