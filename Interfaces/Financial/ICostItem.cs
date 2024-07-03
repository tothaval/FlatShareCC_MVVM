/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ICostItem 
 * 
 *  interface for costitem feature
 */
using SharedLivingCostCalculator.Enums;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    public interface ICostItem
    {
        public double Cost { get; set; }


        public CostShareTypes CostShareTypes { get; set; }


        public string Item { get; set; }
    }
}
// EOF