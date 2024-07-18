using SharedLivingCostCalculator.Interfaces.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Interfaces.Contract
{
    public interface IRoomCostShare
    {

        public string RoomName { get; }
        public double RoomArea { get; }
        public double SharedAreaShare { get; set; }
        public double RentedAreaShare { get; set; }

        public IRoomCostsCarrier ViewModel { get; set; }

        public void CalculateValues();

    }
}
