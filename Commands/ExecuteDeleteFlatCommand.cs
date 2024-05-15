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
    class ExecuteDeleteFlatCommand : BaseCommand
    {
        private ObservableCollection<FlatViewModel> _flatCollection;

        public ExecuteDeleteFlatCommand(ObservableCollection<FlatViewModel> flatCollection)
        {
                _flatCollection = flatCollection;
        }


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
                        $"{viewModel.Area}m²; {viewModel.Rooms} room(s)?",
                        "Remove Flat", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {                        
                        _flatCollection.Remove(viewModel);
                    }
                }
            }
        }
    }
}
