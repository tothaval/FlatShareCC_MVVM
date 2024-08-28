/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  DataOutputProgressionTypes 
 * 
 *  enum holds all supported data progression options
 *  
 *  there can be either TimeChange, meaning for every month there will be data displayed in rent, credit and other print.
 *  and there can be ValueChange, meaning for every rent change there will be data displayed in rent, credit and other print.
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