using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class RentUpdateViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private readonly RentViewModel _rentViewModel;
        public RentViewModel RentViewModel => _rentViewModel;
        private ValidationHelper _helper = new ValidationHelper();

        public double AnnualRent => _rentViewModel.AnnualRent;
        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;
        public double AnnualCostsTotal => _rentViewModel.AnnualCostsTotal;

        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;

        public double CostsTotal => ColdRent + ExtraCostsTotal;

        public bool HasErrors => ((INotifyDataErrorInfo)_helper).HasErrors;


        public DateTime StartDate
        {
            get { return _rentViewModel.StartDate; }
            set
            {
                _rentViewModel.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        public double ColdRent
        {
            get { return _rentViewModel.ColdRent; }
            set
            {
                _helper.ClearError();

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ColdRent));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ColdRent));
                }

                _rentViewModel.ColdRent = value;
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }

        public double ExtraCostsShared
        {
            get { return _rentViewModel.ExtraCostsShared; }
            set
            {
                _helper.ClearError();

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ExtraCostsShared));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ExtraCostsShared));
                }

                _rentViewModel.ExtraCostsShared = value;
                OnPropertyChanged(nameof(ExtraCostsShared));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double ExtraCostsHeating
        {
            get { return _rentViewModel.ExtraCostsHeating; }
            set
            {
                _helper.ClearError();

                if (Double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ExtraCostsHeating));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ExtraCostsHeating));
                }

                _rentViewModel.ExtraCostsHeating = value;
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public RentUpdateViewModel(RentViewModel rentViewModel)
        {
            _rentViewModel = rentViewModel;
            _helper.ErrorsChanged += (_, e) => this.ErrorsChanged?.Invoke(this, e);
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return ((INotifyDataErrorInfo)_helper).GetErrors(propertyName);
        }
    }
}
