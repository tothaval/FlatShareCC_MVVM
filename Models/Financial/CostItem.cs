/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Flat 
 * 
 *  serializable data model class
 *  for CostItemViewModel
 */

/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Flat 
 * 
 *  serializable data model class
 *  for CostItemViewModel
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;

namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class CostItem : ICostItem
    {

        // properties & fields
        #region properties

        public double Cost { get; set; } = 0.0;


        public CostShareTypes CostShareTypes { get; set; } = CostShareTypes.Equal;


        public string Item { get; set; } = "other cost item";

        #endregion properties


        // constructors
        #region constructors

        public CostItem()
        {

        }

        #endregion constructors


    }
}

// EOF