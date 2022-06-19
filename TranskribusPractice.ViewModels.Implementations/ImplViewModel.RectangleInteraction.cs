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
        private Region _mode = Region.Text;
        private RectangleRegion _selectedRectangle;
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
            if (RectangleWidth <= 4 || RectangleHeight <= 4)
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
                    break;
                }
            }
        }
        public RectangleRegion DefineParent(RectangleRegion child, IEnumerable<RectangleRegion> parents)
        {
            double parentArea = 0;
            RectangleRegion parent = null;
            foreach (var rect in parents)
            {
                if (IsIntersect(rect, child))
                {
                    RectangleRegion intersectedRectangle = CreateIntersectedRect(rect, child);
                    double area = intersectedRectangle.CalculateArea();
                    if (parentArea < area)
                    {
                        parent = rect;
                        parentArea = area;
                    }
                }
            }
            return parent;
        }
        public TextRegion DefineParentText()
        {
            RectangleRegion line = new RectangleRegion()
            {
                X = RectangleCanvasLeft,
                Y = RectangleCanvasTop,
                Width = RectangleWidth,
                Height = RectangleHeight
            };
            double parentTextArea = 0;
            TextRegion parentText = null;
            foreach (var text in TextRegions)
            {
                if (IsIntersect(text, line))
                {
                    RectangleRegion intersectedRectangle = CreateIntersectedRect(text, line);
                    double textArea = intersectedRectangle.CalculateArea();
                    if (parentTextArea < textArea)
                    {
                        parentText = text;
                        parentTextArea = textArea;
                    }
                }
            }
            return parentText;
        }
        public LineRegion DefineParentLine() 
        {
            RectangleRegion word = new RectangleRegion()
            {
                X = RectangleCanvasLeft,
                Y = RectangleCanvasTop,
                Width = RectangleWidth,
                Height = RectangleHeight
            };
            double parentLineArea = 0;
            LineRegion parentLine = null;
            foreach (var text in TextRegions)
            {
                foreach(var line in text.Lines) 
                {
                    if (IsIntersect(line, word))
                    {
                        RectangleRegion intersectedRectangle = CreateIntersectedRect(line, word);
                        double lineArea = intersectedRectangle.CalculateArea();
                        if (parentLineArea < lineArea)
                        {
                            parentLine = line;
                            parentLineArea = lineArea;
                        }
                    }
                }
            }
            return parentLine;
        }
        private RectangleRegion CreateIntersectedRect(RectangleRegion rect1, RectangleRegion rect2)
        {
            double x1, y1, x2, y2;
            if (rect2.X < rect1.X)
            {
                x1 = rect1.X;
            }
            else
            {
                x1 = rect2.X;
            }
            if (rect1.X + rect1.Width < rect2.X + rect2.Width)
            {
                x2 = rect1.X + rect1.Width;
            }
            else
            {
                x2 = rect2.X + rect2.Width;
            }
            if (rect2.Y < rect1.Y)
            {
                y1 = rect1.Y;
            }
            else
            {
                y1 = rect2.Y;
            }
            if (rect1.Y + rect1.Height < rect2.Y + rect2.Height)
            {
                y2 = rect1.Y + rect1.Height;
            }
            else
            {
                y2 = rect2.Y + rect2.Height;
            }
            return new RectangleRegion() { X = x1, Y =  y1, Width = x2-x1, Height = y2-y1 };
        }
        private bool IsIntersect(RectangleRegion rect1, RectangleRegion rect2) 
        {
            bool aLeftOfB = (rect1.X + rect1.Width) < rect2.X;
            bool aRightOfB = rect1.X > (rect2.X + rect2.Width);
            bool aAboveB = (rect1.Y + rect1.Height) < rect2.Y;
            bool aBelowB = rect1.Y > (rect2.Y + rect2.Height);
            return !(aLeftOfB || aRightOfB || aAboveB || aBelowB);
        }
        public virtual void RectangleMouseDown(double x, double y)
        {
            RectangleVisibility = true;
            RectangleWidth = 0;
            RectangleHeight = 0;
            _startX = x;
            _startY = y;
        }
        public void RectangleMouseMove(double x, double y)
        {
            if (RectangleVisibility)
            {
                CalculateRecntangle(x, y);
            }
        }
        public void RectangleMouseUp(double x, double y)
        {
            RectangleVisibility = false;
            SelectRectangle();
            if (IsSmallRectangle())
            {
                return;
            }
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
                    TextRegion ParentTextRegion = DefineParentText();
                    ParentTextRegion?.Lines.Add(
                        creator.CreateLineRegion("Line " + (ParentTextRegion.Lines.Count + 1)));
                    break;
                case Region.Word:
                    LineRegion ParentLineRegion = DefineParentLine();
                    ParentLineRegion?.Words.Add(
                        creator.CreateWordRegion("Word " + (ParentLineRegion.Words.Count + 1)));
                    break;
                default: break;
            }
            UpdateAllRegions();
        }
    }
}
