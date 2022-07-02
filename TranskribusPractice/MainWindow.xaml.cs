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
using TranskribusPractice.ViewModels.Implementations;
using TranskribusPractice.ViewModels.ImplementationsNetFramework;
namespace TranskribusPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _mainView.DataContext = new ViewModels.ImplementationsNetFramework.ViewModelImpl();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModelImpl = _mainView.DataContext as ViewModels.ImplementationsNetFramework.ViewModelImpl;
            viewModelImpl?.ClosingWindowCommand?.Execute(e);
        }
    }
}
