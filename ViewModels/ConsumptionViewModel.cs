using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    public class ConsumptionViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties & fields

        private BillingViewModel _billingViewModel;


        public bool DataLock
        {
            get { return !_billingViewModel.HasDataLock; }
        }


        public bool HasErrors => _helper.HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        public double SharedConsumption => _billingViewModel.SharedHeatingUnitsConsumption;


        public double SharedConsumptionPercentage => _billingViewModel.SharedHeatingUnitsConsumptionPercentage;


        public double TotalHeatingUnitsConsumption
        {
            get
            {
                return _billingViewModel.TotalHeatingUnitsConsumption;
            }
            set
            {
                _helper.ClearError(nameof(TotalHeatingUnitsConsumption));
                _helper.ClearError(nameof(TotalHeatingUnitsRoom));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingUnitsConsumption));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingUnitsConsumption));
                }


                if (value < TotalHeatingUnitsRoom)
                {
                    _helper.AddError("value can not be lesser than combined rooms value", nameof(TotalHeatingUnitsConsumption));
                }

                _billingViewModel.TotalHeatingUnitsConsumption = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsConsumption));
            }
        }


        public double TotalHeatingUnitsRoom
        {
            get
            {
                return _billingViewModel.TotalHeatingUnitsRoom;
            }
            set
            {
                _helper.ClearError(nameof(TotalHeatingUnitsRoom));
                _helper.ClearError(nameof(TotalHeatingUnitsConsumption));

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(TotalHeatingUnitsRoom));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(TotalHeatingUnitsRoom));
                }

                if (value > TotalHeatingUnitsConsumption)
                {
                    _helper.AddError("value can not be greater than total value", nameof(TotalHeatingUnitsRoom));
                }

                _billingViewModel.TotalHeatingUnitsRoom = value;
                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties & fields


        // collections
        # region collections

        public ObservableCollection<RoomCostsViewModel> RoomCosts => _billingViewModel.RoomCosts;

        #endregion collections


        // constructors
        #region constructors

        public ConsumptionViewModel(BillingViewModel billingViewModel)
        {
            _billingViewModel = billingViewModel;

            if (_billingViewModel != null)
            {
                foreach (RoomCostsViewModel rhu in RoomCosts)
                {
                    rhu.HeatingUnitsChange += HeatingUnitsChange;
                }
            }

            _billingViewModel.PropertyChanged += _billingViewModel_PropertyChanged;
        }

        #endregion constructors


        // methods
        #region methods

        public void CalculateRoomsConsumption()
        {
            if (RoomCosts != null)
            {
                TotalHeatingUnitsRoom = 0.0;

                foreach (RoomCostsViewModel roomConsumption in RoomCosts)
                {
                    TotalHeatingUnitsRoom += roomConsumption.HeatingUnitsConsumption;
                }

                OnPropertyChanged(nameof(TotalHeatingUnitsRoom));
            }
        }


        private void HeatingUnitsChange(object? sender, EventArgs e)
        {
            CalculateRoomsConsumption();
        }

        #endregion methods


        // events
        #region events

        private void _billingViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DataLock));
        }

        #endregion events


    }
}
