/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Room 
 * 
 *  data model class
 *  for RoomViewModel
 */

namespace SharedLivingCostCalculator.Models.Contract
{
    [Serializable]
    public class Room
    {

        // Properties & Fields
        #region Properties & Fields

        public double RoomArea { get; set; } = 0.0;


        public string RoomName { get; set; } = string.Empty;

        #endregion


        // Constructors
        #region Constructors

        public Room()
        {

        }


        public Room(string name)
        {
            RoomName = name;
        }


        public Room(string roomName, double roomArea)
        {
            RoomName = roomName;
            RoomArea = roomArea;
        }

        #endregion


    }
}
// EOF