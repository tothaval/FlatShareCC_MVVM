/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ListToStringConverter 
 * 
 *  helper class for the convertion of list values
 *  to a concatenated string
 */

using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace SharedLivingCostCalculator.Converters
{
    [ValueConversion(typeof(ObservableCollection<string>), typeof(string))]
    public class ListToStringConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(string))
                throw new InvalidOperationException("The target must be a String");

            return String.Join(", ", ((ObservableCollection<string>)value).ToArray());
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
// EOF