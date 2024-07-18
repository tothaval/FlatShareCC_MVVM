using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class RoomCostShareBilling : IRoomCostShare
    {
        private readonly Room _Room;


        public string RoomName => _Room.RoomName;

        public double RoomArea => _Room.RoomArea;

        public double SharedAreaShare { get; set; }
        public double RentedAreaShare { get; set; }


        public IRoomCostsCarrier ViewModel { get; set; }


        public RoomCostShareBilling(Room room, BillingViewModel billingViewModel)
        {
            _Room = room;
            ViewModel = billingViewModel;

            CalculateValues();

            /* RoomCostShareRent Billing    <- instanciated by billingviewmodel
             * 
             * RoomName                     <- base class or interface?
             * RoomArea                     <- base class or interface?
             * SharedAreaShare              <- base class or interface?
             * RentedArea                   <- base class or interface?
             * 
             * RentCostsAnnualShare         <- Rent share based on RentedArea of total flat area
             * FixedCostsAnnualCostsShare   <- Fixed Costs share based on RentedArea of total flat area
             * HeatingCostsAnnualCostsShare <- Heating Costs share based on consumption share of consumed units
             * 
             * EqualSharedCostsShare        <- additional costs share equally shared by number of rooms/tenants
             * AreaSharedCostsShare         <- additional costs share based on RentedArea of total flat area
             * ConsumptionSharedCosstsShare <- additional costs share based on consumption share of consumed units
             * 
             */


        }
        public void CalculateValues()
        {
            SharedAreaShare = ViewModel.GetFlatViewModel().SharedArea / ViewModel.GetFlatViewModel().RoomCount;

            RentedAreaShare = SharedAreaShare + RoomArea;
        }
    }
}
