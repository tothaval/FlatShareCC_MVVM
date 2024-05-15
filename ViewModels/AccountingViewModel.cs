using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class AccountingViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;

        public string Address => _flatViewModel.Address;

        public ICommand LeaveCommand { get; }
        
        

        public AccountingViewModel(FlatViewModel flatViewModel,INavigationService navigationService)
        {
            _flatViewModel = flatViewModel;
            LeaveCommand = new NavigateCommand(navigationService);
        }
    }
}
