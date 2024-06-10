using SharedLivingCostCalculator.Calculations;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.ViewModels
{
    public class RoomCostsViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        /// <summary>
        /// to do:
        /// 
        /// write new functions to specifically calculate billing period costs
        /// write new functions to calculate rent based on billing values
        /// 
        /// check every formula for mistakes
        /// 
        /// structure code and refactor or split this class and its components
        /// 
        /// comment methods and calculations
        /// 
        /// unit test calculations and modules
        /// </summary>


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

        public event EventHandler? HeatingUnitsChange;

        public double Percentage => CalculateConsumptionRatio() * 100;

        public double RentShare
        {
            get { return _roomCosts.RentShare; }
            set { _roomCosts.RentShare = value; OnPropertyChanged(nameof(RentShare)); }
        }

        public double FixedShare
        {
            get { return _roomCosts.FixedShare; }
            set { _roomCosts.FixedShare = value; OnPropertyChanged(nameof(FixedShare)); }
        }

        public double HeatingShare
        {
            get { return _roomCosts.HeatingShare; }
            set { _roomCosts.HeatingShare = value; OnPropertyChanged(nameof(HeatingShare)); }
        }

        public double Consumption => CalculateConsumption();
        public double RentCosts => CalculateRentCosts();
        public double FixedCosts => CalculateFixedCosts();
        public double HeatingCosts => CalculateHeatingCosts();
        public double BillingCosts => FixedCosts + HeatingCosts;

        public double AnnualRentShare => RentShare * 12;
        public double AnnualFixedShare => FixedShare * 12;
        public double AnnualHeatingShare => HeatingShare * 12;
        public double AnnualCosts => TotalCosts * 12;

        public double CostsPercentage => CalculateCostsPercentage();

        public double ExtraCostsShare => CalculateExtraCosts();

        public double TotalCosts => RentShare + ExtraCostsShare;

        public double TotalPayments => GetPayments();

        public double Balance => CalculateBalance(); // braucht visibility oder andere einschränkung


        public RoomCostsViewModel(RoomViewModel room, IRoomCostsCarrier roomCostsCarrier)
        {
            _room = room;
            _roomCosts = new RoomCosts(room);

            _roomCostsCarrier = roomCostsCarrier;

            _roomCostsCarrier.DataChange += _roomCostsCarrier_DataChange;

            CalculateData();
        }

        private void _roomCostsCarrier_DataChange(object? sender, PropertyChangedEventArgs e)
        {
            CalculateData();
        }


        public void CalculateData()
        {
            RentShare = CalculateRentCosts();
            FixedShare = CalculateFixedCosts();
            HeatingShare = CalculateHeatingCosts();            
        }

        private double CalculateRentCosts()
        {
            double rentCosts = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                TimeSpan timeSpan = billingViewModel.EndDate - billingViewModel.StartDate;

                int months = timeSpan.Days / 30;

                if (billingViewModel.RentViewModel != null)
                {
                    rentCosts = AreaRatio() * billingViewModel.RentViewModel.ColdRent * months;
                }
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

                balance = TotalPayments - BillingCosts - RentCosts;
            }

            return balance;
        }

        private double GetPayments()
        {
            BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            return _room.CalculatePaymentsPerPeriod(billingViewModel.StartDate, billingViewModel.EndDate);
        }

        private double CalculateConsumption()
        {
            BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

            double sharedConsumption = billingViewModel.TotalHeatingUnitsConsumption - billingViewModel.TotalHeatingUnitsRoom;

            double consumption = _roomCosts.HeatingUnitsConsumption + sharedConsumption / billingViewModel.GetFlatViewModel().RoomCount;

            return consumption;
        }

            
        private double CalculateConsumptionRatio()
        {
            double consumptionRatio = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                consumptionRatio = CalculateConsumption() / ((BillingViewModel)_roomCostsCarrier).TotalHeatingUnitsConsumption;
            }

            return consumptionRatio;
        }


        private double CalculateExtraCosts()
        {
            return CalculateFixedShare() + CalculateHeatingShare();
        }


        private double CalculateFixedShare()
        {
            double fixedShare = 0.0;

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                if (billingViewModel.RentViewModel != null)
                {
                    fixedShare = AreaRatio() * billingViewModel.RentViewModel.FixedCostsAdvance;
                }
            }


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

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;
               
                double consumptionRatio = CalculateConsumptionRatio();

                if (billingViewModel.RentViewModel != null)
                {
                    heatingShare = consumptionRatio * billingViewModel.RentViewModel.HeatingCostsAdvance;
                }
            }


            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                heatingShare = AreaRatio() * ((RentViewModel)_roomCostsCarrier).HeatingCostsAdvance;
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
            return RentedArea() / _roomCostsCarrier.GetFlatViewModel().Area;
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

            if (_roomCostsCarrier.GetType() == typeof(BillingViewModel))
            {
                BillingViewModel billingViewModel = (BillingViewModel)_roomCostsCarrier;

                if (billingViewModel.RentViewModel != null)
                {
                    rent = AreaRatio() * billingViewModel.RentViewModel.ColdRent;
                }
            }

            if (_roomCostsCarrier.GetType() == typeof(RentViewModel))
            {
                rent = AreaRatio() * ((RentViewModel)_roomCostsCarrier).ColdRent;
            }

            RentShare = rent;

            return rent;
        }


        private bool TotalConsumptionIsLesserThanSum()
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
    }
}
