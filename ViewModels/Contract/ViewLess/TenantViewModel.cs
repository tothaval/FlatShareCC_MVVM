/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for Tenant model
 */


/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TenantSetupViewModel  : BaseViewModel
 * 
 *  viewmodel for Tenant model
 */

using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class TenantViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public bool IsActive
        {
            get { return _tenant.IsActive; }
            set
            {
                _tenant.IsActive = value;
                OnPropertyChanged(nameof(IsActive));

                TenantIsActiveChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public DateTime MovingIn
        {
            get { return _tenant.MovingIn; }
            set
            {
                _tenant.MovingIn = value;
                OnPropertyChanged(nameof(MovingIn));
            }
        }


        public DateTime MovingOut
        {
            get { return _tenant.MovingOut; }
            set
            {
                _tenant.MovingOut = value;
                OnPropertyChanged(nameof(MovingOut));
            }
        }


        public string Name
        {
            get { return _tenant.Name; }
            set
            {
                _tenant.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        private Tenant _tenant;
        public Tenant GetTenant => _tenant;

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler TenantIsActiveChanged;

        #endregion event properties & fields


        // constructors
        #region constructors

        public TenantViewModel(Tenant tenant)
        {
            _tenant = tenant;
        }

        #endregion constructors


    }
}
// EOF