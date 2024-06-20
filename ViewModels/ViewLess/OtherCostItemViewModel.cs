using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class OtherCostItemViewModel : BaseViewModel
    {
        public event EventHandler? ValueChange;

        private readonly OtherCostItem _OtherCostItem;
        public OtherCostItem OtherCostItem => _OtherCostItem;

        //private RoomViewModel _RoomViewModel;

        //public string RoomName => _RoomViewModel.RoomName;
        //public double RoomArea => _RoomViewModel.RoomArea;


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


        public OtherCostItemViewModel(OtherCostItem otherCostItem) //, RoomViewModel roomViewModel)
        {
            _OtherCostItem = otherCostItem;                
            //_RoomViewModel = roomViewModel;
        }
    }
}
