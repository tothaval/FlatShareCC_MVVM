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

        // Properties & Fields
        #region Properties & Fields

        public double ConsumptionValue { get; set; } = 0.0;


        public Room Room { get; set; } = new Room();

        #endregion


        // Constructors
        #region Constructors

        public RoomConsumption()
        {

        }

        public RoomConsumption(Room room, double consumptionValue)
        {
            Room = room;
            ConsumptionValue = consumptionValue;
        } 

        #endregion


    }
}
// EOF