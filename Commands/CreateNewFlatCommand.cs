﻿using SharedLivingCostCalculator.Services;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Commands
{
    internal class CreateNewFlatCommand : BaseCommand
    {
        private INavigationService _navigationService;

        public CreateNewFlatCommand(INavigationService navigationService)
        {          
            _navigationService = navigationService;
        }

        public override void Execute(object? parameter)
        {
            _navigationService.ChangeView();
        }
    }
}
