using System;
using System.Windows;
using System.Windows.Controls;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.Views.Design
{
    public class RectangleRegionTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is TextRegion)
            {
                return element.FindResource("TextTemplate") as DataTemplate;
            }
            else if (item is LineRegion)
            {
                return element.FindResource("LineTemplate") as DataTemplate;
            }
            else if (item is WordRegion)
            {
                return element.FindResource("WordTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
