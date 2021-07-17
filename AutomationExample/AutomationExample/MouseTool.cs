using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutomationExample
{
    class MouseTool
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };



        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;



        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }


        internal static void MoveTo(Point point)
        {
            SetCursorPos(point.X, point.Y);
        }

        internal static void MouseClick(int type)
        {

            Point p = GetMousePosition();
            mouse_event(type, p.X, p.Y, 0, 0);
        }

        internal static void MouseClick(Point p)
        {
            Point old = GetMousePosition();
            MoveTo(p);
            mouse_event(MOUSEEVENTF_LEFTDOWN, p.X, p.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, p.X, p.Y, 0, 0);
            MoveTo(old);
        }

        internal static void MouseClick(System.Windows.Point p)
        {
            MouseClick(new Point((int)p.X, (int)p.Y));
        }

    }
}
