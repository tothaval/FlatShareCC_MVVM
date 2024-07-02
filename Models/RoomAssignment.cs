using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class RoomAssignment
    {
        public string RoomName { get; set; }
        public string TenantName { get; set; }

        public RoomAssignment()
        {
                
        }


        public RoomAssignment(string roomName, string tenantName)
        {
            RoomName = roomName;
            TenantName = tenantName;                
        }

    }
}
