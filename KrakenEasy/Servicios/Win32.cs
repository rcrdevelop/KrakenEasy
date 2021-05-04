using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace KrakenEasy.Servicios
{
    public struct RECT
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    public class Win32
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(
          IntPtr hWnd,
          out RECT lpPoint
        );
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(
          IntPtr hWnd
        );
        [DllImport("user32.dll")]
        public static extern IntPtr GetTopWindow(
          IntPtr hWnd
        );
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MoveWindow(IntPtr hWnd, long X, long Y, long nWidth, long nHeight, bool bRepaint);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        // Delegate to filter which windows to include 
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        /// <summary> Get the text for the window pointed to by hWnd </summary>
        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(
          IntPtr hWnd,
          IntPtr hWndInsertAfter,
          int X,
          int Y,
          int cx,
          int cy,
          UIntPtr uFlags
        );
        /// <summary> Find all windows that match the given filter </summary>
        /// <param name="filter"> A delegate that returns true for windows
        ///    that should be returned and false for windows that should
        ///    not be returned </param>
        public static IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)
        {
            IntPtr found = IntPtr.Zero;
            List<IntPtr> windows = new List<IntPtr>();

            EnumWindows(delegate (IntPtr wnd, IntPtr param)
            {
                if (filter(wnd, param))
                {
                    // only add the windows that pass the filter
                    windows.Add(wnd);
                }

                // but return true here so that we iterate all windows
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        /// <summary> Find all windows that contain the given title text </summary>
        /// <param name="titleText"> The text that the window title must contain. </param>
        public static IEnumerable<IntPtr> FindWindowsWithText(string titleText)
        {
            return FindWindows(delegate (IntPtr wnd, IntPtr param)
            {
                return GetWindowText(wnd).Contains(titleText);
            });
        }
    }
}
