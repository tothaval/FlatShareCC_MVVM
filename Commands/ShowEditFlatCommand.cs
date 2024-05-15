using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Commands
{
    class ShowEditFlatCommand : BaseCommand
    {
        INavigationService _navigationService;

        public ShowEditFlatCommand(INavigationService navigationService)
        {
                _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            if (parameter != null && parameter.GetType() == typeof(FlatViewModel))
            {
                _navigationService.ChangeView(new EditFlatViewModel(
                                        (FlatViewModel)parameter, _navigationService));
            }
        }
    }
}
