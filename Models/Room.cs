using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class Room : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int ID { get; set; } = -1;  
        public string RoomName { get; set; } = string.Empty;

        private double roomArea = 0;
        public double RoomArea { get { return roomArea; } set { roomArea = value; OnPropertyChanged(nameof(RoomArea)); } }

        public double TotalPayments
        {
            get
            {
                return CalculateTotalPayments();
            }
        }

        private ObservableCollection<Payment> payments;
        public ObservableCollection<Payment> Payments
        {
            get { return payments; }
            set { payments = value; OnPropertyChanged(nameof(Payments));

                OnPropertyChanged(nameof(TotalPayments));
            }
        }

        public string Signature => $"{RoomName}\n{RoomArea}m²";

        public Room()
        {

            payments = new ObservableCollection<Payment>();
        }

        public Room(int id) { ID = id;
            payments = new ObservableCollection<Payment>();
        }

        public Room(int iD, string roomName, double roomArea)
        {
            ID = iD;
            RoomName = roomName;
            RoomArea = roomArea;

            payments = new ObservableCollection<Payment>();
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


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
