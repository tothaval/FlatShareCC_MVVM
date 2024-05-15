using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace SharedLivingCostCalculator.Commands
{
    class ShowFlatAccountingCommand : BaseCommand
    {
        private INavigationService _navigationService;

        public ShowFlatAccountingCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter.GetType()==typeof(FlatViewModel))
            {
                _navigationService.ChangeView(new AccountingViewModel(
                                        (FlatViewModel)parameter, _navigationService));
            }

        }
    }
}
