﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CostDisplayViewModel  : BaseViewModel
 * 
 *  viewmodel for CostDisplayView
 *  
 *  shows either a RentCostsViewModel or BillingCostsViewModel
 *  depending on constructor
 */
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class CostDisplayViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private readonly AccountingViewModel _AccountingViewModel;
        public AccountingViewModel Accounting => _AccountingViewModel;


        private ICostDisplay _ActiveViewModel;
        public ICostDisplay ActiveViewModel
        {
            get { return _ActiveViewModel; }
            set
            {
                _ActiveViewModel = value;

                OnPropertyChanged(nameof(ActiveViewModel));
            }
        }


        private bool _BillingSelected;
        public bool BillingSelected
        {
            get { return _BillingSelected; }
            set
            {
                _BillingSelected = value;

                if (_BillingSelected && _billingViewModel != null && _flatViewModel != null)
                {
                    ActiveViewModel = (ICostDisplay)new BillingCostsViewModel(_billingViewModel, _flatViewModel);
                    
                    RentSelected = false;
                }
                else if (!_RentSelected && !BillingSelected)
                {
                    RentSelected = true;
                }


                _FlatManagementViewModel.ShowCostsBillingSelected = value;


                OnPropertyChanged(nameof(BillingSelected));
            }
        }


        private BillingViewModel? _billingViewModel;


        private readonly FlatManagementViewModel _FlatManagementViewModel;
        public FlatManagementViewModel FlatManagementViewModel => _FlatManagementViewModel;



        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        public bool HasBilling => _flatViewModel.AnnualBillings.Count > 0;


        private bool _RentSelected;
        public bool RentSelected
        {
            get { return _RentSelected; }
            set
            {
                _RentSelected = value;

                if (_RentSelected && _rentViewModel != null && _flatViewModel != null)
                {
                    ActiveViewModel = (ICostDisplay)new RentCostsViewModel(_rentViewModel, _flatViewModel);

                    BillingSelected = false;
                }
                else if(!_RentSelected && !BillingSelected)
                {
                    BillingSelected = true;
                }
                OnPropertyChanged(nameof(RentSelected));
            }
        }


        private RentViewModel? _rentViewModel;

        #endregion properties & fields


        // constructors
        #region constructors

        public CostDisplayViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            _AccountingViewModel = flatManagementViewModel.Accounting;

            _AccountingViewModel.AccountingChanged += _AccountingViewModel_AccountingChanged;

            //_AccountingViewModel.FlatManagement.FlatViewModelChange += FlatManagement_FlatViewModelChange;

            Update();
        }
        #endregion constructors


        // methods
        #region methods

        public void Update()
        {
            if (_AccountingViewModel.FlatViewModel != null)
            {
                _flatViewModel = _AccountingViewModel.FlatViewModel;
            }

            if (_AccountingViewModel.Rents.SelectedValue != null)
            {
                _rentViewModel = _AccountingViewModel.Rents.SelectedValue;

                ActiveViewModel = (ICostDisplay)new RentCostsViewModel(_rentViewModel, _flatViewModel);
            }

            if (_flatViewModel != null && _flatViewModel.AnnualBillings != null )
            {
                if (_FlatManagementViewModel.AnnualBilling != null && _FlatManagementViewModel.AnnualBilling.SelectedValue != null)
                {
                    _billingViewModel = _FlatManagementViewModel.AnnualBilling.SelectedValue;

                    _billingViewModel.UpdateCosts(); 
                }
            }
            else
            {
                _billingViewModel = null;
            }


            if (_AccountingViewModel.Rents.SelectedValue != null
                && _flatViewModel != null && _flatViewModel.AnnualBillings.Count > 0)
            {
                if (_FlatManagementViewModel.ShowCostsBillingSelected)
                {
                    BillingSelected = true;
                }
            }
            else
            {
                RentSelected = true;
            }


            OnPropertyChanged(nameof(FlatViewModel));
            OnPropertyChanged(nameof(ActiveViewModel));
            OnPropertyChanged(nameof(HasBilling));
        }

        #endregion methods


        // events
        #region events

        private void _AccountingViewModel_AccountingChanged(object? sender, EventArgs e)
        {
            _AccountingViewModel.Rents.SelectedItemChange -= Rents_SelectedItemChange;

            _AccountingViewModel.Rents.SelectedItemChange += Rents_SelectedItemChange;

            Update();
        }


        private void FlatManagement_FlatViewModelChange(object? sender, EventArgs e)
        {
            _AccountingViewModel.FlatManagement.SelectedItem.PropertyChanged += SelectedItem_PropertyChanged;

            Update();
        }


        private void Rents_SelectedItemChange(object? sender, EventArgs e)
        {
            Update();
        }


        private void SelectedItem_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Update();
        }


        private void UpdateViewModel_RentConfigurationChange(object? sender, EventArgs e)
        {
            Update();
        }

        #endregion events


    }
}
// EOF