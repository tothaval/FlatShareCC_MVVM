/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *    
 *  Tenant 
 * 
 *  data model class <- as of yet unsure if there is a need to this class,
 *                   <- because if there is a tenant change and the tenant
 *                   <- pays a sum based on the previous tenants habits, all
 *                   <- that happens is the new tenant paying advances anyway
 *                   
 *                   <- in extreme cases the billing received would require more
 *                   <- payments from the new tenant or could be a credit
 *                   
 *                   <- if calculations are not factored in a tenant change,
 *                   <- then i wouldn't need this class and could include a
 *                   <- field into the Room class like RoomName.                   
 *                   <- but then this introduces a new problem, because users
 *                   <- using some sort of history function to view all previous
 *                   <- billings and rent costs would see the same name, which
 *                   <- would be the wrong one in some cases. so i would need
 *                   <- this class  and it's date values, but only for such
 *                   <- feature
 */

namespace SharedLivingCostCalculator.Models
{
    public class Tenant
    {

        // properties & fields
        #region properties

        public DateTime MovingOut { get; set; }


        public string Name { get; set; }


        public DateTime MovingIn { get; set; }

        #endregion properties


        // constructors
        #region constructors

        public Tenant()
        {
            Name = string.Empty;
            MovingIn = DateTime.Now;
            MovingOut = DateTime.Now.AddMonths(1);
        }        
        
        #endregion constructors


    }
}
// EOF