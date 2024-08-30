/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for Tenant model
 */


/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for Tenant model
 */

using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class TenantViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public double DepositShare
        {
            get { return _Tenant.DepositShare; }
            set
            {
                _Tenant.DepositShare = value;
                OnPropertyChanged(nameof(DepositShare));
            }
        }


        public bool IsActive
        {
            get { return _Tenant.IsActive; }
            set
            {
                _Tenant.IsActive = value;
                OnPropertyChanged(nameof(IsActive));

                TenantIsActiveChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public DateTime MovingIn
        {
            get { return _Tenant.MovingIn; }
            set
            {
                _Tenant.MovingIn = value;
                OnPropertyChanged(nameof(MovingIn));
            }
        }


        public DateTime MovingOut
        {
            get { return _Tenant.MovingOut; }
            set
            {
                _Tenant.MovingOut = value;
                OnPropertyChanged(nameof(MovingOut));
            }
        }


        public string Name
        {
            get { return _Tenant.Name; }
            set
            {
                _Tenant.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        private Tenant _Tenant;
        public Tenant Tenant => _Tenant;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler TenantIsActiveChanged;

        #endregion event properties & fields


        // constructors
        #region constructors

        public TenantViewModel(Tenant tenant)
        {
            _Tenant = tenant;
        }

        #endregion constructors


    }
}
// EOF