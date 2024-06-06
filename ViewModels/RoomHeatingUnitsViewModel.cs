using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    public class RoomHeatingUnitsViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly RoomHeatingUnits _roomHeatingUnits;
        public RoomHeatingUnits GetRoomHeatingUnits => _roomHeatingUnits;

        private readonly BillingViewModel _billingViewModel;
        private ValidationHelper helper = new ValidationHelper();

        public event EventHandler? HeatingUnitsChange;

        public RoomViewModel Room => GetRoomHeatingUnits.Room;

        //public string RoomName => GetRoomHeatingUnits.Room.RoomName;
        //public double RoomArea => GetRoomHeatingUnits.Room.RoomArea;


        public double HeatingUnitsConsumption
        {
            get
            {
                return _roomHeatingUnits.HeatingUnitsConsumption;
            }
            set
            {

                helper.ClearError(nameof(HeatingUnitsConsumption));

                if (value < 0)
                {
                    helper.AddError("Value must be positive", nameof(HeatingUnitsConsumption));
                }

                if (!TotalConsumptionIsLesserThanSum())
                {
                    helper.AddError("the sum of room consumption values must be lesser\n" +
                        "or equal to the total consumption", nameof(HeatingUnitsConsumption));
                }

                _roomHeatingUnits.HeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(HeatingUnitsConsumption));
                OnPropertyChanged(nameof(Percentage));
                

                HeatingUnitsChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public double Percentage => _roomHeatingUnits.HeatingUnitsConsumption / _billingViewModel.TotalHeatingUnitsConsumption * 100;

        public RoomHeatingUnitsViewModel(RoomHeatingUnits roomHeatingUnits, BillingViewModel billingPeriod)
        {
            _roomHeatingUnits = roomHeatingUnits;
            _billingViewModel = billingPeriod;
        }

        private bool TotalConsumptionIsLesserThanSum()
        {
            double totalConsumption = _billingViewModel.TotalHeatingUnitsConsumption;

            foreach (RoomHeatingUnitsViewModel roomConsumption in _billingViewModel.RoomConsumptionValues)
            {
                totalConsumption -= roomConsumption.HeatingUnitsConsumption;


                if (totalConsumption < 0)
                {
                    break;
                }
            }

            return totalConsumption >= 0;
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool HasErrors => helper.HasErrors;
        public IEnumerable GetErrors(string? propertyName) => helper.GetErrors(propertyName);
    }
}
