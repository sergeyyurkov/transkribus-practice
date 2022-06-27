using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept
{
    public class LineRegion : RectangleRegion, ICloneable
    {
        private ObservableCollection<WordRegion> _words;
        public ObservableCollection<WordRegion> Words
        {
            get
            {
                return _words ?? (_words = new ObservableCollection<WordRegion>());
            }
            set
            {
                if (_words == value) return;
                _words = value;
                NotifyPropertyChanged();
            }
        }

        public object Clone()
        {
            var words = new ObservableCollection<WordRegion>();
            foreach (var word in this.Words)
            {
                words.Add((WordRegion)word.Clone());
            }
            return new LineRegion()
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Name = this.Name,
                IsSelected = false,
                Words = words
            };
        }
    }
}
