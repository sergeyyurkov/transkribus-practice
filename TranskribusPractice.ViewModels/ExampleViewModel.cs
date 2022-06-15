using System;
using System.Windows.Input;

namespace TranskribusPractice.ViewModels
{
    public abstract class ExampleViewModel
    {
        public abstract string Text { get; set; }

        public abstract ICommand SaveCommand { get; }
    }
}
