using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TranskribusPractice.ViewModels.Implementations
{
    public class ExampleViewModel : ViewModels.ExampleViewModel
    {
        public override string Text { get; set; } = "The real implementation";

        public override ICommand SaveCommand => null;
    }
}
