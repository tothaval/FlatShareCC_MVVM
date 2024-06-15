/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PaymentViewModel  : BaseViewModel
 * 
 *  viewmodel for Payment model
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System.Collections;
using System.ComponentModel;


namespace SharedLivingCostCalculator.ViewModels
{
    public class PaymentViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private readonly Payment _payment;


        public Payment GetPayment => _payment;


        public DateTime StartDate
        {
            get { return _payment.StartDate; }
            set
            {
                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (StartDate == EndDate || EndDate < StartDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(StartDate));
                }
                _payment.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }


        public DateTime EndDate
        {
            get { return _payment.EndDate; }
            set
            {
                _helper.ClearError(nameof(StartDate));
                _helper.ClearError(nameof(EndDate));

                if (StartDate == EndDate || EndDate < StartDate)
                {
                    _helper.AddError("start date must be before enddate", nameof(EndDate));
                }
                _payment.EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public int PaymentQuantity
        {
            get { return _payment.PaymentQuantity; }
            set
            {
                _helper.ClearError(nameof(PaymentQuantity));

                if (Double.IsNaN(value))
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
            }
        }


        public double Sum
        {
            get { return _payment.Sum; }
            set
            {
                _helper.ClearError(nameof(Sum));

                if (Double.IsNaN(value))
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
            }
        }


        public double PaymentTotal => Sum * PaymentQuantity;


        public bool EndDateVisible => PaymentQuantity > 1;


        public PaymentViewModel(Payment payment)
        {
            _payment = payment;
        }


    }
}
// EOF