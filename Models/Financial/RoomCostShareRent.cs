/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCostShareRent 
 * 
 *  model class for the calculation of room costs for the rent change feature
 */
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;

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


        #region Annual Costs

        //public double AnnualCompleteCostShare => CompleteCostShare * 12;


        //public double AnnualFixedCostsAdvanceShare => FixedCostsAdvanceShare * 12;


        //public double AnnualHeatingCostsAdvanceShare => HeatingCostsAdvanceShare * 12;


        //public double AnnualOtherCostsShare => OtherCostsShare * 12;


        //public double AnnualRentShare => RentShare * 12;

        #endregion


        // First Year Costs
        #region First Year Costs

        public double FirstYearCompleteCostShare => CompleteCostShare * DetermineMonthsUntilYearsEnd();

        public double FirstYearContractCostsShare => FirstYearAdvanceShare + FirstYearRentShare;

        public double FirstYearAdvanceShare => ContractCostsAdvanceShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearOtherCostsShare => OtherCostsShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearRentShare => RentShare * DetermineMonthsUntilYearsEnd();

        #endregion


        // Monthly Costs
        #region Monthly Costs

        /// <summary>
        /// represents the sum of all monthly area share type FTI
        /// costs setup in other costs tab in rent changes view
        /// </summary>
        public double AreaSharedCostsShare { get; set; }


        public double CompleteCostShare { get; set; }


        public double CostAndCreditShare { get; set; }


        /// <summary>
        /// represents the sum of all monthly credit FTIs
        /// setup in credits tab in rent changes view
        /// </summary>
        public double CreditShare { get; set; }


        /// <summary>
        /// represents the sum of all monthly equal share type FTI
        /// costs setup in other costs tab in rent changes view
        /// </summary>
        public double EqualSharedCostsShare { get; set; }


        /// <summary>
        /// represents the sum of all monthly contract cost related advances absent rent costs
        /// </summary>
        public double ContractCostsAdvanceShare =>
            ProRataAdvanceShare
            + FixedAmountAdvanceShare;


        /// <summary>
        /// represents the sum of all monthly non pro rata contract cost advances, absent rent costs
        /// </summary>
        public double FixedAmountAdvanceShare =>
            BasicHeatingCostsAdvanceShare
            + ConsumptionHeatingCostsAdvanceShare
            + ColdWaterCostsAdvanceShare
            + WarmWaterCostsAdvanceShare;


        /// <summary>
        /// monthly heating related basic costs advance absent consumption,
        /// area shared
        /// </summary>
        public double BasicHeatingCostsAdvanceShare { get; set; }


        /// <summary>
        /// monthly cold water consumption costs advance, currently not used
        /// as a consumption share type FTI like heating, thoughts suggest
        /// to make it an option sometime in the future
        /// </summary>
        public double ColdWaterCostsAdvanceShare { get; set; }


        /// <summary>
        /// represents the monthly advance for Pro Rata costs, which
        /// are building maintainance and operation related costs
        /// that are distributed between all renting factions via
        /// area share. excluded from these costs are cold water,
        /// warm water, basic heating costs and consumption heating costs.
        /// </summary>
        public double ProRataAdvanceShare { get; set; }


        /// <summary>
        /// monthly heating related consumption costs advance
        /// </summary>
        public double ConsumptionHeatingCostsAdvanceShare { get; set; }


        /// <summary>
        /// monthly warm water consumption costs advance, currently not used
        /// as a consumption share type FTI like heating, thoughts suggest
        /// to make it an option sometime in the future
        /// </summary>
        public double WarmWaterCostsAdvanceShare { get; set; }


        /// <summary>
        /// monthly other costs advances or costs setup in the other costs tab
        /// in rent changes view, represents all consumption share types
        /// </summary>
        public double OtherCostsShare { get; set; }


        public double PriceShare { get; set; }


        /// <summary>
        /// represents the monthly rent share of the room, calculated using the
        /// area share ratio of the rented area compared to the flat area.
        /// </summary>
        public double RentShare { get; set; }

        #endregion


        // Other Properties
        #region Other Properties

        public double ConsumptionShareRatio { get; set; }


        public double RentedAreaShare { get; set; }
        
        
        public double RentedAreaShareRatio { get; set; }


        private RentViewModel _RentViewModel { get; }


        private readonly Room _Room;
        public Room Room => _Room;


        public string RoomName => _Room.RoomName;


        public double RoomArea => _Room.RoomArea;


        public string Tenant { get; set; } = "activeAssignedTenant";


        public IRoomCostsCarrier ViewModel { get; set; }

        #endregion


        // Collections
        #region Collections

        public ObservableCollection<FinancialTransactionItemRentViewModel> Credits { get; set; } = new ObservableCollection<FinancialTransactionItemRentViewModel>();


        public ObservableCollection<FinancialTransactionItemRentViewModel> FinancialTransactionItemViewModels { get; set; } = new ObservableCollection<FinancialTransactionItemRentViewModel>();

        #endregion


        // Constructors
        #region Constructors

        public RoomCostShareRent(Room room, RentViewModel rentViewModel)
        {
            _RentViewModel = rentViewModel;
            _Room = room;
            ViewModel = rentViewModel;

            RentedAreaShare = new Compute().RentedAreaShare(_Room, _RentViewModel.GetFlatViewModel());
            RentedAreaShareRatio = new Compute().RentedAreaShareRatio(_Room, _RentViewModel.GetFlatViewModel());

            if (_RentViewModel.IsInitialRent && _RentViewModel.Rent.UseRoomCosts4InitialRent)
            {
                RentShare = room.InitialColdRent;
                ProRataAdvanceShare = room.InitialAdvance;

                PriceShare = RentShare + ContractCostsAdvanceShare;

                CompleteCostShare = PriceShare + 0.0;
            }
            else
            {
                CalculateValues();
            }            
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

            RentShare = RentedAreaShareRatio * _RentViewModel.ColdRent;

            // rethink and adapt code, search in list of billings
            int counter = 0;
            int alt_counter = 0;

            foreach (BillingViewModel item in ViewModel.GetFlatViewModel().AnnualBillings)
            {
                if (item.StartDate.Year == _RentViewModel.StartDate.Year - 1
                    && (_RentViewModel.StartDate > item.BillingDate))
                {

                    RoomAdvanceShare(item);

                    counter++;
                    break;
                }
            }

            if (counter == 0)
            {
                foreach (BillingViewModel item in ViewModel.GetFlatViewModel().AnnualBillings)
                {
                    if (item.StartDate.Year == _RentViewModel.StartDate.Year - 2)
                    {

                        RoomAdvanceShare(item);

                        alt_counter++;
                        break;
                    }
                }
            }


            if (counter == 0 && alt_counter == 0)
            {
                // in case no billing was found, costs advances are split by area share
                // until the first billing has been received. if all tenants share an equal 
                // consumption behaviour towards heating, smaller areas probably need
                // less units to keep them cozy.
                // alternatively the user could use room costs for the inital rent setup
                // and use self calculated numbers or some future feature of slcc to calculate those.
                ProRataAdvanceShare = RentedAreaShareRatio * _RentViewModel.Advance;                
            }


            AreaSharedCostsShare = new Compute().GetAreaSharedCostsShare(_Room, _RentViewModel);

            EqualSharedCostsShare = new Compute().GetEqualSharedCostShare(_RentViewModel);

            PriceShare = RentShare + ContractCostsAdvanceShare;

            OtherCostsShare = AreaSharedCostsShare + EqualSharedCostsShare;

            CompleteCostShare = PriceShare + OtherCostsShare;

            FillCreditsCollection();
            FillObservableCollection();

            CostAndCreditShare = CompleteCostShare - CreditShare;
        }

        public double EqualShareRatio()
        {

            return 1.0 / ((RentViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }

        private double DetermineMonthsUntilYearsEnd()
        {
            return ((RentViewModel)ViewModel).DetermineMonthsUntilYearsEnd();
        }


        private void FillCreditsCollection()
        {
            Credits.Clear();

            foreach (FinancialTransactionItemRentViewModel item in ViewModel.Credits)
            {
                FinancialTransactionItemRentViewModel FTIvm = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

                if (item.TransactionShareTypes == Enums.TransactionShareTypesRent.Equal)
                {
                    FTIvm.TransactionSum = EqualShareRatio() * item.TransactionSum;
                }
                else if (item.TransactionShareTypes == Enums.TransactionShareTypesRent.Area)
                {
                    FTIvm.TransactionSum = RentedAreaShareRatio * item.TransactionSum;
                }

                FTIvm.TransactionShareTypes = item.TransactionShareTypes;

                string newItem = item.TransactionItem;
                FTIvm.TransactionItem = newItem;

                CreditShare += FTIvm.TransactionSum;

                Credits.Add(FTIvm);

            }

            OnPropertyChanged(nameof(Credits));
            OnPropertyChanged(nameof(CreditShare));
        }


        private void FillObservableCollection()
        {
            FinancialTransactionItemViewModels.Clear();

            foreach (FinancialTransactionItemRentViewModel item in ViewModel.FinancialTransactionItemViewModels)
            {
                FinancialTransactionItemRentViewModel FTIvm = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

                if (item.TransactionShareTypes == Enums.TransactionShareTypesRent.Equal)
                {
                    FTIvm.TransactionSum = EqualShareRatio() * item.TransactionSum;
                }
                else if (item.TransactionShareTypes == Enums.TransactionShareTypesRent.Area)
                {
                    FTIvm.TransactionSum = RentedAreaShareRatio * item.TransactionSum;
                }

                FTIvm.TransactionShareTypes = item.TransactionShareTypes;

                string newItem = item.TransactionItem;
                FTIvm.TransactionItem = newItem;


                FinancialTransactionItemViewModels.Add(FTIvm);

            }

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        public void RoomAdvanceShare(BillingViewModel billingViewModel)
        {
            Compute get = new Compute();

            ProRataAdvanceShare =
                get.CostsFactor(billingViewModel.ProRataCosts, billingViewModel.TotalCostsPerPeriod)
                * _RentViewModel.Advance
                * RentedAreaShareRatio;

            BasicHeatingCostsAdvanceShare =
                get.CostsFactor(billingViewModel.BasicHeatingCosts, billingViewModel.TotalCostsPerPeriod)
                * _RentViewModel.Advance
                * RentedAreaShareRatio;

            // water could become consumption shared via an print menu option for example,
            // the transaction share type could be changed to consumption. in order to work
            // like Heating, related code would need some work. it would be guessing in all
            // situations, where water consumption per person is not measured or measurable,
            // that is why it is set to equal for the time being.
            ColdWaterCostsAdvanceShare =
                get.CostsFactor(billingViewModel.ColdWaterCosts, billingViewModel.TotalCostsPerPeriod)
                * _RentViewModel.Advance
                * EqualShareRatio();

            // water could become consumption shared via an print menu option for example,
            // the transaction share type could be changed to consumption. in order to work
            // like Heating, related code would need some work. it would be guessing in all
            // situations, where water consumption per person is not measured or measurable,
            // that is why it is set to equal for the time being.
            WarmWaterCostsAdvanceShare =
                get.CostsFactor(billingViewModel.WarmWaterCosts, billingViewModel.TotalCostsPerPeriod)
                * _RentViewModel.Advance
                * EqualShareRatio();


            ConsumptionShareRatio =
                billingViewModel.GetRoomConsumptionRatio(
                    Room
                    );

            ConsumptionHeatingCostsAdvanceShare =
                get.CostsFactor(billingViewModel.ConsumptionHeatingCosts, billingViewModel.TotalCostsPerPeriod)
                * _RentViewModel.Advance
                * ConsumptionShareRatio;
        }

        //public double RentedAreaShareRatio()
        //{
        //    return RentedAreaShare / ViewModel.GetFlatViewModel().Area;
        //}

        #endregion


        // Events
        #region Events

        private void RoomCostShareRent_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //OnPropertyChanged(nameof(RoomArea));
            //OnPropertyChanged(nameof(RoomName));

            CalculateValues();
        }

        #endregion


    }
}
// EOF