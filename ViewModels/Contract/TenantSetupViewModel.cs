/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for TenantSetupView
 *  
 *  allows editing of TenantViewModel
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class TenantSetupViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private readonly FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;


        private TenantConfigurationViewModel _SelectedTenantConfiguration;
        public TenantConfigurationViewModel SelectedTenantConfiguration
        {
            get { return _SelectedTenantConfiguration; }
            set
            {
                _SelectedTenantConfiguration = value;

                if (_SelectedTenantConfiguration != null)
                {
                    _SelectedTenantConfiguration.GetActiveTenants();

                    OnPropertyChanged(nameof(SelectedTenantConfiguration));
                }
            }
        }


        private TenantViewModel _SelectedTenant;
        public TenantViewModel SelectedTenant
        {
            get { return _SelectedTenant; }
            set
            {
                _SelectedTenant = value;
                OnPropertyChanged(nameof(SelectedTenant));
            }
        }

        #endregion properties & fields


        // collections
        #region collections

        #endregion collections


        // commands
        #region commands

        public ICommand NewTenantCommand { get; }


        public ICommand NewTenantConfigurationCommand { get; }


        public ICommand DeleteTenantCommand { get; }


        public ICommand DeleteTenantConfigurationCommand { get; }

        #endregion commands


        // constructors
        #region constructors
        // active and inactive list for tenants or hiding and showing based on IsActive?
        public TenantSetupViewModel(FlatManagementViewModel flatManagementViewModel)
        {

            _FlatManagementViewModel = flatManagementViewModel;

            _flatViewModel = _FlatManagementViewModel.SelectedItem;

            NewTenantCommand = new RelayCommand(p => AddTenant(p), (s) => true);
            NewTenantConfigurationCommand = new RelayCommand(p => AddTenantConfiguration(p), (s) => true);
            DeleteTenantCommand = new RelayCommand(p => RemoveTenant(p), (s) => true);
            DeleteTenantConfigurationCommand = new RelayCommand(p => RemoveTenantConfiguration(p), (s) => true);


            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;

            SelectFirstActiveTenant();
        }

        #endregion constructors


        // methods
        #region methods

        private void AddTenant(object p)
        {
            _flatViewModel.Tenants.Add(
                new TenantViewModel(new Tenant() { Name = $"tenant_{new Random().Next(101)}" }));

            SelectedTenant = _flatViewModel.Tenants.Last();

            OnPropertyChanged(nameof(SelectedTenant));
        }


        private void AddTenantConfiguration(object p)
        {
            _flatViewModel.TenantConfigurations.Add(new TenantConfigurationViewModel(
                new TenantConfiguration(_flatViewModel.Tenants, _flatViewModel)));

            SelectedTenantConfiguration = _flatViewModel.TenantConfigurations.Last();

            OnPropertyChanged(nameof(SelectedTenantConfiguration));
        }

        private void RemoveTenant(object p)
        {
            if (_flatViewModel.Tenants.Contains(SelectedTenant))
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete the selected tenant?\n\n\n" +
                    $"If the tenant is moving out, you can uncheck\n" +
                    $"'Is Active' combobox and enter the date to\n" +
                    $"preserve the tenant data for future uses.\n\n" +
                    $"Do you want to delete the selected tenant?",
                    "Remove Tenant", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    _flatViewModel.Tenants.Remove(SelectedTenant);

                    if (_flatViewModel.Tenants.Count > 0)
                    {
                        SelectedTenant = _flatViewModel.Tenants.Last();
                    }
                }

            }
        }

        private void RemoveTenantConfiguration(object p)
        {
            if (_flatViewModel.Tenants.Contains(SelectedTenant))
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete the selected tenant configuration?\n\n\n",
                    "Delete Tenant Configuration", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    _flatViewModel.TenantConfigurations.Remove(SelectedTenantConfiguration);

                    if (_flatViewModel.TenantConfigurations.Count > 0)
                    {
                        SelectedTenantConfiguration = _flatViewModel.TenantConfigurations.Last();
                    }
                }

            }
        }

        private void SelectFirstActiveTenant()
        {
            if (_flatViewModel != null && _flatViewModel.Tenants.Count > 0)
            {
                foreach (TenantViewModel tenant in _flatViewModel.Tenants)
                {
                    if (!tenant.IsActive)
                    {
                        continue;
                    }

                    SelectedTenant = tenant;

                    break;
                }
            }
        }

        #endregion methods


        // events
        #region events

        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _flatViewModel = _FlatManagementViewModel.SelectedItem;

            OnPropertyChanged(nameof(FlatViewModel));

            OnPropertyChanged(nameof(SelectedTenantConfiguration));

            SelectFirstActiveTenant();
        }

        #endregion events


    }
}
// EOF