/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCosts 
 * 
 *  serializable data model class
 *  for RoomCostsViewModel
 */

using SharedLivingCostCalculator.ViewModels;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
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
// EOF