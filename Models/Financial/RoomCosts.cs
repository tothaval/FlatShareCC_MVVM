/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCosts 
 * 
 *  serializable data model class
 *  for RoomCostsViewModel
 */

using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.Models.Financial
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


        public string RoomName { get; set; }

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
                RoomName = room.RoomName;
            }

        }

        #endregion constructors

    }
}
// EOF