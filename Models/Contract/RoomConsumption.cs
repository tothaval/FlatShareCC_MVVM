using SharedLivingCostCalculator.Models.Financial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models.Contract
{

    [Serializable]
    public class RoomConsumption
    {
    
        public Room Room { get; set; } = new Room();
        public double ConsumptionValue { get; set; } = 0.0;


        public RoomConsumption()
        {
                
        }

        public RoomConsumption(Room room, double consumptionValue)
        {
            Room = room;
            ConsumptionValue = consumptionValue;
        }
    }
}
