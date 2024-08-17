using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Utility
{
    public class Clone
    {
        public Clone()
        {
                
        }

        public FinancialTransactionItemBillingViewModel BillingFTI(FinancialTransactionItemBilling item)
        {
            string cause = item.TransactionItem;

            double sum = item.TransactionSum;

            TransactionShareTypesBilling shareType = item.TransactionShareTypes;

            return new FinancialTransactionItemBillingViewModel(
                new FinancialTransactionItemBilling() { TransactionItem = cause, TransactionSum = sum, TransactionShareTypes = shareType });
        }


        public FinancialTransactionItemRentViewModel FTI(FinancialTransactionItemRent item)
        {
            string cause = item.TransactionItem;

            double sum = item.TransactionSum;

            TransactionShareTypesRent shareType = item.TransactionShareTypes;

            TransactionDurationTypes durationTypes = item.Duration;

            DateTime endDate = new DateTime(item.EndDate.Year, item.EndDate.Month, item.EndDate.Day);
            DateTime startDate = new DateTime(item.StartDate.Year, item.StartDate.Month, item.StartDate.Day);

            return new FinancialTransactionItemRentViewModel(
                new FinancialTransactionItemRent()
                {
                    TransactionItem = cause,
                    TransactionSum = sum,
                    TransactionShareTypes = shareType,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = durationTypes
                });
        }

    }
}
