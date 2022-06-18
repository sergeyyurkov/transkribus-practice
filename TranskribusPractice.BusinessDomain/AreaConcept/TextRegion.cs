using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept
{
    public class TextRegion : RectangleRegion
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
    }
}
