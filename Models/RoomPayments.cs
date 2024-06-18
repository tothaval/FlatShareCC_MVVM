using SharedLivingCostCalculator.ViewModels;
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
        public readonly RoomViewModel RoomViewModel;

        public int RoomID { get; set; }
            
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
