﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PaymentsSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for PaymentsSetupView
 *  
 *  displays all elements of ObservableCollection<PaymentViewModel>
 *  for a selected instance of RoomViewModel
 *  
 *  create, edit or delete instances of PaymentViewModel for a
 *  selected RoomViewModel instance
 */
using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SharedLivingCostCalculator.ViewModels.Financial
{
    public class PaymentsSetupViewModel : BaseViewModel, INotifyDataErrorInfo
    {

        // properties & fields
        #region properties

        private BillingViewModel _BillingViewModel;

        public bool DataLock
        {
            get { return !_BillingViewModel.HasDataLock; }
        }  

        public IEnumerable GetErrors(string? propertyName) => _helper.GetErrors(propertyName);


        private ValidationHelper _helper = new ValidationHelper();


        public bool HasErrors => _helper.HasErrors;


        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _helper.ClearError(nameof(Quantity));

                if (double.IsNaN(value))
                {
                    _helper.AddError("value must be a number", nameof(Quantity));
                }

                if (value < 0)
                {
                    _helper.AddError("value must be greater than 0", nameof(Quantity));
                }
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }


        private RoomPaymentsViewModel _RoomPaymentsViewModel;
        public RoomPaymentsViewModel RoomPaymentsViewModel
        {
            get { return _RoomPaymentsViewModel; }
            set
            {
                _RoomPaymentsViewModel = value;
                OnPropertyChanged(nameof(RoomPaymentsViewModel));
            }
        }

        #endregion properties


        // event properties & fields
        #region event properties

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;


        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion event properties


        // collections
        #region collections

        public ObservableCollection<PaymentViewModel> Payments
        {
            get{
                if (RoomPaymentsViewModel != null)
                {
                    return RoomPaymentsViewModel.PaymentViewModels;
                }

                return null;
            }
        }

        #endregion collections


        // commands
        #region commands

        public ICommand AddPaymentCommand { get; }


        public ICommand DeletePaymentCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public PaymentsSetupViewModel(RoomPaymentsViewModel roomPaymentsViewModel, BillingViewModel billingViewModel)
        {
            _BillingViewModel = billingViewModel;
            _RoomPaymentsViewModel = roomPaymentsViewModel;
            _quantity = 1;

            AddPaymentCommand = new AddPaymentCommand(_RoomPaymentsViewModel);
            DeletePaymentCommand = new DeletePaymentCommand(this);

            if (_RoomPaymentsViewModel != null)
            {
                _RoomPaymentsViewModel.RoomPayments.Payments.CollectionChanged += Payments_CollectionChanged;
            }            
        }
        #endregion constructors


        // events
        #region events

        private void Payments_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Payments));
            OnPropertyChanged(nameof(RoomPaymentsViewModel));
        }

        #endregion events


    }
}
// EOF