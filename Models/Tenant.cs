/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *    
 *  Tenant 
 * 
 *  serializable data model class 
 */

namespace SharedLivingCostCalculator.Models
{

    [Serializable]
    public class Tenant
    {

        // properties & fields
        #region properties

        public bool IsActive { get; set; }


        public DateTime MovingIn { get; set; }


        public string Name { get; set; }


        // only show and set if IsActive is false;
        public DateTime MovingOut { get; set; }

        #endregion properties


        // constructors
        #region constructors

        public Tenant()
        {
            IsActive = true;
            Name = "tenant";
            MovingIn = DateTime.Now;
            MovingOut = DateTime.Now.AddMonths(1);
        }        
        
        #endregion constructors


    }
}
// EOF