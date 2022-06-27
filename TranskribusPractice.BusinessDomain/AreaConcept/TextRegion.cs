using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept
{
    public class TextRegion : RectangleRegion, ICloneable
    {
        private ObservableCollection<LineRegion> _lines;
        public ObservableCollection<LineRegion> Lines
        {
            get
            {
                return _lines ?? (_lines = new ObservableCollection<LineRegion>());
            }
            set
            {
                if (_lines == value) return;
                _lines = value;
                NotifyPropertyChanged();
            }
        }
        public object Clone()
        {
            var lines = new ObservableCollection<LineRegion>();
            foreach (var line in this.Lines)
            {
                lines.Add((LineRegion)line.Clone());
            }
            return new TextRegion()
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Name = this.Name,
                IsSelected = false,
                Lines = lines
            };
        }
    }
}
