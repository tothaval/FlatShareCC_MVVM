using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    class AccountingViewModel : BaseViewModel
    {
        private FlatViewModel _flatViewModel;
        public FlatViewModel FlatViewModel => _flatViewModel;

        private RentUpdateViewModel _rentUpdateViewModel;
        public RentUpdateViewModel RentUpdate => _rentUpdateViewModel;

        private BillingPeriodViewModel _billingPeriodViewModel;
        public BillingPeriodViewModel BillingPeriod => _billingPeriodViewModel;

        public ICommand LeaveCommand { get; }



        public AccountingViewModel(FlatViewModel flatViewModel, INavigationService navigationService)
        {
            _flatViewModel = flatViewModel;
            LeaveCommand = new NavigateCommand(navigationService);

            _billingPeriodViewModel = new BillingPeriodViewModel(FlatViewModel);
            _rentUpdateViewModel = new RentUpdateViewModel(FlatViewModel);

        }
    }
}
