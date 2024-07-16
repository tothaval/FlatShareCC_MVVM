/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Billing 
 * 
 *  data model class for BillingViewModel
 */

using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using static System.Net.WebRequestMethods;

namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class Billing
    {

        // properties & fields
        #region properties

        // end of billing period
        public DateTime EndDate { get; set; } = DateTime.Now;


        public bool HasCredits { get; set; } = false;


        public bool HasDataLock { get; set; } = false;


        public bool HasPayments { get; set; } = false;


        // begin of billing period
        public DateTime StartDate { get; set; } = DateTime.Now - TimeSpan.FromDays(365);


        // combined costs of fixed costs and heating costs
        // costs need to take RoomPayments per room into consideration
        public double TotalCostsPerPeriod { get; set; } = 0.0;


        // fixed costs
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of Rooms))/(total area)) * fixed costs
        public FinancialTransactionItem TotalFixedCostsPerPeriod { get; set; } = new FinancialTransactionItem()
        {
            TransactionItem = "Fixed",
            TransactionShareTypes = TransactionShareTypes.Area,
            TransactionSum = 0.0
        };


        // heating costs 
        // shared space heating costs can be devided by the number of Rooms
        // room based heating costs must take heating units constumption into
        // account
        public FinancialTransactionItem TotalHeatingCostsPerPeriod { get; set; } = new FinancialTransactionItem()
        {
            TransactionItem = "Heating",
            TransactionShareTypes = TransactionShareTypes.Consumption,
            TransactionSum = 0.0
        };

        // heating units used in billing period
        // values for Rooms must be determined in order to
        // calculate new rent shares based on consumption
        public double TotalHeatingUnitsConsumption { get; set; } = 0.0;


        // combined sum of room heating units consumption
        public double TotalHeatingUnitsRoom { get; set; } = 0.0;

        #endregion properties


        // collections
        #region collections

        // storing the costs of each room
        // per billing period and the consumption of heating units per billing period
        [XmlArray("Consumptions")]
        public ObservableCollection<ConsumptionItem> ConsumptionItems { get; set; } = new ObservableCollection<ConsumptionItem>();
        // storing TransactionItems in case of other costs being factored in into rent calculation


        [XmlArray("OtherCostItemCollection")]
        public ObservableCollection<FinancialTransactionItem> Costs { get; set; } = new ObservableCollection<FinancialTransactionItem>();


        // storing the costs of each room
        // per billing period and the consumption of heating units per billing period
        [XmlArray("HeatingUnits")]
        public ObservableCollection<RoomCosts> RoomCostsConsumptionValues { get; set; } = new ObservableCollection<RoomCosts>();


        // storing the payments of each room
        // per billing period
        [XmlArray("Payments")]
        public ObservableCollection<RoomPayments> RoomPayments { get; set; } = new ObservableCollection<RoomPayments>();

        #endregion collections


        // constructors
        #region constructors

        public Billing()
        {

        }


        public Billing(
                FlatViewModel model
                )
        {

            foreach (RoomViewModel room in model.Rooms)
            {
                RoomCostsConsumptionValues.Add(new RoomCosts(room));
                RoomPayments.Add(new RoomPayments(room));
            }
        }


        public Billing(
                        FlatViewModel model,
                        DateTime startDate,
                        DateTime endDate,
                        double totalCostsPerPeriod,
                        double totalFixedCostsPerPeriod,
                        double totalHeatingCostsPerPeriod,
                        double totalHeatingUnitsConsumption,
                        double totalHeatingUnitsRoom
                        )
        {
            StartDate = startDate;
            EndDate = endDate;
            TotalCostsPerPeriod = totalCostsPerPeriod;
            TotalFixedCostsPerPeriod.TransactionSum = totalFixedCostsPerPeriod;
            TotalHeatingCostsPerPeriod.TransactionSum = totalHeatingCostsPerPeriod;
            TotalHeatingUnitsConsumption = totalHeatingUnitsConsumption;
            TotalHeatingUnitsRoom = totalHeatingUnitsRoom;

            foreach (RoomViewModel room in model.Rooms)
            {
                RoomCostsConsumptionValues.Add(new RoomCosts(room));
                RoomPayments.Add(new RoomPayments(room));
            }
        }

        #endregion constructors


        // Methods
        #region Methods

        public void AddConsumptionItem(ConsumptionItem consumptionItem)
        {
           int count = 0;

            foreach (ConsumptionItem item in ConsumptionItems)
            {
                if (!item.ConsumptionCause.TransactionItem.Equals(consumptionItem.ConsumptionCause.TransactionItem)
                    && !item.ConsumptionCause.TransactionItem.Equals("other cost item"))
                {
                    count++;
                }
            }

            if (count > 0)
            {
                ConsumptionItems.Add(consumptionItem);
            }            

        }


        public void AddFinacialTransactionItem(FinancialTransactionItem financialTransactionItem)
        {
            if (IsNewFTI(financialTransactionItem))
            {
                Costs.Add(financialTransactionItem);
            }

            if (!CollectionContainsFTI(Costs, financialTransactionItem))
            {
                Costs.Add(financialTransactionItem);
            }
        }


        private bool CollectionContainsFTI(ICollection<FinancialTransactionItem> FTIs, FinancialTransactionItem item)
        {
            foreach (FinancialTransactionItem fti in FTIs)
            {
                if (CompareFTI(fti, item))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CompareFTI(FinancialTransactionItem fti, FinancialTransactionItem item)
        {
            if (item.TransactionItem == fti.TransactionItem
                && item.TransactionSum == fti.TransactionSum
                && item.TransactionShareTypes == fti.TransactionShareTypes)
            {
                return true;
            }

            return false;
        }


        private bool IsNewFTI(FinancialTransactionItem item)
        {
            if (item.TransactionSum == 0.0
                && item.TransactionItem.Equals("other cost item"))
            {
                return true;
            }

            return false;
        }




        public void RemoveConsumptionItem(ConsumptionItem consumptionItem)
        {
            if (ConsumptionItems.Contains(consumptionItem))
            {
                ConsumptionItems.Remove(consumptionItem);
            }
        }


        public void RemoveFinacialTransactionItem(FinancialTransactionItem financialTransactionItem)
        {
            if (CollectionContainsFTI(Costs, financialTransactionItem))
            {
                    Costs.Remove(financialTransactionItem);                    
            }
        }

        #endregion


    }
}
// EOF