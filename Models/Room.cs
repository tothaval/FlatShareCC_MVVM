/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Room 
 * 
 *  data model class
 *  for RoomViewModel
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class Room
    {

        public int ID { get; set; } = -1;  


        public string RoomName { get; set; } = string.Empty;


        public double RoomArea { get; set; } = 0.0;


        public string Signature => $"{RoomName}\n{RoomArea}m²";


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


    }
}
// EOF