using SharedLivingCostCalculator.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    [Serializable]
    public class OtherCostItem
    {
        public CostShareTypes CostShareTypes { get; set; } = CostShareTypes.Equal;

        public string Item { get; set; } = "other cost item";

        public double Cost { get; set; } = 0.0;

        public OtherCostItem()
        {
            
        }
    }
}
