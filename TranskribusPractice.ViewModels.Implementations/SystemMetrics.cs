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
        const int SM_CXDRAG = 68;
        const int SM_CYDRAG = 69;
        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getsystemmetrics
        //The number of pixels on either side of a mouse-down point
        //that the mouse pointer can move before a drag operation begins.
        public static int GetMinimalDragDistanceWidth()
        {
            return GetSystemMetrics(SM_CXDRAG);
        }
        public static int GetMinimalDragDistanceHeight()
        {
            return GetSystemMetrics(SM_CYDRAG);
        }
    }
}
