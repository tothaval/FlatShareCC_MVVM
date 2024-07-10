/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  InverseVisibilityConverter 
 * 
 *  helper class for the inversed
 *  convertion of boolean values
 *  to Visibility.Collapsed or Visibility.Visible
 */
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SharedLivingCostCalculator.Converters
{
    internal class InverseVisibilityConverter : IValueConverter
    {

        // methods
        #region methods
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Collapsed : Visibility.Visible;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion methods


    }
}
// EOF