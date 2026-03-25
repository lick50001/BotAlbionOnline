using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ItsNotMe.Classes
{
    public class HotKeyManager
    {
        // Импорт функций Windows
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;
        private IntPtr _windowHandle;
        private HwndSource _source;

        // Это "событие", которое мы вызовем в MainWindow
        public Action OnHotKeyPressed;

        public void Setup(System.Windows.Window window)
        {
            _windowHandle = new WindowInteropHelper(window).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            // Регистрация F1 (0x70) без модификаторов (Alt/Ctrl)
            RegisterHotKey(_windowHandle, HOTKEY_ID, 0, 0x70);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                // Вызываем наше действие
                OnHotKeyPressed?.Invoke();
                handled = true;
            }
            return IntPtr.Zero;
        }

        public void Cleanup()
        {
            _source?.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
        }
    }
}