/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Room 
 * 
 *  data model class
 *  for RoomViewModel
 */

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class Room
    {

        // propperties & fields
        #region propperties

        public int ID { get; set; } = -1;

        
        public double RoomArea { get; set; } = 0.0;


        public string RoomName { get; set; } = string.Empty;

        
        public string Signature => $"{RoomName}\n{RoomArea}m²";

        #endregion propperties


        // constructors
        #region constructors

        public Room()
        {

        }


        public Room(int id)
        {
            ID = id;
        }


        public Room(int iD, string roomName, double roomArea)
        {
            ID = iD;
            RoomName = roomName;
            RoomArea = roomArea;
        }

        #endregion constructors


    }
}
// EOF