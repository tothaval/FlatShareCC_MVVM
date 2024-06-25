/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  OtherCostItemViewModel  : BaseViewModel
 * 
 *  data model class
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class OtherCostItemViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public double Cost
        {
            get { return _OtherCostItem.Cost; }
            set
            {
                _OtherCostItem.Cost = value;
                OnPropertyChanged(nameof(Cost));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        public CostShareTypes CostShareTypes
        {
            get { return _OtherCostItem.CostShareTypes; }
            set
            {
                _OtherCostItem.CostShareTypes = value;
                OnPropertyChanged(nameof(CostShareTypes));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public string Item
        {
            get { return _OtherCostItem.Item; }
            set
            {
                _OtherCostItem.Item = value;
                OnPropertyChanged(nameof(Item));

                ValueChange?.Invoke(this, EventArgs.Empty);
            }
        }


        private readonly OtherCostItem _OtherCostItem;
        public OtherCostItem OtherCostItem => _OtherCostItem;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler? ValueChange;

        #endregion event properties & fields


        // constructors
        #region constructors

        public OtherCostItemViewModel(OtherCostItem otherCostItem) //, RoomViewModel roomViewModel)
        {
            _OtherCostItem = otherCostItem;
            //_RoomViewModel = roomViewModel;
        }

        #endregion constructors


    }
}
// EOF