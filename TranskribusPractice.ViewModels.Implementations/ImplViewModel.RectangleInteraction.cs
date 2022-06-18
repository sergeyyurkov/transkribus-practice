using System;
using System.Collections.Generic;
using System.Text;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.ViewModels.Implementations
{
    public partial class ImplViewModel
    {
        private double _startX;
        private double _startY;
        private double _rectangleWidth;
        private double _rectangleHeight;
        private double _rectangleCanvasLeft;
        private double _rectangleCanvasTop;
        private bool _rectangleVisibility;
        private TextRegion _mouseDownTextRegion;
        private LineRegion _mouseDownLineRegion;
        private TextRegion _mouseUpTextRegion;
        private LineRegion _mouseUpLineRegion;
        private Region _mode = Region.Text;
        private RectangleRegion _selectedRectangle;
        public bool IsStartedDrawing { get; set; }
        public override double RectangleWidth
        {
            get => _rectangleWidth;
            set
            {
                if (_rectangleWidth == value) return;
                _rectangleWidth = value;
                NotifyPropertyChanged();
            }
        }
        public override double RectangleHeight
        {
            get => _rectangleHeight;
            set
            {
                if (_rectangleHeight == value) return;
                _rectangleHeight = value;
                NotifyPropertyChanged();
            }
        }
        public override double RectangleCanvasLeft
        {
            get => _rectangleCanvasLeft;
            set
            {
                if (_rectangleCanvasLeft == value) return;
                _rectangleCanvasLeft = value;
                NotifyPropertyChanged();
            }
        }
        public override double RectangleCanvasTop
        {
            get => _rectangleCanvasTop;
            set
            {
                if (_rectangleCanvasTop == value) return;
                _rectangleCanvasTop = value;
                NotifyPropertyChanged();
            }
        }
        public override Region Mode
        {
            get => _mode;
            set
            {
                if (_mode == value) return;
                _mode = value;
                NotifyPropertyChanged();
            }
        }
        public override bool RectangleVisibility
        {
            get => _rectangleVisibility;
            set
            {
                if (_rectangleVisibility == value) return;
                _rectangleVisibility = value;
                NotifyPropertyChanged();
            }
        }
        public RectangleRegion SelectedRectangle
        {
            get => _selectedRectangle;
            set
            {
                if (_selectedRectangle == value) return;
                _selectedRectangle = value;
                NotifyPropertyChanged();
            }
        }
        private bool IsInRectangle(double x, double y, RectangleRegion rectangle)
        {
            if (rectangle.X <= x
                && rectangle.Y <= y
                && (rectangle.X + rectangle.Width) >= x
                && (rectangle.Y + rectangle.Height) >= y)
            {
                return true;
            }
            return false;
        }
        public bool IsSmallRectangle()
        {
            // TODO SystemParameters.MinimumHorizontalDragDistance 
            if (RectangleWidth <= 4
                  || RectangleHeight <= 4)
            {
                return true;
            }
            return false;
        }
        private void CalculateRecntangle(double endX, double endY)
        {
            RectangleCanvasLeft = _startX <= endX ? _startX : endX;
            RectangleCanvasTop = _startY <= endY ? _startY : endY;
            RectangleWidth = Math.Abs(endX - _startX);
            RectangleHeight = Math.Abs(endY - _startY);
        }
        private void SelectRectangle()
        {
            foreach (var text in TextRegions)
            {
                if (IsInRectangle(_startX, _startY, text))
                {
                    TextRegion tr = text;
                    SelectedRectangle = tr;
                    foreach (var line in tr.Lines)
                    {
                        if (IsInRectangle(_startX, _startY, line))
                        {
                            LineRegion lr = line;
                            SelectedRectangle = lr;
                            foreach (var word in lr.Words)
                            {
                                if (IsInRectangle(_startX, _startY, word))
                                {
                                    SelectedRectangle = word;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    break; // TODO delete break and check if two rectangles or more
                }
            }
        }
        private TextRegion FindTextRegion(double x, double y)
        {
            foreach (var text in TextRegions)
            {
                if (IsInRectangle(x, y, text))
                {
                    return text;
                }
            }
            return null;
        }
        private LineRegion FindLineRegion(double x, double y, TextRegion text)
        {
            if (!(text is null))
            {
                foreach (var line in text.Lines)
                {
                    if (IsInRectangle(x, y, line))
                    {
                        return line;
                    }
                }
            }
            return null;
        }
        //TODO refactor
        private void FindParentRectangleMouseDown(double x, double y)
        {
            if (Mode != Region.Text)
            {
                _mouseDownTextRegion = FindTextRegion(x, y);
            }
            if (Mode == Region.Word)
            {
                _mouseDownLineRegion = FindLineRegion(x, y, _mouseDownTextRegion);
            }
        }
        //TODO refactor
        private void FindParentRectangleMouseUp(double x, double y)
        {
            if (Mode != Region.Text)
            {
                _mouseUpTextRegion = FindTextRegion(x, y);
            }
            if (Mode == Region.Word)
            {
                _mouseUpLineRegion = FindLineRegion(x, y, _mouseUpTextRegion);
            }
        }
        //TODO change completely
        private LineRegion DefineParentRectangle(double x, double y)
        {
            LineRegion ParentLineRegion = _mouseDownLineRegion;
            if (_mouseDownLineRegion == _mouseUpLineRegion)
            {
                return ParentLineRegion;
            }
            double mouseDownRectHeight;
            if (_startY <= y)
            {
                mouseDownRectHeight = _mouseDownLineRegion.Y + _mouseDownLineRegion.Height - RectangleCanvasTop;
            }
            else
            {
                mouseDownRectHeight = RectangleCanvasTop - _mouseDownLineRegion.Y;
            }
            double mouseUpRectHeight;
            if (_startY <= y)
            {
                mouseUpRectHeight = RectangleHeight + RectangleCanvasTop - _mouseUpLineRegion.Y;
            }
            else
            {
                mouseUpRectHeight = _mouseUpLineRegion.Y + _mouseUpLineRegion.Height - (RectangleHeight + RectangleCanvasTop);
            }
            if (mouseDownRectHeight < mouseUpRectHeight)
            {
                ParentLineRegion = _mouseUpLineRegion;
            }
            return ParentLineRegion;
        }
        public virtual void RectangleMouseDown(double x, double y)
        {
            RectangleWidth = 0;
            RectangleHeight = 0;
            IsStartedDrawing = true;
            _startX = x;
            _startY = y;
            FindParentRectangleMouseDown(x, y);
            RectangleVisibility = true;
        }
        public void RectangleMouseMove(double x, double y)
        {
            if (IsStartedDrawing)
            {
                CalculateRecntangle(x, y);
            }
        }
        public void RectangleMouseUp(double x, double y)
        {
            IsStartedDrawing = false;
            RectangleVisibility = false;
            SelectRectangle();
            if (IsSmallRectangle())
            {
                return;
            }
            FindParentRectangleMouseUp(x, y);
            LineRegion ParentLineRegion = DefineParentRectangle(x, y);
            RegionCreator creator = new RegionCreator(
                RectangleCanvasLeft,
                RectangleCanvasTop,
                RectangleWidth,
                RectangleHeight);
            switch (Mode)
            {
                case Region.Text:
                    TextRegions.Add(
                        creator.CreateTextRegion("Paragraph " + (TextRegions.Count + 1)));
                    break;
                case Region.Line:
                    _mouseDownTextRegion?.Lines.Add(
                        creator.CreateLineRegion("Line " + (_mouseDownTextRegion.Lines.Count + 1)));
                    break;
                case Region.Word:
                    ParentLineRegion?.Words.Add(
                        creator.CreateWordRegion("Word " + (ParentLineRegion.Words.Count + 1)));
                    break;
                default: break;
            }
            UpdateAllRegions();
        }
    }
}
