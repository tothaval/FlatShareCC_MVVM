using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Calculations
{
    [Serializable]
    public class RoomCosts
    {
        [XmlIgnore]
        public RoomViewModel GetRoomViewModel { get; }
        
        public int RoomID { get; set; }
        
        public double HeatingUnitsConsumption { get; set; }

        public double RentShare { get; set; }

        public double FixedShare { get; set; }

        public double HeatingShare { get; set; }

        public RoomCosts()
        {
                
        }

        public RoomCosts(RoomViewModel room)
        {
            GetRoomViewModel = room;

            if (room != null)
            {
                RoomID = room.ID;
            }
            
        }
    }
}
