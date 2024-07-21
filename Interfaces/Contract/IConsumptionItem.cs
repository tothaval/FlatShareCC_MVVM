using SharedLivingCostCalculator.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Interfaces.Contract
{
    public interface IConsumptionItem
    {
        FinancialTransactionItemBilling ConsumptionCause { get; }

        double ConsumedUnits { get; set; }
    }
}
