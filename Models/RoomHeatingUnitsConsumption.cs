using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    public class RoomHeatingUnitsConsumption : INotifyDataErrorInfo
    {
        private readonly BillingPeriod _billingPeriod;
        private ValidationHelper helper = new ValidationHelper();
        

        public Room Room {  get; }

        private double _heatingUnitsConsumption;
        public double HeatingUnitsConsumption
        {
            get { return _heatingUnitsConsumption; }
            set {

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

                _heatingUnitsConsumption = value;            

            }
        }


        public RoomHeatingUnitsConsumption(Room room, BillingPeriod billingPeriod)
        {
                Room = room;
            _billingPeriod = billingPeriod;
        }

        private bool TotalConsumptionIsLesserThanSum()
        {
            double totalConsumption = _billingPeriod.TotalHeatingUnitsConsumption;

            foreach (RoomHeatingUnitsConsumption roomConsumption in _billingPeriod.RoomConsumptionValues)
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
