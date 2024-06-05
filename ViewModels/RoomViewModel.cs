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

        public int ID => _room.ID;
        public string RoomName => _room.RoomName;
        public double RoomArea => _room.RoomArea;

        public double CombinedPayments => CalculateTotalPayments();
        
        public DateTime StartDate => FindOldestPayment();
        public DateTime EndDate => FindNewestPayment(); 


        public ObservableCollection<Payment> Payments
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


        public double CalculateTotalPayments()
        {
            double total = 0.0;

            if (Payments != null)
            {
                foreach (Payment item in Payments)
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
                foreach (Payment item in Payments)
                {
                    if (item.StartDate > end)
                    {
                        end = item.StartDate;

                        if (item.EndDate > end)
                        {
                            end = item.EndDate;
                        }
                    }
                }
            }

            return end;
        }

        public DateTime FindOldestPayment()
        {
            DateTime start = new DateTime();

            if (Payments != null)
            {
                foreach (Payment item in Payments)
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
