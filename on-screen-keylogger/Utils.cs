using System;
using System.Runtime.InteropServices;

namespace on_screen_keylogger
{
    public static class Utils
    {
        //========================================================
        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }
        //--------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        //--------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();
        //--------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);
        //========================================================
        //Also some extension methods cuz why not
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        //--------------------------------------------------------
        public static string TrimStartEnd(this string arg, int start, int end) =>
            arg.Substring(start, arg.Length - (end + 1));
        //========================================================
    }
}
