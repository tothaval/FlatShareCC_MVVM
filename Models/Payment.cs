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
    internal class Payment : INotifyPropertyChanged
    {
        private readonly RoomViewModel _roomViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;
    
        public DateTime StartDate {  get; set; }
        public DateTime EndDate { get; set; }

        private int _paymentQuantity;

        public int PaymentQuantity
        {
            get { return _paymentQuantity; }
            set { _paymentQuantity = value;
                OnPropertyChanged(nameof(PaymentQuantity));
                OnPropertyChanged(nameof(PaymentTotal));
                _roomViewModel.CalculatePayments();
            }
        }

        private double _sum;

        public double Sum
        {
            get { return _sum; }
            set { _sum = value;
                OnPropertyChanged(nameof(Sum));
                OnPropertyChanged(nameof(PaymentTotal));
                _roomViewModel.CalculatePayments();
            }
        }

        public double PaymentTotal => Sum * PaymentQuantity;

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
