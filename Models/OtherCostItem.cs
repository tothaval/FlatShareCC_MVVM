/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Flat 
 * 
 *  serializable data model class
 *  for OtherCostItemViewModel
 */
using SharedLivingCostCalculator.Enums;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class OtherCostItem
    {

        // properties & fields
        #region properties

        public double Cost { get; set; } = 0.0;


        public CostShareTypes CostShareTypes { get; set; } = CostShareTypes.Equal;


        public string Item { get; set; } = "other cost item";

        #endregion properties


        // constructors
        #region constructors

        public OtherCostItem()
        {
            
        }

        #endregion constructors


    }
}

// EOF