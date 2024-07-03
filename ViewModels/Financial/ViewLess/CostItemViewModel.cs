/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CostItemViewModel  : BaseViewModel
 * 
 *  data model class
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
{
    public class CostItemViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public double Cost
        {
            get { return _CostItem.Cost; }
            set
            {
                _CostItem.Cost = value;
                OnPropertyChanged(nameof(Cost));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public CostShareTypes CostShareTypes
        {
            get { return _CostItem.CostShareTypes; }
            set
            {
                _CostItem.CostShareTypes = value;
                OnPropertyChanged(nameof(CostShareTypes));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public string Item
        {
            get { return _CostItem.Item; }
            set
            {
                _CostItem.Item = value;
                OnPropertyChanged(nameof(Item));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        private readonly CostItem _CostItem;
        public CostItem CostItem => _CostItem;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler? ValueChange;

        #endregion event properties & fields


        // constructors
        #region constructors

        public CostItemViewModel(CostItem otherCostItem) //, RoomViewModel roomViewModel)
        {
            _CostItem = otherCostItem;
            //_RoomViewModel = roomViewModel;
        }

        #endregion constructors


    }
}
// EOF