using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    public class Payment : INotifyPropertyChanged
    {
        private readonly RoomViewModel _roomViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;


        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                _StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime _EndDate;
        public DateTime EndDate
        {
            get { return _EndDate; }
            set
            {
                _EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        private int _paymentQuantity;
        public int PaymentQuantity
        {
            get { return _paymentQuantity; }
            set { _paymentQuantity = value;
                OnPropertyChanged(nameof(PaymentQuantity));
                OnPropertyChanged(nameof(PaymentTotal));
                OnPropertyChanged(nameof(EndDateVisible));
            }
        }

        private double _sum;

        public double Sum
        {
            get { return _sum; }
            set { _sum = value;
                OnPropertyChanged(nameof(Sum));
                OnPropertyChanged(nameof(PaymentTotal));
            }
        }

        public double PaymentTotal => Sum * PaymentQuantity;

        public bool EndDateVisible => PaymentQuantity > 1;

        public Payment(RoomViewModel roomViewModel)
        {
            _roomViewModel = roomViewModel;                
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
