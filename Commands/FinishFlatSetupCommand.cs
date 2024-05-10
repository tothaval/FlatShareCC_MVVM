using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WGMietkosten.Exceptions;
using WGMietkosten.Models;
using WGMietkosten.Navigation;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Commands
{
    class FinishFlatSetupCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly FlatSetupViewModel _flatSetupViewModel;

        public FinishFlatSetupCommand(NavigationStore navigationStore, FlatSetupViewModel flatSetupViewModel)
        {
            _navigationStore = navigationStore;
            _flatSetupViewModel = flatSetupViewModel;

            _flatSetupViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }


        public override bool CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(_flatSetupViewModel.Address) &&
                !string.IsNullOrEmpty(_flatSetupViewModel.Details) &&
                !double.IsNaN(_flatSetupViewModel.Area) &&
                _flatSetupViewModel.Rooms > 0 &&
                base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            FlatSetup flatSetup = new FlatSetup(
                _navigationStore.FlatManager.GetFlats().Count() + 1,
                _flatSetupViewModel.Address,
                _flatSetupViewModel.Details,
                _flatSetupViewModel.Area,
                _flatSetupViewModel.Rooms,
                _flatSetupViewModel.Tenants);

            try
            {
                FlatManagementViewModel flatManagementViewModel = new FlatManagementViewModel(_navigationStore);

                flatManagementViewModel.AddFlatSetup(flatSetup);

                _navigationStore.CurrentViewModel = flatManagementViewModel;
            }
            catch (FlatManagementConflictException)
            {
                MessageBox.Show("Flat already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FlatSetupViewModel.Address) ||
                e.PropertyName == nameof(FlatSetupViewModel.Details) ||
                e.PropertyName == nameof(FlatSetupViewModel.Area) ||
                e.PropertyName == nameof(FlatSetupViewModel.Rooms))
            {
                OnCanExecutedChanged();
            }

        }
    }
}
