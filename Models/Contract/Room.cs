/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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

        // propperties & fields
        #region propperties

        public double RoomArea { get; set; } = 0.0;


        public string RoomName { get; set; } = string.Empty;


        public string Signature => $"{RoomName}\n{RoomArea}m²";

        #endregion propperties


        // constructors
        #region constructors

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

        #endregion constructors


    }
}
// EOF