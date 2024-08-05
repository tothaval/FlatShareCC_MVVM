/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  BaseViewModel
 * 
 *  helper class that implements INotifyPropertyChanged
 */
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        // event properties & fields
        #region event properties & fields

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // methods
        #region methods

        protected virtual void Dispose() { }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion methods


    }
}
// EOF