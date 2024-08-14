/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *    
 *  Tenant 
 * 
 *  serializable data model class 
 */

namespace SharedLivingCostCalculator.Models.Contract
{

    [Serializable]
    public class Tenant
    {

        // Properties & Fields
        #region Properties & Fields

        public bool IsActive { get; set; }


        public DateTime MovingIn { get; set; }


        public string Name { get; set; }


        // only show and set if IsActive is false;
        public DateTime MovingOut { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public Tenant()
        {
            IsActive = true;
            Name = "tenant";
            MovingIn = DateTime.Now;
            MovingOut = DateTime.Now.AddMonths(1);
        }

        #endregion


    }
}
// EOF