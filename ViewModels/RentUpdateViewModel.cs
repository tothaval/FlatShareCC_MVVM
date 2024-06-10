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
    class RentUpdateViewModel : BaseViewModel
    {
        private readonly RentViewModel _rentViewModel;
        public RentViewModel RentViewModel => _rentViewModel;

        public bool SetBillingVisibility => HasBilling;
        
        private bool _HasBilling;

        public bool HasBilling
        {
            get { return _HasBilling; }
            set
            {
                _HasBilling = value;
                OnPropertyChanged(nameof(HasBilling));
                OnPropertyChanged(nameof(SetBillingVisibility));
            }
        }

        private ValidationHelper _helper = new ValidationHelper();
        public bool HasErrors => ((INotifyDataErrorInfo)_helper).HasErrors;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        public IEnumerable GetErrors(string? propertyName)
        {
            return ((INotifyDataErrorInfo)_helper).GetErrors(propertyName);
        }

        public double AnnualRent => _rentViewModel.AnnualRent;
        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;
        public double AnnualCostsTotal => _rentViewModel.AnnualCostsTotal;

        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;

        public double CostsTotal => ColdRent + ExtraCostsTotal;

        public DateTime? BillingStartDate => GetBillingStartDate();
        public DateTime? BillingEndDate => GetBillingEndDate();
        public double? BillingConsumedUnits => GetBillingConsumedUnits();

        public ICommand NewBillingCommand { get; }

        public ICollectionView BillingViewModels { get; set; }

        private BillingViewModel _SelectedItem;
        public BillingViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;

                RentViewModel.BillingViewModel = SelectedItem;

                OnPropertyChanged(nameof(SelectedItem));
            }
        }


        private DateTime? GetBillingStartDate()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.StartDate;
            }
            return null;
        }

        private DateTime? GetBillingEndDate()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.EndDate;
            }
            return null;
        }

        private double? GetBillingConsumedUnits()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.TotalHeatingUnitsConsumption;
            }
            return null;
        }



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
            get { return _rentViewModel.FixedCostsAdvance; }
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

                _rentViewModel.FixedCostsAdvance = value;
                OnPropertyChanged(nameof(ExtraCostsShared));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double ExtraCostsHeating
        {
            get { return _rentViewModel.HeatingCostsAdvance; }
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

                _rentViewModel.HeatingCostsAdvance = value;
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public RentUpdateViewModel(FlatViewModel flatViewModel, RentViewModel rentViewModel)
        {
            _rentViewModel = rentViewModel;


            if (_rentViewModel == null)
            {
                _rentViewModel = new RentViewModel(flatViewModel, new Rent());
            }

            _helper.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);

            BillingViewModels = CollectionViewSource.GetDefaultView(RentViewModel.GetFlatViewModel().BillingPeriods);
            BillingViewModels.SortDescriptions.Add(new SortDescription("StartDate", ListSortDirection.Descending));

            NewBillingCommand = new RelayCommand(p => AddBilling(), (s) => true);

            if (RentViewModel.HasBilling)
            {
                SelectedItem = RentViewModel.BillingViewModel;
                HasBilling = true;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private void AddBilling()
        {
            RentViewModel.BillingViewModel = RentViewModel.GetFlatViewModel().AddBilling(RentViewModel);
            SelectedItem = RentViewModel.BillingViewModel;
        }
    }
}
