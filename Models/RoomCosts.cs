/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCosts 
 * 
 *  serializable data model class
 *  for RoomCostsViewModel
 */

using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class RoomCosts
    {

        // properties & fields
        #region properties

        public double FixedShare { get; set; }


        [XmlIgnore]
        public RoomViewModel GetRoomViewModel { get; }


        public double HeatingShare { get; set; }


        public double HeatingUnitsConsumption { get; set; }


        public double RentShare { get; set; }


        public int RoomID { get; set; }

        #endregion properties


        // constructors
        #region constructors

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

        #endregion constructors

    }
}
// EOF