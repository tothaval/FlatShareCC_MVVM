/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PaymentViewModel  : BaseViewModel
 * 
 *  viewmodel for Payment model
 */
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class PaymentViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties & fields

        public DateTime EndDate
        {
            get { return _payment.EndDate; }
            set
            {
                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (PaymentQuantity > 1)
                {
                    if (StartDate == EndDate || EndDate < StartDate)
                    {
                        _helper.AddError("start date must be before enddate", nameof(EndDate));
                    }
                }

                _payment.EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public bool EndDateVisible => PaymentQuantity > 1;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public bool HasErrors => _helper.HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


        private readonly Payment _payment;
        public Payment GetPayment => _payment;


        public int PaymentQuantity
        {
            get { return _payment.PaymentQuantity; }
            set
            {
                _helper.ClearError(nameof(PaymentQuantity));

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(PaymentQuantity));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(PaymentQuantity));
                }

                _payment.PaymentQuantity = value;

                if (_payment.PaymentQuantity == 1)
                {
                    EndDate = StartDate;
                }

                OnPropertyChanged(nameof(PaymentQuantity));
                OnPropertyChanged(nameof(PaymentTotal));
                OnPropertyChanged(nameof(EndDateVisible));

                PaymentChange.Invoke(this, EventArgs.Empty);
            }
        }

        public double PaymentTotal => Sum * PaymentQuantity;


        public DateTime StartDate
        {
            get { return _payment.StartDate; }
            set
            {
                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (PaymentQuantity < 2)
                {
                    if (StartDate == EndDate || EndDate < StartDate)
                    {
                        _helper.AddError("start date must be before enddate", nameof(StartDate));
                    } 
                }

                _payment.StartDate = value;

                if (_payment.PaymentQuantity == 1)
                {
                    EndDate = StartDate;
                }

                OnPropertyChanged(nameof(StartDate));
            }
        }

        public double Sum
        {
            get { return _payment.Sum; }
            set
            {
                _helper.ClearError(nameof(Sum));

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(Sum));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(Sum));
                }

                _payment.Sum = value;
                OnPropertyChanged(nameof(Sum));
                OnPropertyChanged(nameof(PaymentTotal));

                PaymentChange.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event EventHandler? PaymentChange;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // constructors
        #region constructors

        public PaymentViewModel(Payment payment)
        {
            _payment = payment;
        }

        #endregion constructors


    }
}
// EOF