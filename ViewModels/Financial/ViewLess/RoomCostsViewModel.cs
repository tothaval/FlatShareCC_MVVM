/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomCostsViewModel  : BaseViewModel
 * 
 *  viewmodel for RoomCosts model
 *  
 *  purpose:
 *      -> calculate costs for a room instance
 *          within IRoomCostCarrier classes BillingViewModel or RentViewModel 
 */
using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class RoomCostsViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        /// <summary>
        /// to do:
        /// 
        /// check every formula for mistakes    <= begun
        /// 
        /// refactor or split this class and its components
        /// 
        /// comment methods and calculations <= begun elsewhere
        /// 
        /// unit test calculations and modules <= start researching&fixing the bug with the project dependencies in VS, bin and obj delete didn't work
        /// </summary>


        // properties & fields
        #region properties & fields

        // Billing related properties
        #region Billing related properties
        public double BillingCosts => RentCosts + FixedCosts + HeatingCosts;


        public double CombinedConsumption => CalculateCombinedConsumption();


        public double CombinedConsumptionPercentage => CalculateCombinedConsumptionRatio() * 100;


        public double FixedCosts => CalculateFixedCosts();


        public double HeatingCosts => CalculateHeatingCosts();


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

                if (!TotalConsumptionIsLessThanSum())
                {
                    _helper.AddError("the sum of room consumption values must be lesser\n" +
                        "or equal to the total consumption", nameof(HeatingUnitsConsumption));
                }

                _roomCosts.HeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(HeatingUnitsConsumption));
                OnPropertyChanged(nameof(RoomConsumptionPercentage));

                HeatingUnitsChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public double RentCosts => CalculateRentCosts();


        public double RoomConsumptionPercentage => CalculateConsumptionRatio() * 100;


        public double SharedConsumption => CalculateSharedConsumption();
        #endregion Billing related properties


        // Rent related properties
        #region Rent related Properties
        public double SharedAreaShare => BasicCalculations.SharedAreaShare(_roomCostsCarrier.GetFlatViewModel());


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
        #endregion Rent related Properties


        // other properties
        #region other properties

        public double AnnualCombinedCosts => AnnualCosts + AnnualOtherCosts;


        public double AnnualCosts => TotalCosts * 12;


        public double AnnualFixedShare => FixedShare * 12;


        public double AnnualHeatingShare => HeatingShare * 12;


        public double AnnualOtherCosts => CombinedOtherCosts * 12;


        public double AnnualRentShare => RentShare * 12;


        public double Balance => CalculateBalance();


        public double CombinedOtherCosts => CalculateCombinedOtherCosts();


        public double CompleteCosts => TotalCosts + CombinedOtherCosts;


        public double CostsPercentage => CalculateCostsPercentage();


        public double ExtraCostsShare => CalculateFixedShare() + CalculateHeatingShare();


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public bool HasErrors => _helper.HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


        private readonly RoomViewModel _room;
        public RoomViewModel Room => _room;


        private readonly RoomCosts _roomCosts;
        public RoomCosts GetRoomCosts => _roomCosts;


        private readonly IRoomCostsCarrier _roomCostsCarrier;


        public double TotalCosts => RentShare + ExtraCostsShare;


        public double TotalPayments => GetPayments();

        #endregion other properties

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event EventHandler? HeatingUnitsChange;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // collections
        #region collections

        public ObservableCollection<CostItemViewModel> OtherCosts { get; set; } = new ObservableCollection<CostItemViewModel>();

        #endregion collections


        // constructors
        #region constructors        

        public RoomCostsViewModel(RoomCosts roomCosts, IRoomCostsCarrier roomCostsCarrier)
        {
            _roomCosts = roomCosts;

            _roomCostsCarrier = roomCostsCarrier;

            _roomCostsCarrier.DataChange += _roomCostsCarrier_DataChange;

            foreach (RoomViewModel room in _roomCostsCarrier.GetFlatViewModel().Rooms)
            {
                if (room.RoomName.Equals(_roomCosts.RoomName))
                {
                    _room = room;
                    break;
                }
            }

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                CalculateMonthlyCosts();
            }

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                ((BillingViewModel)_roomCostsCarrier).DataChange += BillingViewModel_DataChange;
            }
        }

        #endregion constructors


        // methods
        #region methods

        // Billing Calculations 
        #region Billing Calculations        

        private double CalculateCombinedConsumption()
        {
            double combinedConsumption = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                combinedConsumption = new BillingCalculations().CombinedRoomConsumption(billingViewModel, _roomCosts.HeatingUnitsConsumption);
            }
            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                if (((RentViewModel)_roomCostsCarrier).BillingViewModel != null)
                {
                    BillingViewModel billingViewModel = ((RentViewModel)_roomCostsCarrier).BillingViewModel;

                    foreach (RoomCostsViewModel roomCosts in billingViewModel.RoomCosts)
                    {
                        if (roomCosts.Room.RoomName.Equals(Room.RoomName))
                        {
                            combinedConsumption = new BillingCalculations().CombinedRoomConsumption(billingViewModel, _roomCosts.HeatingUnitsConsumption);

                            break;
                        }
                    }
                }
            }

            return combinedConsumption;
        }


        private double CalculateCombinedConsumptionRatio()
        {
            double combinedConsumptionRatio = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                combinedConsumptionRatio = new BillingCalculations().CombinedRoomConsumptionRatio(billingViewModel, _roomCosts.HeatingUnitsConsumption);
            }

            // probably obsolete due to recent logic, ui and workflow changes
            //if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            //{
            //    if (((RentViewModel)_roomCostsCarrier).BillingViewModel != null)
            //    {
            //        BillingViewModel billingViewModel = ((RentViewModel)_roomCostsCarrier).BillingViewModel;

            //        foreach (RoomCostsViewModel roomCosts in billingViewModel.RoomCosts)
            //        {
            //            if (roomCosts.Room.ID == Room.ID)
            //            {
            //                combinedConsumptionRatio = BillingCalculations.CombinedRoomConsumptionRatio(billingViewModel, _roomCosts.HeatingUnitsConsumption);

            //                break;
            //            }
            //        }
            //    }
            //}

            return combinedConsumptionRatio;
        }


        private double CalculateConsumptionRatio()
        {
            double consumptionRatio = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                consumptionRatio = new BillingCalculations().ConsumptionRatio(billingViewModel, _roomCosts.HeatingUnitsConsumption);
            }

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                if (((RentViewModel)_roomCostsCarrier).BillingViewModel != null)
                {
                    BillingViewModel billingViewModel = ((RentViewModel)_roomCostsCarrier).BillingViewModel;

                    foreach (RoomCostsViewModel roomCosts in billingViewModel.RoomCosts)
                    {
                        if (roomCosts.Room != null)
                        {
                            if (roomCosts.Room.RoomName.Equals(_roomCosts.RoomName))
                            {
                                consumptionRatio = new BillingCalculations().ConsumptionRatio(billingViewModel, roomCosts.HeatingUnitsConsumption);
                            }
                        }
                    }


                }
            }

            return consumptionRatio;
        }


        private double CalculateSharedConsumption()
        {
            double sharedConsumption = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                sharedConsumption = new BillingCalculations().SharedRoomConsumption(billingViewModel);
            }
            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                if (((RentViewModel)_roomCostsCarrier).BillingViewModel != null)
                {
                    BillingViewModel billingViewModel = ((RentViewModel)_roomCostsCarrier).BillingViewModel;

                    sharedConsumption = new BillingCalculations().SharedRoomConsumption(billingViewModel);
                }
            }

            return sharedConsumption;
        }

        #endregion Billing Calculations 


        private double AreaRatio()
        {
            if (_room != null)
            {
                return BasicCalculations.CalculateRatio(RentedArea(), _roomCostsCarrier.GetFlatViewModel().Area);
            }

            return 0.0;
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


        private double CalculateBillingMonths(BillingViewModel billingViewModel, TimeSpan timeSpan)
        {
            // the code below
            // this is targeting at cases where a rent begin is somewhere in between months.
            // hopefully most raises won't happen in the middle of a month, but with the start
            // of a new month.
            // in such cases, this here is irrelevant and could be replaced with some easier and
            // cleaner/better way of counting months to correctly factor in a raised rent into
            // the calculation of monthly costs

            // the calculation of billing months is needed to correctly calculate advances and rents
            // for either billing costs with payments and without. the latter needs the number of months
            // for the correct display of advances, the former for the correct calculation of rent
            // and advances.
            //
            // because payments are combined of all cost factors, it is easier for the user to just
            // insert the payments with no need to worry about the different cost factors. 
            // if payments are set in a BillingWindow, the sum of all Payments per Room shall be
            // subtracted by the complete rent costs for the period, followed by fixed costs and heating
            // costs, the result determines the balance of the room account.

            // a quick research on the net revealed:
            // rents can be a half month or a total month, rent calculation is based on 30 days per month.
            // this holds true for germany, i have absolutely no idea about rent customs and/or laws in
            // other countries.


            int days = timeSpan.Days;

            double value = days / 30.0;

            int total_months = days / 30;

            value = value - total_months;

            double months = 0;

            if (value <= 0.34)
            {
                months = total_months;
            }
            else if (value > 0.34 && value <= 0.75)
            {
                months = total_months + 0.5;
            }
            else if (value > 0.75)
            {
                months = total_months + 1;
            }

            return months;
        }


        private double CalculateCombinedOtherCosts()
        {
            double combinedOtherCosts = 0.0;

            foreach (CostItemViewModel item in OtherCosts)
            {
                combinedOtherCosts += item.Cost;
            }

            return combinedOtherCosts;
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


        private double CalculateFixedShare()
        {
            double fixedShare = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                fixedShare = AreaRatio() * ((RentViewModel)_roomCostsCarrier).FixedCostsAdvance;
            }

            FixedShare = fixedShare;

            return fixedShare;
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


        private double CalculateHeatingShare()
        {
            double heatingShare = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                RentViewModel rentViewModel = (RentViewModel)_roomCostsCarrier;

                if (rentViewModel.HasBilling)
                {
                    heatingShare = CalculateConsumptionRatio() * rentViewModel.HeatingCostsAdvance;
                }
                else
                {
                    heatingShare = AreaRatio() * ((RentViewModel)_roomCostsCarrier).HeatingCostsAdvance;
                }
            }

            HeatingShare = heatingShare;

            return heatingShare;
        }


        public void CalculateMonthlyCosts()
        {
            RentShare = CalculateRentShare();
            FixedShare = CalculateFixedShare();
            HeatingShare = CalculateHeatingShare();

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                // too slow, solve the property update issue different
                // RegisterCostItemViewModelChangeEvents();
                CalculateOtherCosts();
            }
        }


        private void CalculateOtherCosts()
        {
            OtherCosts.Clear();

            double area_share = 0.0;
            double equal_share = 0.0;

            foreach (CostItemViewModel item in ((RentViewModel)_roomCostsCarrier).Costs)
            {
                FinancialTransactionItem otherCostItem = new FinancialTransactionItem();

                otherCostItem.TransactionShareTypes = item.CostShareTypes;
                otherCostItem.TransactionItem = item.Item;

                if (item.CostShareTypes == TransactionShareTypes.Equal)
                {
                    equal_share = item.Cost / _roomCostsCarrier.GetFlatViewModel().RoomCount;

                    otherCostItem.TransactionSum = equal_share;
                }

                if (item.CostShareTypes == TransactionShareTypes.Area)
                {
                    area_share = item.Cost * AreaRatio();

                    otherCostItem.TransactionSum = area_share;
                }



                OtherCosts.Add(new CostItemViewModel(otherCostItem));
            }
        }


        private double CalculateRentCosts()
        {
            double rentCosts = 0.0;
            double months = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                ObservableCollection<RentViewModel> rentCollection = billingViewModel.FindRelevantRentViewModels();


                for (int i = rentCollection.Count - 1; i >= 0; i--)
                {
                    // newest item in the list
                    if (i == rentCollection.Count - 1)
                    {
                        TimeSpan timeSpan = billingViewModel.EndDate - rentCollection[i].StartDate;

                        months = CalculateBillingMonths(billingViewModel, timeSpan);

                        rentCosts += AreaRatio() * rentCollection[i].ColdRent * CalculateBillingMonths(billingViewModel, timeSpan);
                    }

                    // items in between newest and oldest
                    if (i > 0 && i < rentCollection.Count - 1)
                    {
                        TimeSpan timeSpan = rentCollection[i + 1].StartDate - rentCollection[i].StartDate;

                        months += CalculateBillingMonths(billingViewModel, timeSpan);
                        rentCosts += AreaRatio() * rentCollection[i].ColdRent * CalculateBillingMonths(billingViewModel, timeSpan);
                    }

                    // oldest item in the list, calculate span between StartDate billing and StartDate previous item
                    if (i == 0)
                    {
                        TimeSpan timeSpan = rentCollection[i + 1].StartDate - billingViewModel.StartDate;

                        months += CalculateBillingMonths(billingViewModel, timeSpan);
                        rentCosts += AreaRatio() * rentCollection[i].ColdRent * CalculateBillingMonths(billingViewModel, timeSpan);
                    }
                }
            }

            return rentCosts;
        }


        private double CalculateRentShare()
        {
            double rent = 0;

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                rent = AreaRatio() * ((RentViewModel)_roomCostsCarrier).ColdRent;
            }

            RentShare = rent;

            return rent;
        }


        private double GetPayments()
        {
            BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            double payments = 0.0;

            if (billingViewModel != null && billingViewModel.RoomPayments != null)
            {
                foreach (RoomPaymentsViewModel roomPaymentsViewModel in billingViewModel.RoomPayments)
                {
                    if (roomPaymentsViewModel.RoomPayments.RoomViewModel != null && roomPaymentsViewModel.RoomPayments.RoomViewModel.RoomName.Equals(Room.RoomName))
                    {
                        payments = roomPaymentsViewModel.CombinedPayments;
                    }
                }
            }


            return payments;
        }


        private double RentedArea()
        {
            return _room.RoomArea + SharedAreaShare;
        }


        private bool TotalConsumptionIsLessThanSum()
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

        #endregion methods


        // events
        #region events

        private void BillingViewModel_DataChange(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CombinedConsumption));
            OnPropertyChanged(nameof(RoomConsumptionPercentage));
            OnPropertyChanged(nameof(HeatingCosts));
            OnPropertyChanged(nameof(BillingCosts));
        }


        private void _roomCostsCarrier_DataChange(object? sender, PropertyChangedEventArgs e)
        {
            CalculateMonthlyCosts();
        }

        #endregion events


    }
}
// EOF