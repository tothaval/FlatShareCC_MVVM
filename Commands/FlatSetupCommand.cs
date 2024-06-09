using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.Views;
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
        private readonly ObservableCollection<FlatViewModel> _flatCollection;
        private readonly FlatSetupViewModel _viewModel;
        private readonly FlatSetupView _flatSetupView;

        public FlatSetupCommand(ObservableCollection<FlatViewModel> flatCollection, FlatSetupViewModel viewModel, FlatSetupView flatSetupView)
        {
            _flatCollection = flatCollection;
            _viewModel = viewModel;
            _flatSetupView = flatSetupView;
        }

        public override void Execute(object? parameter)
        {
            FlatViewModel flatViewModel = new FlatViewModel(
                new Flat(
                        _flatCollection.Count,
                        _viewModel.FlatSetup.Address,
                        _viewModel.FlatSetup.Area,
                        _viewModel.FlatSetup.RoomCount,
                        _viewModel.FlatSetup.Rooms,
                        _viewModel.FlatSetup.Details));

            RentViewModel rentViewModel = new RentViewModel(
                flatViewModel,
                new Rent(
                    0,
                    DateTime.Now,
                    0.0,
                    0.0,
                    0.0
                    )
                );                       

            flatViewModel.RentUpdates.Add(rentViewModel);


            _flatCollection.Add(flatViewModel);

            _flatSetupView.Close();
        }
    }
}
