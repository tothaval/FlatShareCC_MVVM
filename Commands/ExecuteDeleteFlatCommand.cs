/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ExecuteDeleteFlatCommand 
 * 
 *  command for removing a flatviewmodel
 *  instance from the _FlatCollection   
 *  ObservableCollection<_FlatViewModel>
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

        // Collections
        #region Collections

        private readonly ObservableCollection<FlatViewModel> _FlatCollection;


        private readonly FlatManagementViewModel _FlatManagementViewModel;

        #endregion


        // Constructors
        #region Constructors

        public ExecuteDeleteFlatCommand(ObservableCollection<FlatViewModel> flatCollection, FlatManagementViewModel flatManagementViewModel)
        {
            _FlatCollection = flatCollection;
            _FlatManagementViewModel = flatManagementViewModel;
        }

        #endregion


        // Methods
        #region Methods

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
                        _FlatCollection.Remove(item);       
                    }
                }
            }            
        }

        #endregion


    }
}
// EOF