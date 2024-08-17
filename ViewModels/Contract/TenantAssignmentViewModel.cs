/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantAssignmentViewModel  : BaseViewModel
 * 
 *  viewmodel for TenantAssignementView
 *  
 *  allows editing of TenantViewModel
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Windows;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class TenantAssignmentViewModel : BaseViewModel
    {   

        // properties & fields
        #region properties & fields

        private readonly FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


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

            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            NewTenantConfigurationCommand = new RelayCommand(p => AddTenantConfiguration(p), (s) => true);
            DeleteTenantConfigurationCommand = new RelayCommand(p => RemoveTenantConfiguration(p), (s) => true);

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;
        }

        #endregion constructors


        // methods
        #region methods

        private void AddTenantConfiguration(object p)
        {
            _FlatViewModel.TenantConfigurations.Add(new TenantConfigurationViewModel(
                new TenantConfiguration(_FlatViewModel.Tenants, _FlatViewModel)));

            SelectedTenantConfiguration = _FlatViewModel.TenantConfigurations.Last();

            OnPropertyChanged(nameof(SelectedTenantConfiguration));
        }


        private void RemoveTenantConfiguration(object p)
        {
            MessageBoxResult result = MessageBox.Show(
                $"Do you want to delete the selected tenant configuration?\n\n\n",
                "Delete Tenant Configuration", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                if (_FlatViewModel.TenantConfigurations.Contains(SelectedTenantConfiguration))
                {
                    _FlatViewModel.TenantConfigurations.Remove(SelectedTenantConfiguration);

                    if (_FlatViewModel.TenantConfigurations.Count > 0)
                    {
                        SelectedTenantConfiguration = _FlatViewModel.TenantConfigurations.Last();
                    }
                }

            }
        }

        #endregion methods


        // events
        #region events

        private void _FlatManagementViewModel_FlatViewModelChange(object? sender, EventArgs e)
        {
            _FlatViewModel = _FlatManagementViewModel.SelectedItem;


            if (_FlatViewModel != null && _FlatViewModel.TenantConfigurations.Count > 0)
            {
                SelectedTenantConfiguration = _FlatViewModel.TenantConfigurations.First();
            }


            OnPropertyChanged(nameof(FlatViewModel));

            OnPropertyChanged(nameof(SelectedTenantConfiguration));
        }

        #endregion events


    }
}
// EOF