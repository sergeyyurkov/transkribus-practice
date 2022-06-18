using System;
using System.Collections.Generic;
using System.Text;

namespace TranskribusPractice.ViewModels
{
    public interface IMouseAware
    {
        void RectangleMouseDown(double x, double y);
        void RectangleMouseMove(double x, double y);
        void RectangleMouseUp(double x, double y);
    }
}
