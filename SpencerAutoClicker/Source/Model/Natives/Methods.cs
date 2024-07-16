using System;
using System.Runtime.InteropServices;

namespace SpencerAutoClicker.Source.Model.Natives
{
    class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] Input[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref Rect Rect);
    }
}
