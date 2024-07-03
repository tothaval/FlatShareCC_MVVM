/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ExecuteDeleteFlatCommand 
 * 
 *  command for removing a flatviewmodel
 *  instance from the _flatCollection   
 *  ObservableCollection<FlatViewModel>
 */
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace SharedLivingCostCalculator.Commands
{
    class ExecuteDeleteFlatCommand : BaseCommand
    {

        // collections
        #region collections

        private readonly FlatManagementViewModel _FlatManagementViewModel;

        private readonly ObservableCollection<FlatViewModel> _flatCollection;

        #endregion collections


        // constructors
        #region constructors

        public ExecuteDeleteFlatCommand(ObservableCollection<FlatViewModel> flatCollection, FlatManagementViewModel flatManagementViewModel)
        {
            _flatCollection = flatCollection;
            _FlatManagementViewModel = flatManagementViewModel;
        }

        #endregion constructors


        // methods
        #region methods

        public override void Execute(object? parameter)
        {
            IList selection = (IList)parameter;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you want to delete selected flat(s)?",
                    "Remove Flat(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<FlatViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _flatCollection.Remove(item);       
                    }
                }
            }            
        }

        #endregion methods


    }
}
// EOF