using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models.Financial
{
    /// <summary>
    /// RoomCostShareRent targets all costs that are paid in advance
    /// in regular intervals, f.e. each month.
    /// </summary>
    public class RoomCostShareRent : BaseViewModel, IRoomCostShare
    {
        /* RoomCostShareRent Rent       <- instanciated by rentviewmodel
         * 
         * RoomName
         * RoomArea
         * SharedAreaShare
         * RentedArea
         * 
         * RentShare                    <- Rent share based on RentedArea of total flat area
         * FixedCostsAdvanceShare       <- Fixed Costs share based on RentedArea of total flat area
         * HeatingCostsAdvanceShare     <- Heating Costs share (no Billing) based on RentedArea of total flat area 
         *                              <- Heating Costs share (with Billing) based on previous consumption share of consumed units
         * 
         * EqualSharedCostsShare        <- additional costs share equally shared by number of rooms/tenants
         * AreaSharedCostsShare         <- additional costs share based on RentedArea of total flat area
         * OtherCostsShare              <- EqualSharedCostsShare + AreaSharedCostsShare, sum of other costs
         * CompleteCostShare            <- RentShare + FixedCostsAdvanceShare + HeatingCostsShare + OtherCostsShare, all costs combined
         */


        // Annual Costs
        /// <summary>
        /// maybe later build in a check to calculate annual costs based on rent begin
        /// until the end of the year of the startdate property of rentviewmodel
        /// for cases where rent startdate isn't january the 1st.
        /// </summary>     
        #region Annual Costs

        public double AnnualCompleteCostShare => CompleteCostShare * 12;


        public double AnnualFixedCostsAdvanceShare => FixedCostsAdvanceShare * 12;


        public double AnnualHeatingCostsAdvanceShare => HeatingCostsAdvanceShare * 12;


        public double AnnualOtherCostsShare => OtherCostsShare * 12;


        public double AnnualRentShare => RentShare * 12;

        #endregion


        // Monthly Costs
        #region Monthly Costs

        public double AreaSharedCostsShare { get; set; }


        public double CompleteCostShare { get; set; }


        public double EqualSharedCostsShare { get; set; }


        public double FixedCostsAdvanceShare { get; set; }


        public double HeatingCostsAdvanceShare { get; set; }


        public double OtherCostsShare { get; set; }

        public double RentShare { get; set; }

        #endregion


        // Other Properties
        #region Other Properties

        public double RentedAreaShare { get; set; }


        private readonly Room _Room;


        public string RoomName => _Room.RoomName;


        public double RoomArea => _Room.RoomArea;


        public double SharedAreaShare { get; set; }


        public IRoomCostsCarrier ViewModel { get; set; }

        #endregion


        // Collections
        #region Collections

        public ObservableCollection<FinancialTransactionItemViewModel> FinancialTransactionItemViewModels { get; set; } = new ObservableCollection<FinancialTransactionItemViewModel>();

        #endregion


        // Constructors
        #region Constructors

        public RoomCostShareRent(Room room, RentViewModel rentViewModel)
        {
            _Room = room;
            ViewModel = rentViewModel;

            CalculateValues();
        }

        #endregion


        // Methods
        #region Methods


        /// <summary>
        /// refactor later into smaller methods, one per value calculation
        /// 
        /// calculates all properties
        /// </summary>
        public void CalculateValues()
        {
            SharedAreaShare = ViewModel.GetFlatViewModel().SharedArea / ViewModel.GetFlatViewModel().RoomCount;

            RentedAreaShare = SharedAreaShare + RoomArea;

            RentShare = RentedAreaShareRatio() * ((RentViewModel)ViewModel).ColdRent;

            FixedCostsAdvanceShare = RentedAreaShareRatio() * ((RentViewModel)ViewModel).FixedCostsAdvance;

            if (((RentViewModel)ViewModel).HasBilling)
            {
                BillingViewModel billingViewModel = ((RentViewModel)ViewModel).BillingViewModel;

                double roomConsumptionPercentage =
                    billingViewModel.GetRoomConsumptionPercentage(
                        _Room,
                        ((RentViewModel)ViewModel).Rent.HeatingCostsAdvance
                        );

                HeatingCostsAdvanceShare = roomConsumptionPercentage * ((RentViewModel)ViewModel).HeatingCostsAdvance;
            }
            else
            {
                HeatingCostsAdvanceShare = RentedAreaShareRatio() * ((RentViewModel)ViewModel).HeatingCostsAdvance;
            }

            AreaSharedCostsShare = GetAreaSharedCostsShare();

            EqualSharedCostsShare = GetEqualSharedCostShare();

            OtherCostsShare = AreaSharedCostsShare + EqualSharedCostsShare;

            CompleteCostShare = RentShare + FixedCostsAdvanceShare + HeatingCostsAdvanceShare + OtherCostsShare;

            FillObservableCollection();

            OnPropertyChanged(nameof(SharedAreaShare));
            OnPropertyChanged(nameof(RentedAreaShare));
            OnPropertyChanged(nameof(RentShare));
            OnPropertyChanged(nameof(FixedCostsAdvanceShare));
            OnPropertyChanged(nameof(HeatingCostsAdvanceShare));

            OnPropertyChanged(nameof(OtherCostsShare));
            OnPropertyChanged(nameof(CompleteCostShare));

            OnPropertyChanged(nameof(AnnualFixedCostsAdvanceShare));
            OnPropertyChanged(nameof(AnnualHeatingCostsAdvanceShare));
            OnPropertyChanged(nameof(AnnualOtherCostsShare));
            OnPropertyChanged(nameof(AnnualRentShare));

            OnPropertyChanged(nameof(AnnualCompleteCostShare));
        }

        private double EqualShareRatio()
        {

            return 1.0 / ((RentViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }

        private void FillObservableCollection()
        {
            foreach (FinancialTransactionItemViewModel item in ViewModel.FinancialTransactionItemViewModels)
            {
                FinancialTransactionItemViewModel FTIvm = new FinancialTransactionItemViewModel(new FinancialTransactionItem());

                if (item.CostShareTypes == Enums.TransactionShareTypes.Equal)
                {
                    FTIvm.Cost = EqualShareRatio() * item.Cost;
                }
                else if (item.CostShareTypes == Enums.TransactionShareTypes.Area)
                {
                    FTIvm.Cost = RentedAreaShareRatio() * item.Cost;
                }

                FTIvm.CostShareTypes = item.CostShareTypes;

                string newItem = item.Item;
                FTIvm.Item = newItem;

                if (item.CostShareTypes != Enums.TransactionShareTypes.Consumption)
                {
                    FinancialTransactionItemViewModels.Add(FTIvm);
                }
            }

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        private double GetAreaSharedCostsShare()
        {
            return RentedAreaShareRatio() * ((RentViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypes.Area);
        }


        private double GetEqualSharedCostShare()
        {
            return ((RentViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypes.Equal) / ((RentViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }


        private double RentedAreaShareRatio()
        {
            return RentedAreaShare / ViewModel.GetFlatViewModel().Area;
        } 

        #endregion

    }
}
