/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCostsViewModel  : BaseViewModel
 * 
 *  viewmodel for RoomCosts model
 *  
 *  purpose:
 *      -> calculate costs for a room instance
 *          within IRoomCostCarrier classes BillingViewModel or RentViewModel 
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class RoomCostsViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        /// <summary>
        /// to do:
        /// 
        /// check every formula for mistakes
        /// 
        /// structure code and refactor or split this class and its components
        /// 
        /// comment methods and calculations
        /// 
        /// unit test calculations and modules
        /// </summary>
        /// 


        public event EventHandler? HeatingUnitsChange;


        private ValidationHelper _helper = new ValidationHelper();


        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;


        public bool HasErrors => _helper.HasErrors;


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private readonly IRoomCostsCarrier _roomCostsCarrier;


        private readonly RoomViewModel _room;
        public RoomViewModel Room => _room;


        private readonly RoomCosts _roomCosts;
        public RoomCosts GetRoomCosts => _roomCosts;


        public double HeatingUnitsConsumption
        {
            get
            {
                return _roomCosts.HeatingUnitsConsumption;
            }
            set
            {

                _helper.ClearError(nameof(HeatingUnitsConsumption));

                if (value < 0)
                {
                    _helper.AddError("Value must be positive", nameof(HeatingUnitsConsumption));
                }

                if (!TotalConsumptionIsLesserThanSum())
                {
                    _helper.AddError("the sum of room consumption values must be lesser\n" +
                        "or equal to the total consumption", nameof(HeatingUnitsConsumption));
                }

                _roomCosts.HeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(HeatingUnitsConsumption));
                OnPropertyChanged(nameof(Percentage));

                HeatingUnitsChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public double Percentage => CalculateConsumptionRatio() * 100;


        public double RentShare
        {
            get { return _roomCosts.RentShare; }
            set
            {
                _roomCosts.RentShare = value; OnPropertyChanged(nameof(RentShare));
                OnPropertyChanged(nameof(AnnualRentShare));
            }
        }


        public double FixedShare
        {
            get { return _roomCosts.FixedShare; }
            set
            {
                _roomCosts.FixedShare = value; OnPropertyChanged(nameof(FixedShare));
                OnPropertyChanged(nameof(AnnualFixedShare));
            }
        }


        public double HeatingShare
        {
            get { return _roomCosts.HeatingShare; }
            set
            {
                _roomCosts.HeatingShare = value; OnPropertyChanged(nameof(HeatingShare));
                OnPropertyChanged(nameof(AnnualHeatingShare));
            }
        }


        public double Consumption => CalculateConsumption();


        public double RentCosts => CalculateRentCosts();


        public double FixedCosts => CalculateFixedCosts();


        public double HeatingCosts => CalculateHeatingCosts();


        public double BillingCosts => RentCosts + FixedCosts + HeatingCosts;


        public double AnnualRentShare => RentShare * 12;


        public double AnnualFixedShare => FixedShare * 12;


        public double AnnualHeatingShare => HeatingShare * 12;


        public double AnnualCosts => TotalCosts * 12;


        public double AnnualOtherCosts => CombinedOtherCosts * 12;


        public double AnnualCombinedCosts => AnnualCosts + AnnualOtherCosts;


        public double CostsPercentage => CalculateCostsPercentage();


        public double ExtraCostsShare => CalculateFixedShare() + CalculateHeatingShare();


        public double TotalCosts => RentShare + ExtraCostsShare;


        public double TotalPayments => GetPayments();


        public double Balance => CalculateBalance();


        public double CombinedOtherCosts => CalculateCombinedOtherCosts();


        public double CompleteCosts => TotalCosts + CombinedOtherCosts;


        public ObservableCollection<OtherCostItemViewModel> OtherCosts { get; set; } = new ObservableCollection<OtherCostItemViewModel>();


        public RoomCostsViewModel(RoomCosts roomCosts, IRoomCostsCarrier roomCostsCarrier)
        {
            _roomCosts = roomCosts;

            _roomCostsCarrier = roomCostsCarrier;

            _roomCostsCarrier.DataChange += _roomCostsCarrier_DataChange;

            foreach (RoomViewModel room in _roomCostsCarrier.GetFlatViewModel().Rooms)
            {
                if (room.ID == _roomCosts.RoomID)
                {
                    _room = room;
                    break;
                }
            }

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                CalculateMonthlyCosts();
            }
        }


        private void _roomCostsCarrier_DataChange(object? sender, PropertyChangedEventArgs e)
        {
            CalculateMonthlyCosts();

        }


        public void CalculateMonthlyCosts()
        {
            RentShare = CalculateRentShare();
            FixedShare = CalculateFixedShare();
            HeatingShare = CalculateHeatingShare();

            CalculateOtherCosts();
        }

        private double CalculateCombinedOtherCosts()
        {
            double combinedOtherCosts = 0.0;

            foreach (OtherCostItemViewModel item in OtherCosts)
            {
                combinedOtherCosts += item.Cost;
            }

            return combinedOtherCosts;
        }

        private void CalculateOtherCosts()
        {
            OtherCosts.Clear();

            double area_share = 0.0;
            double equal_share = 0.0;

            foreach (OtherCostItemViewModel item in ((RentViewModel)_roomCostsCarrier).OtherCosts)
            {
                OtherCostItem otherCostItem = new OtherCostItem();

                otherCostItem.CostShareTypes = item.CostShareTypes;
                otherCostItem.Item = item.Item;

                if (item.CostShareTypes == CostShareTypes.Equal)
                {
                    equal_share = item.Cost / _roomCostsCarrier.GetFlatViewModel().RoomCount;

                    otherCostItem.Cost = equal_share;
                }

                if (item.CostShareTypes == CostShareTypes.Area)
                {
                    area_share = item.Cost * AreaRatio();

                    otherCostItem.Cost = area_share;
                }

                OtherCosts.Add(new OtherCostItemViewModel(otherCostItem));
            }
        }


        private double CalculateRentCosts() // !!! hier anpassen, alle mieten durchsuchen nach datum.
        {
            double rentCosts = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                TimeSpan timeSpan = billingViewModel.EndDate - billingViewModel.StartDate;

                int months = timeSpan.Days / 30;

                //if (billingViewModel.RentViewModel != null)
                //{
                //    rentCosts = AreaRatio() * billingViewModel.RentViewModel.ColdRent * months;
                //}
            }

            return rentCosts;
        }


        private double CalculateFixedCosts()
        {
            double fixedCosts = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                fixedCosts = billingViewModel.TotalFixedCostsPerPeriod * AreaRatio();
            }

            return fixedCosts;
        }


        private double CalculateHeatingCosts()
        {
            double heatingCosts = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                double consumptionRatio = CalculateConsumptionRatio();

                heatingCosts = billingViewModel.TotalHeatingCostsPerPeriod * consumptionRatio;
            }

            return heatingCosts;
        }


        private double CalculateBalance()
        {
            double balance = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                balance = TotalPayments - BillingCosts;
            }

            return balance;
        }


        private double GetPayments()
        {
            BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            double payments = 0.0;

            if (billingViewModel != null && billingViewModel.RoomPayments != null)
            {
                foreach (RoomPaymentsViewModel roomPaymentsViewModel in billingViewModel.RoomPayments)
                {
                    if (roomPaymentsViewModel.RoomPayments.RoomViewModel != null && roomPaymentsViewModel.RoomPayments.RoomViewModel.ID == Room.ID)
                    {
                        payments = roomPaymentsViewModel.CombinedPayments;
                    }
                }
            }


            return payments;
        }


        private double CalculateConsumption()
        {
            double consumption = 0.0;
            double sharedConsumption = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                sharedConsumption = billingViewModel.TotalHeatingUnitsConsumption - billingViewModel.TotalHeatingUnitsRoom;

                consumption = _roomCosts.HeatingUnitsConsumption + sharedConsumption / billingViewModel.GetFlatViewModel().RoomCount;
            }
            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                if (((RentViewModel)_roomCostsCarrier).BillingViewModel != null)
                {
                    BillingViewModel billingViewModel = ((RentViewModel)_roomCostsCarrier).BillingViewModel;

                    sharedConsumption = billingViewModel.TotalHeatingUnitsConsumption - billingViewModel.TotalHeatingUnitsRoom;

                    foreach (RoomCostsViewModel roomCosts in billingViewModel.RoomCosts)
                    {
                        if (roomCosts.Room.ID == Room.ID)
                        {
                            consumption = roomCosts.HeatingUnitsConsumption + sharedConsumption / billingViewModel.GetFlatViewModel().RoomCount;

                            break;
                        }
                    }
                }
            }

            return consumption;
        }


        private double CalculateConsumptionRatio()
        {
            double consumptionRatio = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                consumptionRatio = CalculateConsumption() / billingViewModel.TotalHeatingUnitsConsumption;
            }
            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                if (((RentViewModel)_roomCostsCarrier).BillingViewModel != null)
                {
                    BillingViewModel billingViewModel = ((RentViewModel)_roomCostsCarrier).BillingViewModel;
                    consumptionRatio = CalculateConsumption() / billingViewModel.TotalHeatingUnitsConsumption;
                }

            }

            return consumptionRatio;
        }


        private double CalculateFixedShare()
        {
            double fixedShare = 0.0;

            //if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            //{
            //    BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            //    if (billingViewModel.RentViewModel != null)
            //    {
            //        fixedShare = AreaRatio() * billingViewModel.RentViewModel.FixedCostsAdvance;
            //    }
            //}


            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                fixedShare = AreaRatio() * ((RentViewModel)_roomCostsCarrier).FixedCostsAdvance;
            }

            FixedShare = fixedShare;

            return fixedShare;
        }


        private double CalculateHeatingShare()
        {
            double heatingShare = 0.0;
            double consumptionRatio = CalculateConsumptionRatio();

            //if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            //{
            //    BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            //    if (billingViewModel.RentViewModel != null)
            //    {
            //        heatingShare = consumptionRatio * billingViewModel.RentViewModel.HeatingCostsAdvance;
            //    }
            //}


            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                RentViewModel rentViewModel = (RentViewModel)_roomCostsCarrier;

                if (rentViewModel.HasBilling)
                {
                    heatingShare = consumptionRatio * rentViewModel.HeatingCostsAdvance;
                }
                else
                {
                    heatingShare = AreaRatio() * ((RentViewModel)_roomCostsCarrier).HeatingCostsAdvance;
                }


            }

            HeatingShare = heatingShare;

            return heatingShare;
        }


        private double RentedArea()
        {
            double sharedAreaShare = _roomCostsCarrier.GetFlatViewModel().SharedArea / _roomCostsCarrier.GetFlatViewModel().RoomCount;

            return _room.RoomArea + sharedAreaShare;
        }


        private double AreaRatio()
        {
            if (_room != null)
            {
                return RentedArea() / _roomCostsCarrier.GetFlatViewModel().Area;
            }

            return 0.0;
        }


        private double CalculateCostsPercentage()
        {
            double percentage = 0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                percentage = (FixedCosts + HeatingCosts) / billingViewModel.TotalCostsPerPeriod * 100;

                // anteil müsste sein alle kosten über zeitraum pro raum /  alle kosten über zeitraum
                // also werte aus billingviewmodel ziehen.
                // rent vorerst außen vor, ist ein anderer kostenteil, ist auch in der abrechnung
                // nicht ausgewiesen, wird nur benötigt um die payments korrekt zu rechnen.
            }

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                percentage = TotalCosts / ((RentViewModel)_roomCostsCarrier).CostsTotal * 100;
            }

            return percentage;
        }


        private double CalculateRentShare()
        {
            double rent = 0;

            //if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            //{
            //    BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            //    if (billingViewModel.RentViewModel != null)
            //    {
            //        rent = AreaRatio() * billingViewModel.RentViewModel.ColdRent;
            //    }
            //}

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                rent = AreaRatio() * ((RentViewModel)_roomCostsCarrier).ColdRent;
            }

            RentShare = rent;

            return rent;
        }


        private bool TotalConsumptionIsLesserThanSum()
        {
            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                double totalConsumption = ((BillingViewModel)_roomCostsCarrier).TotalHeatingUnitsConsumption;

                foreach (RoomCostsViewModel roomCosts in ((BillingViewModel)_roomCostsCarrier).RoomCosts)
                {
                    totalConsumption -= roomCosts.HeatingUnitsConsumption;

                    if (totalConsumption < 0)
                    {
                        break;
                    }
                }

                return totalConsumption >= 0;
            }

            return false;
        }


    }
}
// EOF