using SharedLivingCostCalculator.Interfaces.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class CostPeriod : ICostPeriod
    {
        public int NumberOfDays => throw new NotImplementedException();
        public int NumberOfRequiredPayments { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime PeriodBegin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime PeriodEnd { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }



        public ICollection<ICostItem> CostItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
