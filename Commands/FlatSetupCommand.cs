/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FlatSetupCommand 
 * 
 *  adds a new FlatViewModel instance
 *  to the _flatCollection ObservableCollection<FlatViewModel>
 */
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.Views;
using System.Collections.ObjectModel;

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
                    flatViewModel,
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
// EOF