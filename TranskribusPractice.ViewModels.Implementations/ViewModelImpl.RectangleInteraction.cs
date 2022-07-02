using System;
using System.Collections.Generic;
using System.Text;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.ViewModels.Implementations
{
    public partial class ViewModelImpl 
    {
        private double _startX;
        private double _startY;
        private double _rectangleWidth;
        private double _rectangleHeight;
        private double _rectangleCanvasLeft;
        private double _rectangleCanvasTop;
        private bool _rectangleVisibility;
        private Region _mode = Region.Undefined;
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
        public override RectangleRegion SelectedRectangle
        {
            get => _selectedRectangle;
            set
            {
                if (_selectedRectangle == value) return;
                _selectedRectangle = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsSmallRectangle()
        {
            if (RectangleWidth <= SystemMetrics.GetMinimalDragDistanceWidth()
                || RectangleHeight <= SystemMetrics.GetMinimalDragDistanceHeight())
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
            if (Mode == Region.Undefined) 
            {
                return;
            }
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
            if (IsSmallRectangle() || Mode == Region.Undefined)
            {
                BuildRichTextBox();
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
                    var tr = creator.CreateTextRegion("Paragraph " + (TextRegions.Count + 1));
                    tr.SelectRegion += SelectRegion;
                    TextRegions.Add(tr);
                    break;
                case Region.Line:
                    var ParentTextRegion = DefineParentText();
                    if(!(ParentTextRegion is null))
                    {
                        var lr = creator.CreateLineRegion("Line " + (ParentTextRegion.Lines.Count + 1));
                        lr.SelectRegion += SelectRegion;
                        ParentTextRegion?.Lines.Add(lr);
                    }
                    break;
                case Region.Word:
                    var ParentLineRegion = DefineParentLine();
                    if (!(ParentLineRegion is null))
                    { 
                        var wr = creator.CreateWordRegion("Word " + (ParentLineRegion.Words.Count + 1));
                        wr.SelectRegion += SelectRegion;
                        ParentLineRegion?.Words.Add(wr);
                    }
                    break;
                default: break;
            }
            UpdateAllRegions();
        }
        public void SelectRegion(object sender, EventArgs e)
        {
            SelectedRectangle = sender as RectangleRegion;
            BuildRichTextBox();
        }
    }
}
