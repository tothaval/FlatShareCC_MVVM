using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    public class OtherCostsViewModel : BaseViewModel
    {
        private readonly RentViewModel _RentViewModel;


        public ICommand AddOtherCostItemCommand { get; }
        public ICommand RemoveOtherCostItemCommand { get; }

        public ObservableCollection<OtherCostItemViewModel> OtherCostItems => _RentViewModel.OtherCosts;


        public OtherCostsViewModel(RentViewModel rentViewModel)
        {

            AddOtherCostItemCommand = new RelayCommand((s) => AddOtherCostItem(s), (s) => true);
            RemoveOtherCostItemCommand = new RelayCommand((s) => RemoveOtherCostItem(s), (s) => true);

            _RentViewModel = rentViewModel;

        }


        private void AddOtherCostItem(object s)
        {
            OtherCostItemViewModel otherCostItemViewModel = new OtherCostItemViewModel(new OtherCostItem());

            _RentViewModel.OtherCosts.Add(otherCostItemViewModel);

            OnPropertyChanged(nameof(OtherCostItems));
        }


        private void RemoveOtherCostItem(object s)
        {
            IList selection = (IList)s;

            if (selection != null)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Do you wan't to delete selected other costs?",
                    "Remove Other Cost(s)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var selected = selection.Cast<OtherCostItemViewModel>().ToArray();

                    foreach (var item in selected)
                    {
                        _RentViewModel.OtherCosts.Remove(item);
                        OnPropertyChanged(nameof(OtherCostItems));
                    }
                }
            }
        }


    }
}
