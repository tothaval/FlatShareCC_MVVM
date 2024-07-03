/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentUpdateViewModel  : BaseViewModel
 * 
 *  viewmodel for RentUpdateView
 *  
 *  allows for editing of RentViewModel
 *  
 *  is encapsulated within a RentManagementViewModel
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.Views.Windows;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class RentUpdateViewModel : BaseViewModel, IWindowOwner
    {

        // properties & fields
        #region properties

        private bool _AnnualBillingWindowActive;
        public bool AnnualBillingWindowActive
        {
            get { return _AnnualBillingWindowActive; }
            set
            {
                _AnnualBillingWindowActive = value;


                if (Application.Current.MainWindow != null)
                {
                    var ownedWindows = Application.Current.MainWindow.OwnedWindows;

                    if (_AnnualBillingWindowActive == false && ownedWindows != null)
                    {
                        foreach (Window wdw in Application.Current.MainWindow.OwnedWindows)
                        {
                            if (wdw.GetType() == typeof(BillingWindow))
                            {
                                wdw.Close();
                            }
                        }
                    }
                }


                OnPropertyChanged(nameof(AnnualBillingWindowActive));
            }
        }


        public double AnnualCostsTotal => _rentViewModel.AnnualCostsTotal;


        public double AnnualExtraCosts => _rentViewModel.AnnualExtraCosts;


        public double AnnualRent => _rentViewModel.AnnualRent;


        public double? BillingConsumedUnits => GetBillingConsumedUnits();


        public DateTime? BillingEndDate => GetBillingEndDate();


        public DateTime? BillingStartDate => GetBillingStartDate();


        public double ColdRent
        {
            get { return _rentViewModel.ColdRent.Cost; }
            set
            {
                _helper.ClearError();

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ColdRent));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ColdRent));
                }

                _rentViewModel.ColdRent.Cost = value;
                OnPropertyChanged(nameof(ColdRent));
                OnPropertyChanged(nameof(AnnualRent));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double CostsTotal => ColdRent + ExtraCostsTotal;


        private bool _DataLockCheckbox;
        public bool DataLockCheckbox
        {
            get { return _DataLockCheckbox; }
            set
            {
                _DataLockCheckbox = value;
                _rentViewModel.HasDataLock = _DataLockCheckbox;
                OnPropertyChanged(nameof(DataLockCheckbox));
                OnPropertyChanged(nameof(HasDataLock));
            }
        }


        public double ExtraCostsHeating
        {
            get { return _rentViewModel.HeatingCostsAdvance.Cost; }
            set
            {
                _helper.ClearError();

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ExtraCostsHeating));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ExtraCostsHeating));
                }

                _rentViewModel.HeatingCostsAdvance.Cost = value;
                OnPropertyChanged(nameof(ExtraCostsHeating));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double ExtraCostsShared
        {
            get { return _rentViewModel.FixedCostsAdvance.Cost; }
            set
            {
                _helper.ClearError();

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(ExtraCostsShared));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(ExtraCostsShared));
                }

                _rentViewModel.FixedCostsAdvance.Cost = value;
                OnPropertyChanged(nameof(ExtraCostsShared));
                OnPropertyChanged(nameof(ExtraCostsTotal));
                OnPropertyChanged(nameof(AnnualExtraCosts));
                OnPropertyChanged(nameof(CostsTotal));
            }
        }


        public double ExtraCostsTotal => ExtraCostsShared + ExtraCostsHeating;


        public bool HasCredits
        {
            get { return RentViewModel.HasCredits; }
            set
            {
                if (value == false)
                {
                    MessageBoxResult result = MessageBox.Show(
                    $"Warning: If you uncheck this checkbox, all associated data will be lost. Proceed?",
                    "Remove Accounting Factor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //RentViewModel.RemoveBilling();
                        RentViewModel.HasCredits = value;
                    }
                }
                else
                {
                    RentViewModel.HasCredits = value;
                }

                OnPropertyChanged(nameof(HasCredits));
                OnPropertyChanged(nameof(SetCreditVisibility));

                RentConfigurationChange?.Invoke(this, EventArgs.Empty);
            }
        }



        public bool HasDataLock => !DataLockCheckbox;


        public bool HasErrors => ((INotifyDataErrorInfo)_helper).HasErrors;


        private ValidationHelper _helper = new ValidationHelper();


        private bool _CostsWindowActive;
        public bool CostsWindowActive
        {
            get { return _CostsWindowActive; }
            set
            {
                _CostsWindowActive = value;

                if (Application.Current.MainWindow != null)
                {
                    var ownedWindows = Application.Current.MainWindow.OwnedWindows;

                    if (_CostsWindowActive == false && ownedWindows != null)
                    {
                        foreach (Window wdw in Application.Current.MainWindow.OwnedWindows)
                        {
                            if (wdw.GetType() == typeof(CostsView))
                            {
                                wdw.Close();
                            }
                        }
                    }
                }

                OnPropertyChanged(nameof(CostsWindowActive));
            }
        }


        private readonly RentViewModel _rentViewModel;
        public RentViewModel RentViewModel => _rentViewModel;


        private bool _SetBillingVisibility;
        public bool SetBillingVisibility
        {
            get { return _SetBillingVisibility; }
            set
            {
                if (value == false)
                {
                    MessageBoxResult result = MessageBox.Show(
                    $"Warning: If you uncheck this checkbox, all associated data will be lost. Proceed?",
                    "Remove Accounting Factor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        RentViewModel.RemoveBilling();
                        _SetBillingVisibility = value;
                    }
                }
                else
                {
                    _SetBillingVisibility = value;
                }

                OnPropertyChanged(nameof(SetBillingVisibility));

                RentConfigurationChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public bool SetCreditVisibility => HasCredits;


        public DateTime StartDate
        {
            get { return _rentViewModel.StartDate; }
            set
            {
                _rentViewModel.StartDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        #endregion properties


        // event properties & fields
        #region event handlers
        public event EventHandler RentConfigurationChange;
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion event handlers


        // commands
        #region commands

        public ICommand ShowBillingCommand { get; }


        public ICommand ShowCostsCommand { get; }

        #endregion commands


        // constructors
        #region constructors
        public RentUpdateViewModel(FlatViewModel flatViewModel, RentViewModel rentViewModel)
        {
            _rentViewModel = rentViewModel;

            if (_rentViewModel == null)
            {
                _rentViewModel = new RentViewModel(flatViewModel, new Rent());
            }

            _helper.ErrorsChanged += (_, e) => ErrorsChanged?.Invoke(this, e);


            ShowBillingCommand = new RelayCommand(p => ShowBillingView(), (s) => true);
            ShowCostsCommand = new RelayCommand(p => ShowCostsView(), (s) => true);


            if (_rentViewModel.HasBilling)
            {
                SetBillingVisibility = true;
            }
            if (_rentViewModel.HasDataLock)
            {
                DataLockCheckbox = true;
            }

            AnnualBillingWindowActive = false;
            CostsWindowActive = false;

            OnPropertyChanged(nameof(HasDataLock));
        }
        #endregion constructors


        // methods
        #region methods

        private double? GetBillingConsumedUnits()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.TotalHeatingUnitsConsumption;
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


        private DateTime? GetBillingStartDate()
        {
            if (RentViewModel.BillingViewModel != null)
            {
                return RentViewModel.BillingViewModel.StartDate;
            }
            return null;
        }


        public IEnumerable GetErrors(string? propertyName)
        {
            return ((INotifyDataErrorInfo)_helper).GetErrors(propertyName);
        }


        private void ShowBillingView()
        {
            if (AnnualBillingWindowActive)
            {
                var mainWindow = Application.Current.MainWindow;

                BillingWindow billingWindow = new BillingWindow();

                if (RentViewModel.BillingViewModel == null)
                {
                    Billing billing = new Billing(RentViewModel.GetFlatViewModel());

                    RentViewModel.BillingViewModel = new BillingViewModel(RentViewModel.GetFlatViewModel(), billing);
                }

                billingWindow.DataContext = new BillingPeriodViewModel(RentViewModel.GetFlatViewModel(), RentViewModel.BillingViewModel);

                billingWindow.Owner = mainWindow;
                billingWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                billingWindow.Closed += OwnedWindow_Closed;

                billingWindow.Show();
            }
        }


        private void ShowCostsView()
        {
            if (CostsWindowActive)
            {
                var mainWindow = Application.Current.MainWindow;

                CostsView otherCostsView = new CostsView();

                otherCostsView.DataContext = new CostsViewModel(RentViewModel);

                otherCostsView.Owner = mainWindow;
                otherCostsView.WindowStartupLocation = WindowStartupLocation.CenterOwner;

                otherCostsView.Closed += OwnedWindow_Closed;

                otherCostsView.Show();
            }
        }
        #endregion methods


        // events
        #region events
        public void OwnedWindow_Closed(object? sender, EventArgs e)
        {
            if (sender.GetType() == typeof(BillingWindow))
            {
                AnnualBillingWindowActive = false;
            }

            if (sender.GetType() == typeof(CostsView))
            {
                CostsWindowActive = false;
            }

            var mainWindow = Application.Current.MainWindow;
            mainWindow.Focus();
        }
        #endregion events


    }
}
// EOF