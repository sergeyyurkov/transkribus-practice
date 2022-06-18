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

namespace TranskribusPractice.Views
{
    /// <summary>
    /// Interaction logic for ExampleView.xaml
    /// </summary>
    public partial class ExampleView : UserControl
    {
        private ViewModels.IMouseAware _mouseAware;
        public ExampleView()
        {
            InitializeComponent();
        }
        //called in startup project after this datacontext has been changed
        public void UpdateInfo()
        {
            _mouseAware = this.DataContext as ViewModels.IMouseAware;
        } 
        public void ImageArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(ImageArea);
            _mouseAware.RectangleMouseDown(mousePosition.X, mousePosition.Y);
        }
        public void ImageArea_MouseMove(object sender, MouseEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(ImageArea);
            _mouseAware.RectangleMouseMove(mousePosition.X, mousePosition.Y);
        }
        public void ImageArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var mouseAware = _mouseAware;
            if (mouseAware is null) return;
            var mousePosition = e.GetPosition(ImageArea);
            _mouseAware.RectangleMouseUp(mousePosition.X, mousePosition.Y);
        }
    }
}
