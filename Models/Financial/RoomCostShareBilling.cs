/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCostShareBilling 
 * 
 *  model class for the calculation of room costs for the annual billing feature
 */
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class RoomCostShareBilling : BaseViewModel, IRoomCostShare
    {

        // Properties & Fields
        #region Properties & Fields

        public double Advances => DetermineAdvances();


        public double AreaSharedCostsShare { get; set; }


        public double Balance => Advances - TotalCostsAnnualCostsShare;


        public double BasicHeatingCostsShare { get; set; }


        public double ColdWaterCostsShare { get; set; }


        public double ConsumptionHeatingCostsShare { get; set; }


        public double ConsumptionSharedCostsShare { get; set; }

                
        public double CreditShare { get; set; }


        public double EqualSharedCostsShare { get; set; }


        public double ExtraCostsAnnualShare => ProRataAmounts + FixedAmountShare;


        public double ProRataAmounts { get; set; }


        private double _FlatArea { get; set; } = 0.0;


        public double FixedAmountShare { get; set; }


        public double HeatingUnitsAnnualConsumptionShare { get; set; }


        public double HeatingUnitsSharedConsumptionShare { get; set; }


        public double HeatingUnitsTotalConsumptionShare { get; set; }


        public double HeatingUnitsTotalConsumptionSharePercentage { get; set; }


        public double HeatingUnitsTotalConsumptionShareRatio { get; set; }


        public double OtherCostsAnnualCostsShare { get; set; }


        public double RentCostsAnnualShare { get; set; }


        public double RentedAreaShare { get; set; }


        private readonly Room _Room;
        public Room Room => _Room;


        private int _RoomCount { get; set; } = 1;


        public string RoomName => _Room.RoomName;


        public double RoomArea => _Room.RoomArea;


        public double SharedAreaShare { get; set; }


        private double _SharedFlatArea { get; set; } = 0.0;


        public string Tenant { get; set; } = "activeAssignedTenant";


        public double TotalContractCostsShare { get; set; }


        public double TotalCostsAnnualCostsShareNoRent { get; set; }


        public double TotalCostsAnnualCostsShare { get; set; }


        public IRoomCostsCarrier ViewModel { get; set; }


        public double WarmWaterCostsShare { get; set; }

        #endregion


        // Collections
        #region Collections

        public ObservableCollection<RoomConsumptionViewModel> ConsumptionItemViewModels { get; set; } = new ObservableCollection<RoomConsumptionViewModel>();


        public ObservableCollection<FinancialTransactionItemRentViewModel> Credits { get; set; } = new ObservableCollection<FinancialTransactionItemRentViewModel>();


        public ObservableCollection<FinancialTransactionItemBillingViewModel> FinancialTransactionItemViewModels { get; set; } = new ObservableCollection<FinancialTransactionItemBillingViewModel>();

        #endregion


        public RoomCostShareBilling(Room room, BillingViewModel billingViewModel)
        {
            _Room = room;
            ViewModel = billingViewModel;


            if (billingViewModel.GetFlatViewModel() != null)
            {
                billingViewModel.GetFlatViewModel().PropertyChanged += RoomCostShareBilling_PropertyChanged; ;

                _RoomCount = ViewModel.GetFlatViewModel().RoomCount;
                _FlatArea = ViewModel.GetFlatViewModel().Area;
                _SharedFlatArea = ViewModel.GetFlatViewModel().SharedArea;

                //foreach (TenantConfigurationViewModel item in billingViewModel.GetFlatViewModel().TenantConfigurations)
                //{
                //    if (item.Start > billingViewModel.EndDate)
                //    {
                //        continue;
                //    }

                //    if (item.Start.Year == billingViewModel.Year)
                //    {

                //    }

                //    if (item.Start.Year == billingViewModel.StartDate.Year - 1)
                //    {
                //        foreach (RoomAssignmentViewModel roomAssignment in item.RoomAssignements)
                //        {
                //            if (roomAssignment.RoomViewModel.Room.Equals(room))
                //            {
                //                Tenant = roomAssignment.AssignedTenant.Name;
                //            }
                //        }                        
                //    }
                //}
            }


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
             * ConsumptionSharedCostsShare  <- additional costs share based on consumption share of consumed units
             * 
             * TotalCostsAnnualCostsShare   <- RentCostsAnnualShare + FixedCostsAnnualCostsShare + HeatingCostsAnnualCostsShare + OtherCostsAnnualCostsShare
             * 
             * Advances                     <- all advances within the period
             * 
             * Balance                      <- TotalCostsAnnualCostsShare - Advance
             * 
             */
        }

        public double CalculateBasicHeatingCostsShare()
        {
            BillingViewModel billingViewModel = (BillingViewModel)ViewModel;

            double basicHeatingCostsPercentage = billingViewModel.BasicHeatingCostsPercentage / 100;

            double basicHeatingCostsShare = basicHeatingCostsPercentage * billingViewModel.TotalHeatingCostsPerPeriod;

            basicHeatingCostsShare = CalculateEqualCostShare(basicHeatingCostsShare);

            return basicHeatingCostsShare;
        }

        public double CalculateConsumptionHeatingCostsShare()
        {
            BillingViewModel billingViewModel = (BillingViewModel)ViewModel;

            double fixedAmountPercentage = billingViewModel.ConsumptionHeatingCostsPercentage / 100;

            double fixedAmountShare = fixedAmountPercentage * billingViewModel.TotalHeatingCostsPerPeriod * HeatingUnitsTotalConsumptionShareRatio;

            return fixedAmountShare;
        }


        public double CalculateEqualCostShare(double totalCosts)
        {
            return totalCosts / _RoomCount;
        }


        public void CalculateValues()
        {
            SharedAreaShare = _SharedFlatArea / _RoomCount;

            RentedAreaShare = SharedAreaShare + RoomArea;

            DetermineHeatingUnitsAnnualConsumptionShares();

            RentCostsAnnualShare = DetermineRentCostsForPaymentOption();
            ProRataAmounts = RentedAreaShareRatio() * ((BillingViewModel)ViewModel).TotalFixedCostsPerPeriod;

            BasicHeatingCostsShare = CalculateBasicHeatingCostsShare();
            ColdWaterCostsShare = CalculateEqualCostShare(((BillingViewModel)ViewModel).ColdWaterCosts);
            WarmWaterCostsShare = CalculateEqualCostShare(((BillingViewModel)ViewModel).WarmWaterCosts);
            ColdWaterCostsShare = CalculateEqualCostShare(((BillingViewModel)ViewModel).ColdWaterCosts);

            ConsumptionHeatingCostsShare = CalculateConsumptionHeatingCostsShare();

            FixedAmountShare = ColdWaterCostsShare + WarmWaterCostsShare + BasicHeatingCostsShare + ConsumptionHeatingCostsShare;

            AreaSharedCostsShare = GetAreaSharedCostsShare();

            EqualSharedCostsShare = GetEqualSharedCostShare();

            ConsumptionSharedCostsShare = 0.0;

            DetermineAdvances();

            FillConsumptionItemViewModels();

            FillCreditsCollection();

            FillOtherCostsCollection();

            OtherCostsAnnualCostsShare = AreaSharedCostsShare + EqualSharedCostsShare + ConsumptionSharedCostsShare;

            TotalContractCostsShare = RentCostsAnnualShare + ProRataAmounts + FixedAmountShare;

            TotalCostsAnnualCostsShare = RentCostsAnnualShare + ProRataAmounts + FixedAmountShare + OtherCostsAnnualCostsShare - CreditShare;

            TotalCostsAnnualCostsShareNoRent = ExtraCostsAnnualShare + OtherCostsAnnualCostsShare - CreditShare;

            OnPropertyChanged(nameof(SharedAreaShare));
            OnPropertyChanged(nameof(RentedAreaShare));

            OnPropertyChanged(nameof(HeatingUnitsAnnualConsumptionShare));

            OnPropertyChanged(nameof(HeatingUnitsSharedConsumptionShare));

            OnPropertyChanged(nameof(HeatingUnitsTotalConsumptionShare));

            OnPropertyChanged(nameof(HeatingUnitsTotalConsumptionSharePercentage));

            OnPropertyChanged(nameof(ExtraCostsAnnualShare));

            OnPropertyChanged(nameof(OtherCostsAnnualCostsShare));
            OnPropertyChanged(nameof(TotalCostsAnnualCostsShare));
            OnPropertyChanged(nameof(TotalContractCostsShare));

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));

        }



        private double DetermineRentCostsForPaymentOption()
        {
            double rentCosts = 0.0;

            if (((BillingViewModel)ViewModel).HasPayments)
            {
                // sort List by StartDate, ascending
                ObservableCollection<RentViewModel> RentList = ((BillingViewModel)ViewModel).FindRelevantRentViewModels();


                for (int i = 0; i < RentList.Count; i++)
                {
                    double months = ((BillingViewModel)ViewModel).DeterminePaymentMonths(RentList, i);

                    rentCosts += RentList[i].ColdRent * months * RentedAreaShareRatio();
                }

            }

            return rentCosts;
        }


        /// <summary>
        /// obsolete, calculation will use actual advances paid to calculate values
        /// </summary>
        /// <returns></returns>
        //private double DetermineNoPaymentOptionAdvances()
        //{
        //    double advance = 0.0;

        //    // sort List by StartDate, ascending
        //    ObservableCollection<RentViewModel> RentList = ((BillingViewModel)ViewModel).FindRelevantRentViewModels();

        //    DateTime start = DateTime.Now;
        //    DateTime end = DateTime.Now;

        //    for (int i = 0; i < RentList.Count; i++)
        //    {
        //        double months = ((BillingViewModel)ViewModel).DeterminePaymentMonths(RentList, start, end, i);

        //        if (RentList[i] != null)
        //        {

        //            foreach (RoomCostShareRent item in RentList[i].RoomCostShares)
        //            {
        //                if (item.RoomArea == RoomArea && item.RoomName.Equals(RoomName))
        //                {
        //                    advance += item.AdvanceShare * months;
        //                    break;
        //                }
        //            }
        //        }
        //    }


        //    return advance;
        //}


        private double DetermineAdvances()
        {

            BillingViewModel billingViewModel = ((BillingViewModel)ViewModel);

            double advance = 0.0;

            if (ViewModel != null)
            {
                if (billingViewModel.HasPayments)
                {
                    advance += GetRoomPaymentsPerPeriod(_Room);
                }
                else
                {

                    ObservableCollection<RentViewModel> RentList = ((BillingViewModel)ViewModel).FindRelevantRentViewModels();

                    for (int i = 0; i < RentList.Count; i++)
                    {
                        double months = ((BillingViewModel)ViewModel).DeterminePaymentMonths(RentList, i);

                        RentList[i].RecalculateRoomCosts();

                        Task.Delay(10);

                        if (RentList[i] != null && RentList[i].RoomCostShares.Count > 0)
                        {
                            foreach (RoomCostShareRent item in RentList[i].RoomCostShares)
                            {
                                if (item.RoomArea == RoomArea && item.RoomName.Equals(RoomName))
                                {
                                    advance += item.AdvanceShare * months;

                                    break;
                                }
                            }
                        }

                        else
                        {

                            advance += RentedAreaShareRatio() * RentList[i].Advance * months;
                        }
                    }
                }
            }

            return advance;
        }


        private void DetermineHeatingUnitsAnnualConsumptionShares()
        {
            FinancialTransactionItemBilling financialTransactionItem = ((BillingViewModel)ViewModel).Billing.TotalHeatingCostsPerPeriod;

            foreach (ConsumptionItemViewModel consumptionItemViewModel in ((BillingViewModel)ViewModel).ConsumptionItemViewModels)
            {
                if (consumptionItemViewModel.ConsumptionCause.Equals(financialTransactionItem.TransactionItem))
                {
                    foreach (RoomConsumptionViewModel item in consumptionItemViewModel.RoomConsumptionViewModels)
                    {
                        if (item.RoomName.Equals(RoomName) && item.RoomArea == RoomArea)
                        {
                            HeatingUnitsAnnualConsumptionShare = item.ConsumptionValue;

                            HeatingUnitsSharedConsumptionShare = consumptionItemViewModel.RoomSharedConsumption;

                            HeatingUnitsTotalConsumptionShare = HeatingUnitsAnnualConsumptionShare + HeatingUnitsSharedConsumptionShare;

                            HeatingUnitsTotalConsumptionSharePercentage = DetermineTotalConsumptionSharePercentage(HeatingUnitsTotalConsumptionShare, consumptionItemViewModel);

                            HeatingUnitsTotalConsumptionShareRatio = DetermineTotalConsumptionShareRatio(HeatingUnitsTotalConsumptionShare, consumptionItemViewModel);

                            break;
                        }
                    }
                }
            }
        }


        public double DetermineTotalConsumptionSharePercentage(double totalShare, ConsumptionItemViewModel consumptionItemViewModel)
        {
            return totalShare / consumptionItemViewModel.ConsumedUnits * 100;
        }


        public double DetermineTotalConsumptionShareRatio(double totalShare, ConsumptionItemViewModel consumptionItemViewModel)
        {
            return totalShare / consumptionItemViewModel.ConsumedUnits;
        }


        /// <summary>
        /// calculates the factor of 1 against the total amount of rooms
        /// </summary>
        /// <returns>1.0 / _RoomCount</returns>
        private double EqualShareRatio()
        {
            return 1.0 / _RoomCount;
        }



        private void FillConsumptionItemViewModels()
        {
            if (ViewModel != null)
            {
                ConsumptionItemViewModels.Clear();

                foreach (ConsumptionItemViewModel item in ((BillingViewModel)ViewModel).ConsumptionItemViewModels)
                {
                    foreach (RoomConsumptionViewModel roomConsumptionViewModel in item.RoomConsumptionViewModels)
                    {
                        if (roomConsumptionViewModel.RoomArea == RoomArea && roomConsumptionViewModel.RoomName.Equals(RoomName)
                            && !roomConsumptionViewModel.ConsumptionCause.Equals(((BillingViewModel)ViewModel).Billing.TotalHeatingCostsPerPeriod.TransactionItem)
                            )
                        {
                            double totalshare = roomConsumptionViewModel.ConsumptionValue + item.RoomSharedConsumption;

                            roomConsumptionViewModel.ConsumptionCost = DetermineTotalConsumptionShareRatio(totalshare, item) * item.ConsumptionItem.ConsumptionCause.TransactionSum;

                            ConsumptionItemViewModels.Add(roomConsumptionViewModel);
                            break;
                        }
                    }
                }

            }

            OnPropertyChanged(nameof(ConsumptionItemViewModels));
        }


        private void FillCreditsCollection()
        {
            CreditShare = 0.0;

            if (ViewModel != null)
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
                        FTIvm.TransactionSum = RentedAreaShareRatio() * item.TransactionSum;
                    }

                    FTIvm.TransactionShareTypes = item.TransactionShareTypes;

                    string newItem = item.TransactionItem;
                    FTIvm.TransactionItem = newItem;

                    CreditShare += FTIvm.TransactionSum;

                    Credits.Add(FTIvm);
                }

            }

            OnPropertyChanged(nameof(CreditShare));

            OnPropertyChanged(nameof(TotalCostsAnnualCostsShare));

            OnPropertyChanged(nameof(Balance));

            OnPropertyChanged(nameof(Credits));
        }


        private void FillOtherCostsCollection()
        {
            ConsumptionSharedCostsShare = 0.0;

            if (ViewModel != null)
            {
                FinancialTransactionItemViewModels.Clear();

                foreach (FinancialTransactionItemBillingViewModel item in ViewModel.FinancialTransactionItemViewModels)
                {
                    FinancialTransactionItemBillingViewModel FTIvm = new FinancialTransactionItemBillingViewModel(new FinancialTransactionItemBilling());

                    if (item.TransactionShareTypes == Enums.TransactionShareTypesBilling.Equal)
                    {
                        FTIvm.TransactionSum = EqualShareRatio() * item.TransactionSum;
                    }
                    else if (item.TransactionShareTypes == Enums.TransactionShareTypesBilling.Area)
                    {
                        FTIvm.TransactionSum = RentedAreaShareRatio() * item.TransactionSum;
                    }
                    else if (item.TransactionShareTypes == Enums.TransactionShareTypesBilling.Consumption)
                    {
                        foreach (RoomConsumptionViewModel roomConsumption in ConsumptionItemViewModels)
                        {
                            if (roomConsumption.RoomName.Equals(RoomName) && roomConsumption.RoomArea == RoomArea
                                && roomConsumption.ConsumptionCause.Equals(item.TransactionItem)
                                && !roomConsumption.ConsumptionCause.Equals(((BillingViewModel)ViewModel).Billing.TotalHeatingCostsPerPeriod.TransactionItem))
                            {
                                FTIvm.TransactionSum = roomConsumption.ConsumptionCost;

                                ConsumptionSharedCostsShare += FTIvm.TransactionSum;
                                break;
                            }
                        }
                    }

                    FTIvm.TransactionShareTypes = item.TransactionShareTypes;

                    string newItem = item.TransactionItem;
                    FTIvm.TransactionItem = newItem;

                    FinancialTransactionItemViewModels.Add(FTIvm);
                }

            }

            OnPropertyChanged(nameof(ConsumptionSharedCostsShare));

            OnPropertyChanged(nameof(OtherCostsAnnualCostsShare));
            OnPropertyChanged(nameof(TotalCostsAnnualCostsShare));

            OnPropertyChanged(nameof(Balance));

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        private double GetAreaSharedCostsShare()
        {
            return RentedAreaShareRatio() * ((BillingViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypesBilling.Area);
        }


        private double GetEqualSharedCostShare()
        {
            return ((BillingViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypesBilling.Equal) / _RoomCount;
        }


        private double GetRoomPaymentsPerPeriod(Room room)
        {
            double paymentsPerPeriod = 0.0;

            foreach (RoomPaymentsViewModel roomPaymentsViewModel in ((BillingViewModel)ViewModel).RoomPayments)
            {
                if (roomPaymentsViewModel.RoomName.Equals(room.RoomName) && roomPaymentsViewModel.RoomArea == room.RoomArea)
                {
                    foreach (Payment payment in roomPaymentsViewModel.RoomPayments.Payments)
                    {
                        if (payment.StartDate >= ((BillingViewModel)ViewModel).StartDate
                            && payment.StartDate <= ((BillingViewModel)ViewModel).EndDate
                            && payment.EndDate >= ((BillingViewModel)ViewModel).StartDate
                            && payment.EndDate <= ((BillingViewModel)ViewModel).EndDate
                            )
                        {
                            paymentsPerPeriod += payment.PaymentTotal;
                        }
                    }
                }


            }

            return paymentsPerPeriod;
        }


        /// <summary>
        /// calculates the factor of rented area, which is room area 
        /// and the equal shared shared area of the remaining flat,
        /// against the total rented area
        /// </summary>
        /// <returns>RentedAreaShare / _FlatArea</returns>
        private double RentedAreaShareRatio()
        {
            return RentedAreaShare / _FlatArea;
        }


        private void RoomCostShareBilling_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RoomArea));
            OnPropertyChanged(nameof(RoomName));

            CalculateValues();
        }


    }
}
// EOF