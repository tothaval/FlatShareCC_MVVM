using Microsoft.VisualBasic;
using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
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
    public class RoomCostShareBilling : BaseViewModel, IRoomCostShare
    {
        private readonly Room _Room;

        public double Advances { get; set; }

        public double Balance => Advances - TotalCostsAnnualCostsShare;

        public double AreaSharedCostsShare { get; set; }
        public double ConsumptionSharedCostsShare { get; set; }
        public double EqualSharedCostsShare { get; set; }


        public string RoomName => _Room.RoomName;

        public double RoomArea => _Room.RoomArea;

        public double SharedAreaShare { get; set; }
        public double RentedAreaShare { get; set; }

        public double RentCostsAnnualShare { get; set; }
        public double FixedCostsAnnualCostsShare { get; set; }
        public double HeatingCostsAnnualCostsShare { get; set; }

        public double OtherCostsAnnualCostsShare { get; set; }
        public double TotalCostsAnnualCostsShare { get; set; }

        public double HeatingUnitsAnnualConsumptionShare { get; set; }
        public double HeatingUnitsSharedConsumptionShare { get; set; }
        public double HeatingUnitsTotalConsumptionShare { get; set; }
        public double HeatingUnitsTotalConsumptionSharePercentage { get; set; }
        public double HeatingUnitsTotalConsumptionShareRatio { get; set; }
        

        public IRoomCostsCarrier ViewModel { get; set; }


        // Collections
        #region Collections

        public ObservableCollection<FinancialTransactionItemViewModel> FinancialTransactionItemViewModels { get; set; } = new ObservableCollection<FinancialTransactionItemViewModel>();

        #endregion


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


        public void CalculateValues()
        {
            SharedAreaShare = ViewModel.GetFlatViewModel().SharedArea / ViewModel.GetFlatViewModel().RoomCount;

            RentedAreaShare = SharedAreaShare + RoomArea;

            DetermineHeatingUnitsAnnualConsumptionShares();

            RentCostsAnnualShare = 0.0;
            FixedCostsAnnualCostsShare = RentedAreaShareRatio() * ((BillingViewModel)ViewModel).TotalFixedCostsPerPeriod;            
            HeatingCostsAnnualCostsShare = HeatingUnitsTotalConsumptionShareRatio * ((BillingViewModel)ViewModel).TotalHeatingCostsPerPeriod;

            AreaSharedCostsShare = GetAreaSharedCostsShare();

            EqualSharedCostsShare = GetEqualSharedCostShare();

            ConsumptionSharedCostsShare = 0.0;

            DetermineAdvances();

            FillObservableCollection();

            OnPropertyChanged(nameof(SharedAreaShare));
            OnPropertyChanged(nameof(RentedAreaShare));

            OnPropertyChanged(nameof(HeatingUnitsAnnualConsumptionShare));

            OnPropertyChanged(nameof(HeatingUnitsSharedConsumptionShare));

            OnPropertyChanged(nameof(HeatingUnitsTotalConsumptionShare));

            OnPropertyChanged(nameof(HeatingUnitsTotalConsumptionSharePercentage));

            OnPropertyChanged(nameof(OtherCostsAnnualCostsShare));
            OnPropertyChanged(nameof(TotalCostsAnnualCostsShare));

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));

        }

        private void DetermineAdvances()
        {
            Advances = 0.0;

            if (ViewModel != null)
            {
                if (((BillingViewModel)ViewModel).HasPayments)
                {
                    Advances += ((BillingViewModel)ViewModel).GetRoomPaymentsPerPeriod(_Room);
                }
                else
                {
                    // needs to get advances per room or needs to calculate the correct value
                    Advances += ((BillingViewModel)ViewModel).TotalAdvancePerPeriod;
                }
            }

            OnPropertyChanged(nameof(Advances));
        }

        private void DetermineHeatingUnitsAnnualConsumptionShares()
        {
            FinancialTransactionItem financialTransactionItem = ((BillingViewModel)ViewModel).GetBilling.TotalHeatingCostsPerPeriod;

            foreach (ConsumptionItemViewModel consumptionItemViewModel in ((BillingViewModel)ViewModel).ConsumptionItemViewModels)
            {
                if (consumptionItemViewModel.ConsumptionCause.Equals(financialTransactionItem.TransactionItem))
                {
                    foreach (RoomConsumptionViewModel item in consumptionItemViewModel.RoomConsumptionViewModels)
                    {
                        if (item.RoomName.Equals(RoomName) && item.RoomArea == RoomArea)
                        {
                            HeatingUnitsAnnualConsumptionShare = item.ConsumptionValue;

                            HeatingUnitsSharedConsumptionShare = DetermineSharedConsumptionShare(consumptionItemViewModel);

                            HeatingUnitsTotalConsumptionShare = HeatingUnitsAnnualConsumptionShare + HeatingUnitsSharedConsumptionShare;

                            HeatingUnitsTotalConsumptionSharePercentage = DetermineTotalConsumptionSharePercentage(HeatingUnitsTotalConsumptionShare, consumptionItemViewModel);

                            HeatingUnitsTotalConsumptionShareRatio = DetermineTotalConsumptionShareRatio(HeatingUnitsTotalConsumptionShare, consumptionItemViewModel);

                            break;
                        }
                    }
                }
            }
        }

        public double DetermineSharedConsumptionShare(ConsumptionItemViewModel consumptionItemViewModel)
        {
            return consumptionItemViewModel.SharedConsumption / ViewModel.GetFlatViewModel().RoomCount;
        }

        public double DetermineTotalConsumptionSharePercentage(double totalShare, ConsumptionItemViewModel consumptionItemViewModel)
        {
            return totalShare / consumptionItemViewModel.ConsumedUnits * 100;
        }


        public double DetermineTotalConsumptionShareRatio(double totalShare, ConsumptionItemViewModel consumptionItemViewModel)
        {
            return totalShare / consumptionItemViewModel.ConsumedUnits;
        }


        private double EqualShareRatio()
        {

            return 1.0 / ((BillingViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }


        private double GetAreaSharedCostsShare()
        {
            return RentedAreaShareRatio() * ((BillingViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypes.Area);
        }


        private void FillObservableCollection()
        {
            ConsumptionSharedCostsShare = 0.0;

            if (ViewModel != null)
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
                    else if (item.CostShareTypes == Enums.TransactionShareTypes.Consumption)
                    {
                        foreach (ConsumptionItemViewModel consumptionItemViewModel in ((BillingViewModel)ViewModel).ConsumptionItemViewModels)
                        {
                            if (consumptionItemViewModel.ConsumptionCause.Equals(item.Item)
                                && !consumptionItemViewModel.ConsumptionCause.Equals(((BillingViewModel)ViewModel).GetBilling.TotalHeatingCostsPerPeriod.TransactionItem))
                            {

                                foreach (RoomConsumptionViewModel roomConsumption in consumptionItemViewModel.RoomConsumptionViewModels)
                                {
                                    if (roomConsumption.RoomName.Equals(RoomName) && roomConsumption.RoomArea == RoomArea)
                                    {
                                        double totalshare = roomConsumption.ConsumptionValue + DetermineSharedConsumptionShare(consumptionItemViewModel);
                                       
                                        FTIvm.Cost = DetermineTotalConsumptionShareRatio(totalshare, consumptionItemViewModel) * item.Cost;
                                        ConsumptionSharedCostsShare += FTIvm.Cost;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    FTIvm.CostShareTypes = item.CostShareTypes;
                    
                    string newItem = item.Item;
                    FTIvm.Item = newItem;

                    FinancialTransactionItemViewModels.Add(FTIvm);
                }

            }

            OtherCostsAnnualCostsShare = AreaSharedCostsShare + EqualSharedCostsShare + ConsumptionSharedCostsShare;

            TotalCostsAnnualCostsShare = RentCostsAnnualShare + FixedCostsAnnualCostsShare + HeatingCostsAnnualCostsShare + OtherCostsAnnualCostsShare;
             
            OnPropertyChanged(nameof(ConsumptionSharedCostsShare));

            OnPropertyChanged(nameof(OtherCostsAnnualCostsShare));
            OnPropertyChanged(nameof(TotalCostsAnnualCostsShare));

            OnPropertyChanged(nameof(Balance));

            OnPropertyChanged(nameof(FinancialTransactionItemViewModels));
        }


        private double GetEqualSharedCostShare()
        {
            return ((BillingViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypes.Equal) / ((BillingViewModel)ViewModel).GetFlatViewModel().RoomCount;
        }


        private double RentedAreaShareRatio()
        {
            return RentedAreaShare / ViewModel.GetFlatViewModel().Area;
        }


    }
}
