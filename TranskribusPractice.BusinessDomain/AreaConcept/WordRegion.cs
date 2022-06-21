using System;
using System.Collections.Generic;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept
{
    public class WordRegion : RectangleRegion, ICloneable
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

        public object Clone()
        {
            return new WordRegion()
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height,
                Name = this.Name,
                Content = this.Content,
                SelectionMode = false
            };
        }
    }
}
