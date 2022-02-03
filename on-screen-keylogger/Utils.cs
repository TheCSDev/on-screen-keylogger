using System;
using System.Runtime.InteropServices;

namespace on_screen_keylogger
{
    public static class Utils
    {
        //========================================================
        public sealed class MouseEventF
        {
            public const uint
                MOUSEEVENTF_ABSOLUTE = 0x8000,
                MOUSEEVENTF_LEFTDOWN = 0x0002,
                MOUSEEVENTF_LEFTUP = 0x0004,
                MOUSEEVENTF_MIDDLEDOWN = 0x0020,
                MOUSEEVENTF_MIDDLEUP = 0x0040,
                MOUSEEVENTF_MOVE = 0x0001,
                MOUSEEVENTF_RIGHTDOWN = 0x0008,
                MOUSEEVENTF_RIGHTUP = 0x0010,
                MOUSEEVENTF_WHEEL = 0x0800,
                MOUSEEVENTF_XDOWN = 0x0080,
                MOUSEEVENTF_XUP = 0x0100;
        }
        //========================================================
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        //--------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();
        //--------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);
        //--------------------------------------------------------
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        //========================================================
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        //--------------------------------------------------------
        public static string SubstrStartEnd(this string arg, int start, int end) =>
            arg.Substring(start, arg.Length - (end + 1));
        //========================================================
    }
}
