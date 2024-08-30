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

        public double FirstYearAdvanceShare => AdvanceShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearOtherCostsShare => OtherCostsShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearRentShare => RentShare * DetermineMonthsUntilYearsEnd();

        #endregion


        // Monthly Costs
        #region Monthly Costs

        public double AreaSharedCostsShare { get; set; }


        public double CompleteCostShare { get; set; }


        public double CostAndCreditShare { get; set; }


        public double CreditShare { get; set; }


        public double EqualSharedCostsShare { get; set; }


        public double AdvanceShare { get; set; }


        public double OtherCostsShare { get; set; }


        public double PriceShare { get; set; }


        public double RentShare { get; set; }

        #endregion


        // Other Properties
        #region Other Properties

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
                AdvanceShare = room.InitialAdvance;

                PriceShare = RentShare + AdvanceShare;

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

                    AdvanceShare = new Compute().RoomAdvanceShare(item, _RentViewModel, _Room);

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

                        AdvanceShare = new Compute().RoomAdvanceShare(item, _RentViewModel, _Room);

                        alt_counter++;
                        break;
                    }
                }
            }


            if (counter == 0 && alt_counter == 0)
            {
                AdvanceShare = RentedAreaShareRatio * _RentViewModel.Advance;
            }


            AreaSharedCostsShare = new Compute().GetAreaSharedCostsShare(_Room, _RentViewModel);

            EqualSharedCostsShare = new Compute().GetEqualSharedCostShare(_RentViewModel);

            PriceShare = RentShare + AdvanceShare;

            OtherCostsShare = AreaSharedCostsShare + EqualSharedCostsShare;

            CompleteCostShare = PriceShare + OtherCostsShare;

            FillCreditsCollection();
            FillObservableCollection();

            CostAndCreditShare = CompleteCostShare - CreditShare;

            //OnPropertyChanged(nameof(RentedAreaShare));
            //OnPropertyChanged(nameof(RentShare));
            //OnPropertyChanged(nameof(AdvanceShare));

            //OnPropertyChanged(nameof(OtherCostsShare));
            //OnPropertyChanged(nameof(CompleteCostShare));

            //OnPropertyChanged(nameof(AnnualFixedCostsAdvanceShare));
            //OnPropertyChanged(nameof(AnnualHeatingCostsAdvanceShare));
            //OnPropertyChanged(nameof(AnnualOtherCostsShare));
            //OnPropertyChanged(nameof(AnnualRentShare));

            //OnPropertyChanged(nameof(AnnualCompleteCostShare));
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