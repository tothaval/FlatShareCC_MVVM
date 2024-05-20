using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    class RoomHeatingUnitsConsumption
    {
        public Room Room {  get; }
        public double HeatingUnitsConsumption { get; set; }

        public RoomHeatingUnitsConsumption(Room room)
        {
                Room = room;
        }
    }
}
