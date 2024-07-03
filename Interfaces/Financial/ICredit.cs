/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  ICredit 
 * 
 *  interface for credit feature
 */
using SharedLivingCostCalculator.Enums;
using System.Collections.ObjectModel;

namespace SharedLivingCostCalculator.Interfaces.Financial
{
    /// <summary>
    /// Credits: for payments and credits
    /// 
    /// can occur if by some event or action the landlord is legally bound
    /// to compensate financially the renting parties. costs are either
    /// reduced or nilled for a certain period of time or the renting
    /// parties receive money directly.
    /// 
    /// f.e. the landlord has not fixed the roof and water enters the apartment through the
    /// ceiling, consequence: the TV, the computer and a couch of one renting party was flooded,
    /// only one renting party profits from the credit
    /// 
    /// f.e. the annual billing is received and there is a surplus for the entire flat
    /// 
    /// f.e. rodents, the entire flat has reduced rents for a fixed period of time
    /// 
    /// a credit could also be a budget.
    /// 
    /// credits could be given to the entire flat, only a room or a bunch of rooms
    /// 
    /// credits could be given over varying amounts of time
    /// credits may occur monthly, which would affect the rentviewmodel
    /// credits could occur annually, which would affect the billingviewmodel
    /// 
    /// costs are either reduced or nilled for a certain period or
    /// the renting parties receive money directly.
    /// </summary>
    /// 
    public interface ICredit
    {

        /// <summary>
        /// start date of the credit period
        /// </summary>
        public DateTime Begin { get; set; }


        /// <summary>
        /// the reason the credit exists
        /// /// </summary>
        public string Cause { get; set; }


        public double Credit { get; set; }


        public int Duration { get; set; }


        /// <summary>
        /// end date of the credit period
        /// </summary>
        public DateTime End { get; set; }


        public OccurrenceQuantity Quantity { get; set; }


        public ObservableCollection<int> RoomIDs { get; set; }


        public double GetTotalCredit() => Duration * Credit;

    }
}
// EOF