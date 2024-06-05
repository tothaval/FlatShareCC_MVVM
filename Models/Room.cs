using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    public class Room
    {

        public event PropertyChangedEventHandler? PropertyChanged;

        public int ID { get; set; } = -1;  
        public string RoomName { get; set; } = string.Empty;

        public double RoomArea { get; set; } = 0.0;

        public ObservableCollection<Payment> Payments {  get; set; }

        public string Signature => $"{RoomName}\n{RoomArea}m²";

        public Room()
        {

            Payments = new ObservableCollection<Payment>();
        }

        public Room(int id) { ID = id;
            Payments = new ObservableCollection<Payment>();
        }

        public Room(int iD, string roomName, double roomArea)
        {
            ID = iD;
            RoomName = roomName;
            RoomArea = roomArea;

            Payments = new ObservableCollection<Payment>();
        }
    }
}
