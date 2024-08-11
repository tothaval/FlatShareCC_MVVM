/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantAssignmentViewModel  : BaseViewModel
 * 
 *  viewmodel for TenantAssignementView
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
    public class TenantAssignmentViewModel : BaseViewModel
    {   // properties & fields
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

        #endregion properties & fields


        // collections
        #region collections

        #endregion collections


        // commands
        #region commands

        public ICommand NewTenantConfigurationCommand { get; }


        public ICommand DeleteTenantConfigurationCommand { get; }

        #endregion commands


        // constructors
        #region constructors
        // active and inactive list for tenants or hiding and showing based on IsActive?
        public TenantAssignmentViewModel(FlatManagementViewModel flatManagementViewModel)
        {

            _FlatManagementViewModel = flatManagementViewModel;

            _flatViewModel = _FlatManagementViewModel.SelectedItem;

            NewTenantConfigurationCommand = new RelayCommand(p => AddTenantConfiguration(p), (s) => true);
            DeleteTenantConfigurationCommand = new RelayCommand(p => RemoveTenantConfiguration(p), (s) => true);

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;
        }

        #endregion constructors


        // methods
        #region methods

        private void AddTenantConfiguration(object p)
        {
            _flatViewModel.TenantConfigurations.Add(new TenantConfigurationViewModel(
                new TenantConfiguration(_flatViewModel.Tenants, _flatViewModel)));

            SelectedTenantConfiguration = _flatViewModel.TenantConfigurations.Last();

            OnPropertyChanged(nameof(SelectedTenantConfiguration));
        }


        private void RemoveTenantConfiguration(object p)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you want to delete the selected tenant configuration?\n\n\n",
                "Delete Tenant Configuration", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                if (_flatViewModel.TenantConfigurations.Contains(SelectedTenantConfiguration))
                {
                    _flatViewModel.TenantConfigurations.Remove(SelectedTenantConfiguration);

                    if (_flatViewModel.TenantConfigurations.Count > 0)
                    {
                        SelectedTenantConfiguration = _flatViewModel.TenantConfigurations.Last();
                    }
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
        }

        #endregion events


    }
}
// EOF