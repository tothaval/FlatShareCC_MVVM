using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class ConsumptionItemViewModel : BaseViewModel
    {

        private BillingViewModel _BillingViewModel;


        private ConsumptionItem _ConsumptionItem;
        public ConsumptionItem ConsumptionItem => _ConsumptionItem;


        public double ConsumedUnits
        {
            get { return _ConsumptionItem.ConsumedUnits; }
            set
            {
                _ConsumptionItem.ConsumedUnits = value;
                OnPropertyChanged(nameof(ConsumedUnits));
            }
        }

        public string ConsumptionCause
        {
            get { return _ConsumptionItem.ConsumptionCause.TransactionItem; }
            set
            {
                _ConsumptionItem.ConsumptionCause.TransactionItem = value;
                OnPropertyChanged(nameof(ConsumptionCause));
            }
        }


        private ObservableCollection<RoomConsumptionViewModel> _RoomConsumptionViewModels;
        public ObservableCollection<RoomConsumptionViewModel> RoomConsumptionViewModels
        {
            get { return _RoomConsumptionViewModels; }
            set
            {
                _RoomConsumptionViewModels = value;
                OnPropertyChanged(nameof(RoomConsumptionViewModels));
            }
        }

        public double SharedConsumption => ConsumedUnits - TotalRoomUnits;

        public double SharedConsumptionPercentage => SharedConsumption / ConsumedUnits * 100;

        public double TotalRoomUnits => CalculateTotalRoomUnits();



        public ConsumptionItemViewModel(ConsumptionItem consumptionItem, BillingViewModel billingViewModel)
        {
            _ConsumptionItem = consumptionItem;
            _BillingViewModel = billingViewModel;

            RoomConsumptionViewModels = new ObservableCollection<RoomConsumptionViewModel>();


            foreach (RoomViewModel item in _BillingViewModel.FlatViewModel.Rooms)
            {
                RoomConsumptionViewModels.Add(new RoomConsumptionViewModel(new RoomConsumption(item.GetRoom, 0.0)));
            }

        }


        private double CalculateTotalRoomUnits()
        {
            double totalRoomUnits = 0.0;

            foreach (RoomConsumptionViewModel item in RoomConsumptionViewModels)
            {
                totalRoomUnits += item.ConsumptionValue;
            }

            return totalRoomUnits;
        }


    }
}
