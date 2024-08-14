/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for TenantSetupView
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
    public class TenantSetupViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        private readonly FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


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


        public ICommand DeleteTenantCommand { get; }

        #endregion commands


        // constructors
        #region constructors
        // active and inactive list for tenants or hiding and showing based on IsActive?
        public TenantSetupViewModel(FlatManagementViewModel flatManagementViewModel)
        {

            _FlatManagementViewModel = flatManagementViewModel;

            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            NewTenantCommand = new RelayCommand(p => AddTenant(p), (s) => true);
            DeleteTenantCommand = new RelayCommand(p => RemoveTenant(p), (s) => true);

            _FlatManagementViewModel.FlatViewModelChange += _FlatManagementViewModel_FlatViewModelChange;

            SelectFirstActiveTenant();
        }

        #endregion constructors


        // methods
        #region methods

        private void AddTenant(object p)
        {
            _FlatViewModel.Tenants.Add(
                new TenantViewModel(new Tenant() { Name = $"tenant_{new Random().Next(101)}" }));

            SelectedTenant = _FlatViewModel.Tenants.Last();

            OnPropertyChanged(nameof(SelectedTenant));
        }

        private void RemoveTenant(object p)
        {
            if (_FlatViewModel.Tenants.Contains(SelectedTenant))
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

                    _FlatViewModel.Tenants.Remove(SelectedTenant);

                    if (_FlatViewModel.Tenants.Count > 0)
                    {
                        SelectedTenant = _FlatViewModel.Tenants.Last();
                    }
                }

            }
        }


        private void SelectFirstActiveTenant()
        {
            if (_FlatViewModel != null && _FlatViewModel.Tenants.Count > 0)
            {
                foreach (TenantViewModel tenant in _FlatViewModel.Tenants)
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
            _FlatViewModel = _FlatManagementViewModel.SelectedItem;

            OnPropertyChanged(nameof(FlatViewModel));

            SelectFirstActiveTenant();
        }

        #endregion events


    }
}
// EOF