using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class RoomPayments
    {
        [XmlIgnore]
        public RoomViewModel RoomViewModel { get; set; }


        public int RoomID { get; set; }


        [XmlArray("PaymentCollection")]
        public ObservableCollection<Payment> Payments { get; set; } = new ObservableCollection<Payment>();


        public RoomPayments() 
        {
        }


        public RoomPayments(RoomViewModel roomViewModel)
        {
            RoomViewModel = roomViewModel;
            RoomID = roomViewModel.ID;
        }


    }
}
