﻿using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatManagementViewModel : BaseViewModel
    {
        private string headerText;

        public string HeaderText
        {
            get { return headerText; }
            set { headerText = value; OnPropertyChanged(nameof(HeaderText)); }
        }

        private ObservableCollection<FlatViewModel> _flatCollection;
        public ObservableCollection<FlatViewModel> FlatCollection => _flatCollection;


        public ICommand NewFlatCommand { get; }
        public ICommand AccountingCommand { get; }
        public ICommand EditFlatCommand { get; }

        public ICommand DeleteFlatCommand { get; }

        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection, INavigationService newFlatSetupNavigationService,
            INavigationService newFlatManagementNavigationService)
        {
            NewFlatCommand = new NavigateCommand(newFlatSetupNavigationService);

            AccountingCommand = new ShowFlatAccountingCommand(newFlatManagementNavigationService);

            EditFlatCommand = new ShowEditFlatCommand(newFlatManagementNavigationService);

            DeleteFlatCommand = new ExecuteDeleteFlatCommand(flatCollection);

            _flatCollection = flatCollection;

            MainWindowTitleText = "Shared Living Cost Calculator - Flat Overview";
            HeaderText = "Flat Overview";
        }
    }
}
