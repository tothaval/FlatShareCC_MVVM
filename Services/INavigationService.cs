using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Services
{
    internal interface INavigationService
    {
        void ChangeView();
        void ChangeView(BaseViewModel baseViewModel);

        BaseViewModel GetViewModel();

        NavigationStore GetNavigationStore();
    
    }
}
