using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Financial.ViewLess
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