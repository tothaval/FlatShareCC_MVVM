/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        // Properties & Fields
        #region Properties & Fields

        public DateTime EndDate
        {
            get { return _Payment.EndDate; }
            set
            {
                _Helper.ClearError(nameof(StartDate));
                _Helper.ClearError(nameof(EndDate));

                if (PaymentQuantity > 1)
                {
                    if (StartDate == EndDate || EndDate < StartDate)
                    {
                        _Helper.AddError("start date must be before enddate", nameof(EndDate));
                    }
                }

                _Payment.EndDate = value;
                OnPropertyChanged(nameof(EndDate));

                PaymentChange.Invoke(this, EventArgs.Empty);
            }
        }


        public bool EndDateVisible => PaymentQuantity > 1;


        public bool HasErrors => _Helper.HasErrors;


        private ValidationHelper _Helper = new ValidationHelper();


        private readonly Payment _Payment;
        public Payment Payment => _Payment;


        public int PaymentQuantity
        {
            get { return _Payment.PaymentQuantity; }
            set
            {
                _Helper.ClearError(nameof(PaymentQuantity));

                if (double.IsNaN(value))
                {
                    _Helper.AddError("value must be a number", nameof(PaymentQuantity));
                }

                if (value < 0)
                {
                    _Helper.AddError("value must be greater than 0", nameof(PaymentQuantity));
                }

                _Payment.PaymentQuantity = value;

                if (_Payment.PaymentQuantity == 1)
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
            get { return _Payment.StartDate; }
            set
            {
                _Helper.ClearError(nameof(StartDate));
                _Helper.ClearError(nameof(EndDate));

                if (PaymentQuantity < 2)
                {
                    if (StartDate == EndDate || EndDate < StartDate)
                    {
                        _Helper.AddError("start date must be before enddate", nameof(StartDate));
                    }
                }

                _Payment.StartDate = value;

                if (_Payment.PaymentQuantity == 1)
                {
                    EndDate = StartDate;
                }

                OnPropertyChanged(nameof(StartDate));

                PaymentChange.Invoke(this, EventArgs.Empty);
            }
        }


        public double Sum
        {
            get { return _Payment.Sum; }
            set
            {
                _Helper.ClearError(nameof(Sum));

                if (double.IsNaN(value))
                {
                    _Helper.AddError("value must be a number", nameof(Sum));
                }

                if (value < 0)
                {
                    _Helper.AddError("value must be greater than 0", nameof(Sum));
                }

                _Payment.Sum = value;
                OnPropertyChanged(nameof(Sum));
                OnPropertyChanged(nameof(PaymentTotal));

                PaymentChange.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion


        // Event Properties & Fields
        #region Event Properties & Fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event EventHandler? PaymentChange;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion


        // Constructors
        #region Constructors

        public PaymentViewModel(Payment payment)
        {
            _Payment = payment;
        }

        #endregion


        #region Methods

        public IEnumerable GetErrors(string? propertyName) => _Helper.GetErrors(propertyName); 
        
        #endregion


    }
}
// EOF