/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  TaxOptionTypes 
 * 
 *  enum holds all supported tax options
 *  
 *  there can be either taxed, meaning the sum already contains the tax
 *  and there can be untaxed, meaning the tax must be applied to the sum.  
 */

namespace SharedLivingCostCalculator.Enums
{
    public enum DataOutputProgressionTypes
    {
        TimeChange,
        ValueChange
    }
}
// EOF