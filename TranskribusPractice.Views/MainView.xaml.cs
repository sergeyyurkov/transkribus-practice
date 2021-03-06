using System;
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
using Microsoft.Xaml.Behaviors;

namespace TranskribusPractice.Views
{
    /// <summary>
    /// Interaction logic for ExampleView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private int _lastSelectedRectangleIndex = -1;
        private ViewModels.IMouseAware _mouseAware;
        public MainView()
        {
            InitializeComponent();
        }
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _mouseAware = DataContext as ViewModels.IMouseAware;
        }
        public void ImageArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(CanvasRectangleArea);
            _mouseAware.RectangleMouseDown(mousePosition.X,mousePosition.Y);
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
            if(_lastSelectedRectangleIndex == ListBoxRectangles.SelectedIndex) 
            {
                ListBoxRectangles.SelectedIndex = -1;
            }
            _lastSelectedRectangleIndex = ListBoxRectangles.SelectedIndex;
            var mousePosition = e.GetPosition(CanvasRectangleArea);
            _mouseAware.RectangleMouseUp(mousePosition.X, mousePosition.Y);
        }
    }
}
