using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SharedLivingCostCalculator.Commands
{
    internal class FlatSetupCommand : BaseCommand
    {
        private ObservableCollection<FlatViewModel> _flatCollection;
        private FlatSetupViewModel _viewModel;
        private INavigationService _navigationService;

        public FlatSetupCommand(ObservableCollection<FlatViewModel> flatCollection, FlatSetupViewModel viewModel, INavigationService navigationService)
        {
            _flatCollection = flatCollection;
            _viewModel = viewModel;
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _flatCollection.Add(
                new FlatViewModel(
                    new Flat(
                        _flatCollection.Count,
                        _viewModel.FlatSetup.Address,
                        _viewModel.FlatSetup.Area,
                        _viewModel.FlatSetup.Rooms,
                        _viewModel.FlatSetup.rooms,
                        _viewModel.FlatSetup.Details)));
            
            _navigationService.ChangeView();            
        }
    }
}
