/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ValidationHelper 
 * 
 *  helper class that implements INotifyDataErrorInfo
 *  for data validation upon user input
 */
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharedLivingCostCalculator.Utility
{
    public class ValidationHelper : INotifyDataErrorInfo
    {

        // properties & fields
        #region properties

        public string this[string propertyName]
        {
            get
            {
                if (errorList.ContainsKey(propertyName))
                {
                    return errorList[propertyName].First();
                }
                return string.Empty;
            }
        }


        public bool HasErrors => errorList.Count > 0;

        #endregion properties


        // event properties
        #region event properties

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        #endregion event properties


        // collections
        #region collections

        private IDictionary<string, List<string>> errorList = new Dictionary<string, List<string>>();

        #endregion collections


        // constructors
        #region constructors

        public ValidationHelper()
        {

        }

        #endregion constructors


        // methods
        #region methods

        public void AddError(string message, [CallerMemberName] string property = "")
        {
            if (!errorList.ContainsKey(property))
            {
                errorList.Add(property, new List<string>());
            }
            errorList[property].Add(message);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
        }


        public void ClearError([CallerMemberName] string property = "")
        {
            if (errorList.Remove(property))
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(property));
            }

        }


        public IEnumerable GetErrors(string? propertyName)
        {
            if (errorList.ContainsKey(propertyName))
            {
                return errorList[propertyName];
            }
            return Array.Empty<string>();
        }

        #endregion methods


    }
}
// EOF