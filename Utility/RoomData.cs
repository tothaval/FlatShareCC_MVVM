/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomData 
 * 
 *  helper class to serialize RoomViewModel
 *
 *  obsolete, Room now holds a parameterless constructor
 *  
 *  to do: remove and replace with Room model
 */
using SharedLivingCostCalculator.Models;
using System.Collections.ObjectModel;


namespace SharedLivingCostCalculator.Utility
{

    [Serializable]
    public class RoomData
    {

        public int ID { get; set; } = -1;


        public string RoomName { get; set; } = string.Empty;


        public double RoomArea { get; set; } = 0.0;


        public ObservableCollection<Payment> Payments { get; set; }


        public RoomData()
        {
            Payments = new ObservableCollection<Payment>();
        }


    }
}
// EOF