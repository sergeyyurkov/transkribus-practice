using System;
using System.Collections.Generic;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept
{
    public class WordRegion : RectangleRegion
    {
        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (_content == value) return;
                _content = value;
                NotifyPropertyChanged();
            }
        }
    }
}
