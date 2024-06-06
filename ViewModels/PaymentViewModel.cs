using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        private readonly Payment _payment;
        public Payment GetPayment => _payment;


        public DateTime StartDate
        {
            get { return _payment.StartDate; }
            set
            {
                _payment.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public DateTime EndDate
        {
            get { return _payment.EndDate; }
            set
            {
                _payment.EndDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }


        public int PaymentQuantity
        {
            get { return _payment.PaymentQuantity; }
            set
            {
                _payment.PaymentQuantity = value;
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
