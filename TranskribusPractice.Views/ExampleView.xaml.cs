﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranskribusPractice.Views
{
    /// <summary>
    /// Interaction logic for ExampleView.xaml
    /// </summary>
    public partial class ExampleView : UserControl
    {
        private ViewModels.IMouseAware _mouseAware;
        private ViewModels.IKeyboardAware _keyboardAware;
        private ViewModels.IFocusAware _focusAware;
        public ExampleView()
        {
            InitializeComponent();

        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _mouseAware = DataContext as ViewModels.IMouseAware;
            _keyboardAware = DataContext as ViewModels.IKeyboardAware;
            _focusAware = DataContext as ViewModels.IFocusAware;
        }
        public void ImageArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(CanvasRectangleArea);
            _mouseAware.RectangleMouseDown(mousePosition.X, mousePosition.Y);
        }
        public void ImageArea_MouseMove(object sender, MouseEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(CanvasRectangleArea);
            _mouseAware.RectangleMouseMove(mousePosition.X, mousePosition.Y);
        }
        public void ImageArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(CanvasRectangleArea);
            _mouseAware.RectangleMouseUp(mousePosition.X, mousePosition.Y);
        }

        private void ImageArea_KeyDown(object sender, KeyEventArgs e)
        {
            var keyboardAware = _keyboardAware;
            if (keyboardAware is null) return;
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                keyboardAware.DeleteSelectedRectangle();
            }
        }

        private void ListBoxRectangles_LostFocus(object sender, RoutedEventArgs e)
        {
            //TODO refactor
            //ListBoxRectangles.SelectedIndex = -1;
        }
    }
}
