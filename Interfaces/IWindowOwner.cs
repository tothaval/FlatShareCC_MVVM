/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  IWindowOwner 
 * 
 *  interface for viewmodels that can open window views
 */

namespace SharedLivingCostCalculator.Interfaces
{
    public interface IWindowOwner
    {
        public void OwnedWindow_Closed(object? sender, EventArgs e);


    }
}
// EOF