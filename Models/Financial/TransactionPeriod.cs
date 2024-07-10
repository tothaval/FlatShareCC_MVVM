using SharedLivingCostCalculator.Interfaces.Financial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models.Financial
{
    public class TransactionPeriod : IFinancialTransactionPeriod
    {
        public int NumberOfDays => (PeriodEnd - PeriodBegin).Days;
        public int NumberOfTransactions { get; set; } = 0;
        public DateTime PeriodBegin { get; set; } = DateTime.Now;
        public DateTime PeriodEnd { get; set; } = DateTime.Now.AddDays(1);

        public ObservableCollection<IFinancialTransactionItem> TransactionItems { get; set; } = new ObservableCollection<IFinancialTransactionItem>();
    }
}
