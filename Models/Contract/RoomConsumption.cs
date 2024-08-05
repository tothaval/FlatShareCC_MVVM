/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomConsumption 
 * 
 *  serializable data model class
 *  saves and restores consumption values for rooms
 */

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
// EOF