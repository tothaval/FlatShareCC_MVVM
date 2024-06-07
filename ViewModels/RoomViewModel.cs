using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    public class RoomViewModel : BaseViewModel
    {
        private Room _room;
        public Room GetRoom => _room;

        public int ID
        {
            get { return _room.ID; }
            set
            {
                _room.ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        public string RoomName
        {
            get { return _room.RoomName; }
            set
            {
                _room.RoomName = value;
                OnPropertyChanged(nameof(RoomName));
                
            }
        }

        public double RoomArea
        {
            get { return _room.RoomArea; }
            set
            {
                _room.RoomArea = value;
                OnPropertyChanged(nameof(RoomArea));
            }
        }


        private double _CombinedPayments;

        public double CombinedPayments
        {
            get { return _CombinedPayments; }
            set
            {
                _CombinedPayments = value;
                OnPropertyChanged(nameof(CombinedPayments));
            }
        }


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


        public ObservableCollection<PaymentViewModel> Payments
        {
            get { return _room.Payments; }
            set
            {
                _room.Payments = value;
                OnPropertyChanged(nameof(Payments));
                OnPropertyChanged(nameof(CombinedPayments));
            }
        }

        public RoomViewModel(Room room)
        {
            _room = room;
        }

        public void RegisterPaymentEvents()
        {
            foreach (PaymentViewModel item in Payments)
            {
                item.PropertyChanged += Payment_Change;
            }
        }

        private void Payment_Change(object? sender, EventArgs e)
        {
            DetermineValues();
        }

        public void DetermineValues()
        {
            CombinedPayments = CalculateTotalPayments();

            StartDate = FindOldestPayment();
            EndDate = FindNewestPayment();
        }



        public double CalculateTotalPayments()
        {
            double total = 0.0;

            if (Payments != null)
            {
                foreach (PaymentViewModel item in Payments)
                {
                    total += item.PaymentTotal;
                }
            }


            return total;
        }

        public DateTime FindNewestPayment()
        {
            DateTime end = new DateTime();

            if (Payments != null)
            {
                foreach (PaymentViewModel item in Payments)
                {
                    if (item.PaymentQuantity > 1)
                    {
                        if (item.EndDate > end)
                        {
                            end = item.EndDate;
                        }                    
                    }
                    else
                    {
                        if (item.StartDate > end)
                        {
                            end = item.StartDate;
                        }
                    }
                }
            }

            return end;
        }

        public DateTime FindOldestPayment()
        {
            DateTime start = DateTime.Now;

            if (Payments != null)
            {
                foreach (PaymentViewModel item in Payments)
                {
                    if (item.StartDate < start)
                    {
                        start = item.StartDate;
                    }
                }
            }

            return start;
        }

    }
}
