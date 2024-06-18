using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    public class RoomPaymentsViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public double CombinedPayments => CalculateTotalPayments();


        private RoomViewModel _RoomViewModel;
        public RoomViewModel RoomViewModel
        {
            get { return _RoomViewModel; }
            set
            {
                _RoomViewModel = value;
                OnPropertyChanged(nameof(RoomViewModel));
            }
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


        public RoomPaymentsViewModel(RoomPayments roomPayments)
        {
            RoomPayments = roomPayments;

            RoomViewModel = roomPayments.RoomViewModel;
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