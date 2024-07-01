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
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.Views.Windows;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;


namespace SharedLivingCostCalculator.ViewModels
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
        public bool HasOtherCosts
        {
            get { return RentViewModel.HasOtherCosts; }
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
                        RentViewModel.HasOtherCosts = value;
                    }
                }
                else
                {
                    RentViewModel.HasOtherCosts = value;
                }

                OnPropertyChanged(nameof(HasOtherCosts));
                OnPropertyChanged(nameof(SetOtherCostsVisibility));

                RentConfigurationChange?.Invoke(this, EventArgs.Empty);
            }
        }


        private ValidationHelper _helper = new ValidationHelper();


        private bool _OtherCostsWindowActive;
        public bool OtherCostsWindowActive
        {
            get { return _OtherCostsWindowActive; }
            set
            {
                _OtherCostsWindowActive = value;

                if (Application.Current.MainWindow != null)
                {
                    var ownedWindows = Application.Current.MainWindow.OwnedWindows;

                    if (_OtherCostsWindowActive == false && ownedWindows != null)
                    {
                        foreach (Window wdw in Application.Current.MainWindow.OwnedWindows)
                        {
                            if (wdw.GetType() == typeof(OtherCostsView))
                            {
                                wdw.Close();
                            }
                        }
                    }
                }
                
                OnPropertyChanged(nameof(OtherCostsWindowActive));
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


        public bool SetOtherCostsVisibility => HasOtherCosts;


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


        public ICommand ShowOtherCostsCommand { get; }

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
            ShowOtherCostsCommand = new RelayCommand(p => ShowOtherCostsView(), (s) => true);


            if (_rentViewModel.HasBilling)
            {
                SetBillingVisibility = true;
            }
            if (_rentViewModel.HasOtherCosts)
            {
                HasOtherCosts = true;
            }
            if (_rentViewModel.HasDataLock)
            {
                DataLockCheckbox = true;
            }

            AnnualBillingWindowActive = false;
            OtherCostsWindowActive = false;

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
                billingWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                billingWindow.Closed += OwnedWindow_Closed;

                billingWindow.Show();
            }        
        }


        private void ShowOtherCostsView()
        {
            if (OtherCostsWindowActive)
            {
                var mainWindow = Application.Current.MainWindow;

                OtherCostsView otherCostsView = new OtherCostsView();

                otherCostsView.DataContext = new OtherCostsViewModel(RentViewModel);

                otherCostsView.Owner = mainWindow;
                otherCostsView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

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

            if (sender.GetType() == typeof(OtherCostsView))
            {
                OtherCostsWindowActive = false;
            }
            
            var mainWindow = Application.Current.MainWindow;
            mainWindow.Focus();
        }
        #endregion events


    }
}
// EOF