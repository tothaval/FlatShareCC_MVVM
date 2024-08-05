/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RoomConsumptionViewModel  : BaseViewModel
 * 
 *  viewmodel for RoomConsumption model
 */
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class RoomConsumptionViewModel : BaseViewModel
    {
        
        // Properties & Fields
        #region Properties & Fields

        private ConsumptionItemViewModel _ConsumptionItemViewModel;
        public ConsumptionItemViewModel ConsumptionItemViewModel => _ConsumptionItemViewModel;


        public string ConsumptionCause => _ConsumptionItemViewModel.ConsumptionCause;


        public double ConsumptionCost { get; set; } = 0.0;


        public double ConsumptionPercentage => GetPercentage();


        public double ConsumptionValue
        {
            get { return _RoomConsumption.ConsumptionValue; }
            set
            {
                _RoomConsumption.ConsumptionValue = value;

                OnPropertyChanged(nameof(ConsumptionValue));
                OnPropertyChanged(nameof(SharedConsumption));
                OnPropertyChanged(nameof(ConsumptionPercentage));
            }
        }


        public double SharedConsumption => _ConsumptionItemViewModel.RoomSharedConsumption;


        public double TotalConsumptionValue => ConsumptionValue + SharedConsumption;


        public double RoomArea => _RoomConsumption.Room.RoomArea;


        private RoomConsumption _RoomConsumption;
        public RoomConsumption RoomConsumption => _RoomConsumption;


        public string RoomName => _RoomConsumption.Room.RoomName;

        #endregion

        // Constructors
        #region Constructors

        public RoomConsumptionViewModel(RoomConsumption roomConsumption, ConsumptionItemViewModel consumptionItemViewModel)
        {
            _RoomConsumption = roomConsumption;

            _ConsumptionItemViewModel = consumptionItemViewModel;
        }

        #endregion


        // Methods
        #region Methods

        private double GetPercentage()
        {
            return TotalConsumptionValue / ConsumptionItemViewModel.ConsumedUnits * 100;
        } 

        #endregion


    }
}
// EOF