/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  VisibilityConverter 
 * 
 *  helper class for the convertion of boolean values
 *  to Visibility.Hidden or Visibility.Visible
 */
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace SharedLivingCostCalculator.Converters
{
    internal class VisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
// EOF