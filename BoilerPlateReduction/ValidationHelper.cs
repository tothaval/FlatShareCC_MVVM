using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.BoilerPlateReduction
{
    public class ValidationHelper : INotifyDataErrorInfo
    {
        /// <summary>
        ///  replace this class with another one which implements
        ///  IDataErrorInfo
        ///  
        /// there is a bug on startup with initialization of 
        /// date values, the default value is checked and the 
        /// error raised, before EndData value is set via constructor
        /// 
        /// the check does not happen again
        /// only resetting the date to another value will recall
        /// the event. no clue on how to or when to reraise the event
        /// or the validation on Enddate. very ugly.
        /// </summary>

        private IDictionary<string, List<string>> errorList = new Dictionary<string, List<string>>();


        public string this[string propertyName]
        {
            get
            {
                if (errorList.ContainsKey(propertyName))
                {
                    return errorList[propertyName].First();
                }
                return String.Empty;
            }
        }


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

        // INotifyDataErrorInfo interface implementation
        #region INotifyDataErrorInfo
        public bool HasErrors => errorList.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (errorList.ContainsKey(propertyName))
            {
                return errorList[propertyName];
            }
            return Array.Empty<string>();
        }
        #endregion INotifyDataErrorInfo
    }
}
