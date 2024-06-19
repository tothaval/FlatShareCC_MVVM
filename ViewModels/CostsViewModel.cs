/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CostsViewModel  : BaseViewModel
 * 
 *  viewmodel for CostsView
 *  
 *  a separate window which shows either a
 *  RentCostsViewModel or BillingCostsViewModel
 *  depending on constructor
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class CostsViewModel : BaseViewModel
    {
        // if objects are chained together (f.e. a rent object and a billing object)
        // they should be selected together in every relevant tab

        // combobox to select billing on rentupdateview


        private readonly BillingViewModel? _billingViewModel;


        private readonly RentViewModel? _rentViewModel;


        private readonly FlatViewModel _flatViewModel;


        private BaseViewModel _ActiveViewModel;
        public BaseViewModel ActiveViewModel
        {
            get { return _ActiveViewModel; }
            set
            {
                _ActiveViewModel = value;

                OnPropertyChanged(nameof(ActiveViewModel));
            }
        }


        private bool _BillingSelected;
        public bool BillingSelected
        {
            get { return _BillingSelected; }
            set
            {
                _BillingSelected = value;

                if (_BillingSelected)
                {
                    ActiveViewModel = new BillingCostsViewModel(_billingViewModel, _flatViewModel);
                }
                OnPropertyChanged(nameof(BillingSelected));
            }
        }


        public bool HasBilling => _billingViewModel != null;


        private bool _RentSelected;

        public bool RentSelected
        {
            get { return _RentSelected; }
            set
            {
                _RentSelected = value;

                if (_RentSelected)
                {
                    ActiveViewModel = new RentCostsViewModel(_rentViewModel, _flatViewModel);
                }
                OnPropertyChanged(nameof(RentSelected));
            }
        }



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


        public CostsViewModel(RentViewModel rentViewModel, FlatViewModel flatViewModel)
        {
            WindowTitle = "Shared Living Cost Calculator - Costs - Rent";

            _rentViewModel = rentViewModel;
            _flatViewModel = flatViewModel;

            if (_rentViewModel.BillingViewModel != null)
            {
                _billingViewModel = _rentViewModel.BillingViewModel;
            }

            RentSelected = true;

            OnPropertyChanged(nameof(HasBilling));
        }


    }
}
// EOF