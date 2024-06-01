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
            _flatCollection.Add(
                new FlatViewModel(
                    new Flat(
                        _flatCollection.Count,
                        _viewModel.FlatSetup.Address,
                        _viewModel.FlatSetup.Area,
                        _viewModel.FlatSetup.RoomCount,
                        _viewModel.FlatSetup.Rooms,
                        _viewModel.FlatSetup.Details)));

            _flatSetupView.Close();
        }
    }
}
