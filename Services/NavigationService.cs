using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Services
{
    internal class NavigationService<TViewModel> : INavigationService 
        where TViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void ChangeView()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }

        public BaseViewModel GetViewModel()
        {
            return _navigationStore.CurrentViewModel;
        }
    }
}
