using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace TranskribusPractice.BusinessDomain.AreaConcept 
{
    public class RectangleRegion : INotifyPropertyChanged
    {
        private string _name;
        private bool _isSelected;
        public event EventHandler SelectRegion;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        [XmlIgnore]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                NotifyPropertyChanged();
                if (_isSelected)
                {
                    SelectRegion?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public double CalculateArea()
        {
            return Width * Height;
        }
    }
}
