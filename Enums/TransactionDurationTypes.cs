/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TransactionShareTypesBilling 
 * 
 *  enum holds all supported durations for FTIs
 *  
 *  there can be either an ongoing payment with no planned end, f.e. like a contract with an internet service provider
 *  or there can be payments of limited duration like an installment payment, f.e. for a new washing machine
 */

namespace SharedLivingCostCalculator.Enums
{
    public enum TransactionDurationTypes
    {
        Ongoing,
        Limited
    }
}
// EOF
