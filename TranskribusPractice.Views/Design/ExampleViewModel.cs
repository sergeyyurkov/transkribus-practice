using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TranskribusPractice.Views.Design
{
    internal class ExampleViewModel : ViewModels.ExampleViewModel
    {
        public override string Text { get; set; } = "Example text";

        public override ICommand SaveCommand => null;
    }
}
