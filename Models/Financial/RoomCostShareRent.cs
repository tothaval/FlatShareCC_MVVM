/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCostShareRent 
 * 
 *  model class for the calculation of room costs for the rent change feature
 */
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
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


        // First Year Costs
        #region First Year Costs

        public double FirstYearCompleteCostShare => CompleteCostShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearFixedCostsAdvanceShare => FixedCostsAdvanceShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearHeatingCostsAdvanceShare => HeatingCostsAdvanceShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearOtherCostsShare => OtherCostsShare * DetermineMonthsUntilYearsEnd();


        public double FirstYearRentShare => RentShare * DetermineMonthsUntilYearsEnd();

        #endregion


        // Monthly Costs
        #region Monthly Costs

        public double AreaSharedCostsShare { get; set; }


        public double CompleteCostShare { get; set; }


        public double EqualSharedCostsShare { get; set; }


        public double FixedCostsAdvanceShare { get; set; }


        public double HeatingCostsAdvanceShare { get; set; }


        public double OtherCostsShare { get; set; }


        public double PriceShare { get; set; }


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

        public ObservableCollection<FinancialTransactionItemRentViewModel> FinancialTransactionItemViewModels { get; set; } = new ObservableCollection<FinancialTransactionItemRentViewModel>();

        #endregion


        // Constructors
        #region Constructors

        public RoomCostShareRent(Room room, RentViewModel rentViewModel)
        {
            _Room = room;
            ViewModel = rentViewModel;

            if (rentViewModel.GetFlatViewModel() != null)
            {
                rentViewModel.GetFlatViewModel().PropertyChanged += RoomCostShareRent_PropertyChanged;
            }
            

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

            double heatingCosts = ((RentViewModel)ViewModel).HeatingCostsAdvance;


            // rethink and adapt code, search in list of billings
            int counter = 0;
            int alt_counter = 0;

            foreach (BillingViewModel item in ViewModel.GetFlatViewModel().AnnualBillings)
            {
                if (item.StartDate.Year == ((RentViewModel)ViewModel).StartDate.Year - 1
                    && ((RentViewModel)ViewModel).StartDate > item.BillingDate)
                {
                    double roomConsumptionPercentage =
                        item.GetRoomConsumptionPercentage(
                            _Room,
                            ((RentViewModel)ViewModel).Rent.HeatingCostsAdvance
                            );

                    HeatingCostsAdvanceShare = roomConsumptionPercentage * heatingCosts;
                    counter++;
                    break;
                }
            }

            if (counter == 0)
            {
                foreach (BillingViewModel item in ViewModel.GetFlatViewModel().AnnualBillings)
                {
                    if (item.StartDate.Year == ((RentViewModel)ViewModel).StartDate.Year - 2)
                    {
                        double roomConsumptionPercentage =
                            item.GetRoomConsumptionPercentage(
                                _Room,
                                ((RentViewModel)ViewModel).Rent.HeatingCostsAdvance
                                );

                        HeatingCostsAdvanceShare = roomConsumptionPercentage * heatingCosts;
                        alt_counter++;
                        break;
                    }
                } 
            }


            if (counter == 0 && alt_counter == 0)
            {
                HeatingCostsAdvanceShare = RentedAreaShareRatio() * heatingCosts;
            }
             

            AreaSharedCostsShare = GetAreaSharedCostsShare();

            EqualSharedCostsShare = GetEqualSharedCostShare();

            PriceShare = RentShare + FixedCostsAdvanceShare + HeatingCostsAdvanceShare;

            OtherCostsShare = AreaSharedCostsShare + EqualSharedCostsShare;

            CompleteCostShare = PriceShare + OtherCostsShare;

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

        public double EqualShareRatio()
        {

            return 1.0 / ((RentViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }

        private double DetermineMonthsUntilYearsEnd()
        {
            return ((RentViewModel)ViewModel).DetermineMonthsUntilYearsEnd();
        }

        private void FillObservableCollection()
        {
            FinancialTransactionItemViewModels.Clear();

            foreach (FinancialTransactionItemRentViewModel item in ViewModel.FinancialTransactionItemViewModels)
            {
                FinancialTransactionItemRentViewModel FTIvm = new FinancialTransactionItemRentViewModel(new FinancialTransactionItemRent());

                if (item.CostShareTypes == Enums.TransactionShareTypesRent.Equal)
                {
                    FTIvm.TransactionSum = EqualShareRatio() * item.TransactionSum;
                }
                else if (item.CostShareTypes == Enums.TransactionShareTypesRent.Area)
                {
                    FTIvm.TransactionSum = RentedAreaShareRatio() * item.TransactionSum;
                }

                FTIvm.CostShareTypes = item.CostShareTypes;

                string newItem = item.TransactionItem;
                FTIvm.TransactionItem = newItem;
                
                
                FinancialTransactionItemViewModels.Add(FTIvm);
                
            }

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        private double GetAreaSharedCostsShare()
        {
            return RentedAreaShareRatio() * ((RentViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypesRent.Area);
        }


        private double GetEqualSharedCostShare()
        {
            return ((RentViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypesRent.Equal) / ((RentViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }


        public double RentedAreaShareRatio()
        {
            return RentedAreaShare / ViewModel.GetFlatViewModel().Area;
        }

        #endregion


        // Events
        #region Events

        private void RoomCostShareRent_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RoomArea));
            OnPropertyChanged(nameof(RoomName));

            CalculateValues();
        }

        #endregion


    }
}
// EOF