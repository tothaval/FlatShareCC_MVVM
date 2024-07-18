/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ConsumptionItemViewModel  : BaseViewModel
 * 
 *  viewmodel for ConsumptionItem model
 */
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;

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


        /// <summary>
        /// heating units not consumed by the rooms but by the shared area
        /// are represented by this value, the SharedConsumption will be split
        /// equally onto the number of rooms and the share will be added to 
        /// the room consumption.
        /// </summary>
        public double SharedConsumption => ConsumedUnits - TotalRoomUnits;


        /// <summary>
        /// returns SharedConsumption / ConsumedUnits * 100.
        /// </summary>
        public double SharedConsumptionPercentage => SharedConsumption / ConsumedUnits * 100;


        /// <summary>
        /// heating units consumed by the rooms, but not by the shared area
        /// are represented by this value
        /// </summary>
        public double TotalRoomUnits => CalculateTotalRoomUnits();



        public ConsumptionItemViewModel(ConsumptionItem consumptionItem, BillingViewModel billingViewModel)
        {
            _ConsumptionItem = consumptionItem;
            _BillingViewModel = billingViewModel;

            RoomConsumptionViewModels = new ObservableCollection<RoomConsumptionViewModel>();

            if (_ConsumptionItem.RoomConsumptions.Count > 0)
            {
                foreach (RoomConsumption item in _ConsumptionItem.RoomConsumptions)
                {
                    RoomConsumptionViewModels.Add(new RoomConsumptionViewModel(item, this));
                }
            }
            else
            {
                foreach (RoomViewModel item in _BillingViewModel.FlatViewModel.Rooms)
                {
                    RoomConsumptionViewModels.Add(new RoomConsumptionViewModel(new RoomConsumption(item.GetRoom, 0.0), this));
                }
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
// EOF