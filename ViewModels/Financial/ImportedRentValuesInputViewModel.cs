using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class ImportedRentValuesInputViewModel : BaseViewModel
    {

		private RentViewModel _RentViewModel;
		public RentViewModel RentViewModel => _RentViewModel;


		public ImportedRentValuesInputViewModel(RentViewModel rentViewModel)
        {
			_RentViewModel = rentViewModel;
                
        }


    }
}
// EOF