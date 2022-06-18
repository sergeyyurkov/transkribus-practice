using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept
{
    public class LineRegion : RectangleRegion
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
                //NotifyPropertyChanged();
            }
        }
    }
}
