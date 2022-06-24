using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TranskribusPractice.ViewModels.Implementations
{
    public static class SystemMetrics
    {
        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int index);

        //get SM_CXDRAG; value 68
        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        //The number of pixels on either side of a mouse-down point
        //that the mouse pointer can move before a drag operation begins.
        public static int GetMinimalDragDistance()
        {
            return GetSystemMetrics(68);
        }
    }
}
