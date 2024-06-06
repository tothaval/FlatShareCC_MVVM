using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    //[Serializable]
    public class Room
    {
        public int ID { get; set; } = -1;  
        public string RoomName { get; set; } = string.Empty;

        public double RoomArea { get; set; } = 0.0;

        public ObservableCollection<PaymentViewModel> Payments {  get; set; }

        public string Signature => $"{RoomName}\n{RoomArea}m²";

        public Room()
        {

            Payments = new ObservableCollection<PaymentViewModel>();
        }

        public Room(int id) { ID = id;
            Payments = new ObservableCollection<PaymentViewModel>();
        }

        public Room(int iD, string roomName, double roomArea)
        {
            ID = iD;
            RoomName = roomName;
            RoomArea = roomArea;

            Payments = new ObservableCollection<PaymentViewModel>();
        }
    }
}
