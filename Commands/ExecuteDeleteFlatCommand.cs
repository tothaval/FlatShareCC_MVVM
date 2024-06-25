/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ExecuteDeleteFlatCommand 
 * 
 *  command for removing a flatviewmodel
 *  instance from the _flatCollection   
 *  ObservableCollection<FlatViewModel>
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Windows;

namespace SharedLivingCostCalculator.Commands
{
    class ExecuteDeleteFlatCommand : BaseCommand
    {

        // collections
        #region collections

        private readonly ObservableCollection<FlatViewModel> _flatCollection;

        #endregion collections


        // constructors
        #region constructors

        public ExecuteDeleteFlatCommand(ObservableCollection<FlatViewModel> flatCollection)
        {
            _flatCollection = flatCollection;
        }

        #endregion constructors


        // methods
        #region methods

        public override void Execute(object? parameter)
        {
            if (parameter != null)
            {
                if (parameter.GetType() == typeof(FlatViewModel))
                {
                    FlatViewModel viewModel = (FlatViewModel)parameter;

                    MessageBoxResult result = MessageBox.Show(
                        $"Do you wan't to delete flat id: {viewModel.ID}\n" +
                        $"{viewModel.Address}; {viewModel.Details};\n" +
                        $"{viewModel.Area}m²; {viewModel.RoomCount} room(s)?",
                        "Remove Flat", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _flatCollection.Remove(viewModel);
                    }
                }
            }
        }

        #endregion methods


    }
}
// EOF