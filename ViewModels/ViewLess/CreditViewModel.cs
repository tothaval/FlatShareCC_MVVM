using SharedLivingCostCalculator.Interfaces;

namespace SharedLivingCostCalculator.ViewModels.ViewLess
{
    public class CreditViewModel : BaseViewModel
    {

		private ICredit _Credit;
		public ICredit Credit
		{
			get { return _Credit; }
			set
			{
				_Credit = value;
				OnPropertyChanged(nameof(Credit));
			}
		}



	}
}
// EOF