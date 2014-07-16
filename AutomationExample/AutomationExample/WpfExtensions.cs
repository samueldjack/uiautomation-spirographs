using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AutomationExample
{
    static class WpfExtensions
    {
        public static System.Drawing.Point ToDrawingPoint(this System.Windows.Point point)
        {
            return new Point((int)point.X, (int)point.Y);
        }
    }
}
