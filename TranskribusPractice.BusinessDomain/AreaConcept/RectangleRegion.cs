﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TranskribusPractice.BusinessDomain.AreaConcept 
{
    public class RectangleRegion : INotifyPropertyChanged
    {
        private string _name;
        private bool _selectionMode;
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
        public  bool SelectionMode
        {
            get => _selectionMode;
            set
            {
                if (_selectionMode == value) return;
                _selectionMode = value;
                NotifyPropertyChanged();
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
