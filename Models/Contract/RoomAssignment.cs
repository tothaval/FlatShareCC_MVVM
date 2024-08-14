/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomAssignment 
 * 
 *  serializable data model class
 *  saves and restores assignements of tenants and rooms
 */

namespace SharedLivingCostCalculator.Models.Contract
{
    [Serializable]
    public class RoomAssignment
    {

        // Properties & Fields
        #region Properties & Fields

        public string RoomName { get; set; }


        public string TenantName { get; set; } 

        #endregion


        // Constructors
        #region Constructors

        public RoomAssignment()
        {

        }


        public RoomAssignment(string roomName, string tenantName)
        {
            RoomName = roomName;
            TenantName = tenantName;
        } 

        #endregion

    }
}
// EOF