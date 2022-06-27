using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.Views.Design
{
    public class SelectedRectangleConverter : IMultiValueConverter
    {
        //TODO refactor
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            
            return values[1];
        }
        //TODO refactor
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
           return Enumerable.Repeat(value, targetTypes.Length).ToArray();
        }
    }
}
