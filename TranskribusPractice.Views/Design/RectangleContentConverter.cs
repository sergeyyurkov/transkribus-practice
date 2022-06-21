using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Data;
using TranskribusPractice.BusinessDomain.AreaConcept;

namespace TranskribusPractice.Views.Design
{
    public class RectangleContentConverter : IValueConverter
    {
        private WordRegion _word;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WordRegion word)
            {
                _word = word;
                return word.Content;
            }
            else 
            {
                _word = null;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if(!(_word is null))
            {
                _word.Content = strValue;
            }
            return strValue;
        }
    }
}
