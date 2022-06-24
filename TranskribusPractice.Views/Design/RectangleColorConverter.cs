using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.Views.Design
{
    public class RectangleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Region.Text)
            {
                return new SolidColorBrush(Colors.Red);
            }
            else if (value is Region.Line)
            {
                return new SolidColorBrush(Colors.Blue);
            }
            else if (value is Region.Word)
            {
                return new SolidColorBrush(Colors.Green);
            }
            return new SolidColorBrush(Colors.Black);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
