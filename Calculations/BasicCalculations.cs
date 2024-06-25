/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *    
 *  BasicCalculations 
 * 
 *  provides methods for recurring calculations
 */
using SharedLivingCostCalculator.ViewModels.ViewLess;

namespace SharedLivingCostCalculator.Calculations
{
    public class BasicCalculations
    {

        // constructors
        #region constructors
        
        public BasicCalculations()
        {
                
        }

        #endregion constructors


        // methods
        #region methods

        public static double CalculatePercentage(double ratio)
        {
            return ratio * 100;
        }


        public static double CalculateRatio(double fraction, double total)
        {
            return fraction / total;
        }


        public static double Multiplication(List<double> doubles)
        {
            double result = 1;

            for (int i = 0; i < doubles.Count; i++)
            {
                result *= doubles[i];
            }

            return result;
        }


        /// <summary>
        /// Calculates the shared area of a flat divided by roomcount
        /// </summary>
        /// <param name="flatViewModel"></param>
        /// <returns> SharedArea(FlatViewModel) / RoomCount (FlatViewModel) on success
        /// -100.0 on failure </returns>
        public static double SharedAreaShare(FlatViewModel flatViewModel)
        {
            if (flatViewModel.RoomCount == 0)
            {
                return -100.0;
            }


            return flatViewModel.SharedArea / flatViewModel.RoomCount;
        }

        #endregion methods


    }
}

// EOF