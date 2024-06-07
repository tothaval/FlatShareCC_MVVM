using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class CostsViewModel : BaseViewModel
    {
        // if objects are chained together (f.e. a rent object and a billing object)
        // they should be selected together in every relevant tab

        // split viewmodels in two view models, one for rent costs and one for billing costs?

        // combobox to select billing on rentupdateview


        private readonly BillingViewModel? _billingViewModel;
        private readonly RentViewModel? _rentViewModel;


        private string _WindowTitle;

		public string WindowTitle
		{
			get { return _WindowTitle; }
			set
			{
				_WindowTitle = value;
				OnPropertyChanged(nameof(WindowTitle));
			}
		}


        public CostsViewModel(RentViewModel rentViewModel)
        {
			WindowTitle = "Shared Living Cost Calculator - Costs - Rent";

			_rentViewModel = rentViewModel;

            if (_rentViewModel.BillingViewModel != null)
            {
                _billingViewModel = _rentViewModel.BillingViewModel;
            }

        }

        public CostsViewModel(BillingViewModel billingViewModel)
        {
            WindowTitle = "Shared Living Cost Calculator - Costs - Annual Billing";

            _billingViewModel = billingViewModel;
        }
    }
}
