using Microsoft.VisualBasic;
using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Interfaces.Contract;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class RoomCostShareBilling : BaseViewModel, IRoomCostShare
    {
        private int _RoomCount { get; set; } = 1;
        private double _FlatArea { get; set; } = 0.0;
        private double _SharedFlatArea { get; set; } = 0.0;


        private readonly Room _Room;

        public double Advances => DetermineAdvances();

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

        public ObservableCollection<RoomConsumptionViewModel> ConsumptionItemViewModels { get; set; } = new ObservableCollection<RoomConsumptionViewModel>();


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


        public void CalculateValues()
        {
            SharedAreaShare =_SharedFlatArea / _RoomCount;

            RentedAreaShare = SharedAreaShare + RoomArea;

            DetermineHeatingUnitsAnnualConsumptionShares();

            RentCostsAnnualShare = 0.0;
            FixedCostsAnnualCostsShare = RentedAreaShareRatio() * ((BillingViewModel)ViewModel).TotalFixedCostsPerPeriod;
            HeatingCostsAnnualCostsShare = HeatingUnitsTotalConsumptionShareRatio * ((BillingViewModel)ViewModel).TotalHeatingCostsPerPeriod;

            AreaSharedCostsShare = GetAreaSharedCostsShare();

            EqualSharedCostsShare = GetEqualSharedCostShare();

            ConsumptionSharedCostsShare = 0.0;

            DetermineAdvances();

            FillConsumptionItemViewModels();

            FillFinancialTransactionItemViewModels();

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

        private double DetermineAdvances()
        {
            double advance = 0.0;

            if (ViewModel != null)
            {
                if (((BillingViewModel)ViewModel).HasPayments)
                {
                    advance += GetRoomPaymentsPerPeriod(_Room);
                }
                else
                {
                    {
                        ObservableCollection<RentViewModel> RentList = new ObservableCollection<RentViewModel>();

                        // sort List by StartDate, ascending
                        RentList = ((BillingViewModel)ViewModel).FindRelevantRentViewModels();

                        DateTime start = DateTime.Now;
                        DateTime end = DateTime.Now;


                        for (int i = 0; i < RentList.Count; i++)
                        {
                            int month = 0;
                            double halfmonth = 0.0;

                            if (i == 0 && RentList.Count == 1)
                            {
                                start = ((BillingViewModel)ViewModel).StartDate;

                                end = ((BillingViewModel)ViewModel).EndDate;
                            }

                            else if (i == 0 && RentList.Count > 1)
                            {
                                start = ((BillingViewModel)ViewModel).StartDate;

                                end = RentList[i + 1].StartDate - TimeSpan.FromDays(1);
                            }
                            else if (i < RentList.Count - 1)
                            {
                                start = RentList[i].StartDate;

                                end = RentList[i + 1].StartDate - TimeSpan.FromDays(1);                         
                            }
                            else if (i == RentList.Count - 1)
                            {
                                start = RentList[i].StartDate;

                                end = ((BillingViewModel)ViewModel).EndDate;
                            }


                            if (start.Day == 1 && end.Day != 14 && start.Year == end.Year)
                            {
                                month = end.Month - start.Month;
                            }
                            
                            if (start.Day == 15 && end.Day != 14 && start.Year == end.Year)
                            {
                                month = end.Month - start.Month - 1;
                                halfmonth += 0.5;

                            }

                            if (end.Day == 14 || end.Day == 15 && start.Year == end.Year)
                            {
                                month = end.Month - start.Month - 1;
                                halfmonth = 0.5;
                            }

                            double Months = month + halfmonth;

                            advance += RentList[i].FixedCostsAdvance * Months * RentedAreaShareRatio();

                            if (!RentList[i].HasBilling)
                            {
                                advance += RentList[i].HeatingCostsAdvance * Months * RentedAreaShareRatio();
                            }
                            else
                            {                                
                                // search room consumption view models of billingviewmodel for HeatingUnitsTotalConsumptionShareRatio
                                //advance += RentList[i].HeatingCostsAdvance * Months * RentList[i].BillingViewModel.
                            }

                        }

                    }
                }
            }

            //OnPropertyChanged(nameof(Advances));
            //OnPropertyChanged(nameof(Balance));

            return advance;
        }


        public double GetRoomPaymentsPerPeriod(Room room)
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




        private void FillFinancialTransactionItemViewModels()
        {
            ConsumptionSharedCostsShare = 0.0;

            if (ViewModel != null)
            {
                FinancialTransactionItemViewModels.Clear();

                foreach (FinancialTransactionItemBillingViewModel item in ViewModel.FinancialTransactionItemViewModels)
                {
                    FinancialTransactionItemBillingViewModel FTIvm = new FinancialTransactionItemBillingViewModel(new FinancialTransactionItemBilling());

                    if (item.CostShareTypes == Enums.TransactionShareTypesBilling.Equal)
                    {
                        FTIvm.TransactionSum = EqualShareRatio() * item.TransactionSum;
                    }
                    else if (item.CostShareTypes == Enums.TransactionShareTypesBilling.Area)
                    {
                        FTIvm.TransactionSum = RentedAreaShareRatio() * item.TransactionSum;
                    }
                    else if (item.CostShareTypes == Enums.TransactionShareTypesBilling.Consumption)
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

                    FTIvm.CostShareTypes = item.CostShareTypes;

                    string newItem = item.TransactionItem;
                    FTIvm.TransactionItem = newItem;

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


        private double GetAreaSharedCostsShare()
        {
            return RentedAreaShareRatio() * ((BillingViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypesBilling.Area);
        }


        private double GetEqualSharedCostShare()
        {
            return ((BillingViewModel)ViewModel).GetFTIShareSum(Enums.TransactionShareTypesBilling.Equal) / _RoomCount;
        }


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
