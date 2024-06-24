using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RoomPaymentsViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public double CombinedPayments => CalculateTotalPayments();


        public int ID => RoomPayments.RoomID;
        public string RoomName => GetRoomName();
        public double RoomArea => GetRoomArea();

        public double GetRoomArea()
        {
            if (RoomPayments != null && RoomPayments.RoomViewModel != null)
            {
                return RoomPayments.RoomViewModel.RoomArea;
            }

            return 0.0;
        }

        public string GetRoomName()
        {
            if (RoomPayments != null && RoomPayments.RoomViewModel != null)
            {
                return RoomPayments.RoomViewModel.RoomName;
            }

            return "unknown";
        }



        private RoomPayments _RoomPayments;
        public RoomPayments RoomPayments
        {
            get { return _RoomPayments; }
            set
            {
                _RoomPayments = value;

                OnPropertyChanged(nameof(RoomPayments));
            }
        }

        //private ObservableCollection<PaymentViewModel> _PaymentViewModels;
        public ObservableCollection<PaymentViewModel> PaymentViewModels => GetPaymentViewModels();
        //{
        //    get { return _PaymentViewModels; }
        //    set {
        //        _PaymentViewModels = value;
        //        OnPropertyChanged(nameof(PaymentViewModels));
        //    }
        //}


        public RoomPaymentsViewModel(RoomPayments roomPayments)
        {
            RoomPayments = roomPayments;
        }


        public double CalculateTotalPayments()
        {
            double total = 0.0;

            if (RoomPayments != null)
            {
                foreach (Payment payment in RoomPayments.Payments)
                {
                    total += payment.PaymentTotal;
                }
            }

            return total;
        }


        private ObservableCollection<PaymentViewModel> GetPaymentViewModels()
        {
            ObservableCollection<PaymentViewModel> paymentViewModels = new ObservableCollection<PaymentViewModel>();

            foreach (Payment payment in RoomPayments.Payments)
            {
                PaymentViewModel paymentViewModel = new PaymentViewModel(payment);

                paymentViewModel.PaymentChange += PaymentViewModel_PaymentChange;

                paymentViewModels.Add(paymentViewModel);
            }

            return paymentViewModels;
        }

        private void PaymentViewModel_PaymentChange(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(CombinedPayments));
        }

        //private void SetPaymentViewModels()
        //{
        //    if (PaymentViewModels != null)
        //    {
        //        RoomPayments.Payments.Clear();


        //        foreach (PaymentViewModel paymentViewModel in PaymentViewModels)
        //        {
        //            RoomPayments.Payments.Add(paymentViewModel.GetPayment);
        //        }
        //    }

        //}

        //public DateTime FindNewestPayment()
        //{
        //    DateTime end = new DateTime();

        //    if (Payments != null)
        //    {
        //        foreach (PaymentViewModel item in Payments)
        //        {
        //            if (item.PaymentQuantity > 1)
        //            {
        //                if (item.EndDate > end)
        //                {
        //                    end = item.EndDate;
        //                }
        //            }
        //            else
        //            {
        //                if (item.StartDate > end)
        //                {
        //                    end = item.StartDate;
        //                }
        //            }
        //        }
        //    }

        //    return end;
        //}


        //public DateTime FindOldestPayment()
        //{
        //    DateTime start = DateTime.Now;

        //    if (Payments != null)
        //    {
        //        foreach (PaymentViewModel item in Payments)
        //        {
        //            if (item.StartDate < start)
        //            {
        //                start = item.StartDate;
        //            }
        //        }
        //    }

        //    return start;
        //}


    }
}
// EOF