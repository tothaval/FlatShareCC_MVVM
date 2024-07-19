/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Billing 
 * 
 *  data model class for BillingViewModel
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class Billing
    {

        // properties & fields
        #region properties

        public bool CostsHasDataLock { get; set; } = false;


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

        #endregion properties


        // collections
        #region collections

        // storing the costs of each room
        // per billing period and the consumption of heating units per billing period
        [XmlArray("Consumptions")]
        public ObservableCollection<ConsumptionItem> ConsumptionItems { get; set; } = new ObservableCollection<ConsumptionItem>();
        // storing TransactionItems in case of other costs being factored in into rent calculation


        [XmlArray("Costs")]
        public ObservableCollection<FinancialTransactionItem> Costs { get; set; } = new ObservableCollection<FinancialTransactionItem>();


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

            AddRoomPayments(model);
            Check4HeatingCosts();
        }


        public Billing(
                        FlatViewModel model,
                        DateTime startDate,
                        DateTime endDate,
                        double totalCostsPerPeriod,
                        double totalFixedCostsPerPeriod,
                        double totalHeatingCostsPerPeriod
                        )
        {
            StartDate = startDate;
            EndDate = endDate;
            TotalCostsPerPeriod = totalCostsPerPeriod;
            TotalFixedCostsPerPeriod.TransactionSum = totalFixedCostsPerPeriod;
            TotalHeatingCostsPerPeriod.TransactionSum = totalHeatingCostsPerPeriod;


            AddRoomPayments(model);
        }

        #endregion constructors


        // Methods
        #region Methods

        public void AddConsumptionItem(FinancialTransactionItem financialTransactionItem)
        {
            int count = 0;

            foreach (ConsumptionItem item in ConsumptionItems)
            {
                if (CompareFTI(item.ConsumptionCause, financialTransactionItem))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                ConsumptionItems.Add(new ConsumptionItem(financialTransactionItem));
            }

            Check4HeatingCosts();

        }


        public void AddFinacialTransactionItem(FinancialTransactionItem financialTransactionItem)
        {
            if (IsNewFTI(financialTransactionItem))
            {
                if (financialTransactionItem.TransactionShareTypes == TransactionShareTypes.Consumption)
                {
                    AddConsumptionItem(financialTransactionItem);
                }                

                Costs.Add(financialTransactionItem);
            }

            if (!CollectionContainsFTI(Costs, financialTransactionItem))
            {
                AddConsumptionItem(financialTransactionItem);

                Costs.Add(financialTransactionItem);
            }
        }

        public void AddRoomPayments(FlatViewModel model)
        {
            foreach (RoomViewModel room in model.Rooms)
            {
                RoomPayments.Add(new RoomPayments(room));
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


        public void Check4HeatingCosts()
        {
            TotalHeatingCostsPerPeriod.TransactionShareTypes = TransactionShareTypes.Consumption;
            TotalHeatingCostsPerPeriod.TransactionItem = "Heating";

            int heatingCostsCount = 0;
            bool hasForeignObject = false;

            foreach (ConsumptionItem item in ConsumptionItems)
            {
                if (item.ConsumptionCause.TransactionItem.Equals(TotalHeatingCostsPerPeriod.TransactionItem))
                {
                    heatingCostsCount++;

                    if (heatingCostsCount > 1)
                    {
                        ConsumptionItems.Remove(item);

                        break;
                    }
                }

                if (item.ConsumptionCause.TransactionShareTypes != TransactionShareTypes.Consumption)
                {
                    ConsumptionItems.Remove(item);
                    hasForeignObject = true;
                    break;
                }
            }

            if (heatingCostsCount == 0)
            {
                ConsumptionItems.Add(new ConsumptionItem(TotalHeatingCostsPerPeriod));
            }
            else if(heatingCostsCount > 1)
            {
                Check4HeatingCosts();
            }
            else if(hasForeignObject)
            {
                Check4HeatingCosts();
            }
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


        private void RemoveConsumptionItem(FinancialTransactionItem financialTransactionItem)
        {
            foreach (ConsumptionItem item in ConsumptionItems)
            {
                if (CompareFTI(financialTransactionItem, item.ConsumptionCause))
                {
                    ConsumptionItems.Remove(item);
                    break;
                }
            }

            Check4HeatingCosts();

        }


        public void RemoveFinacialTransactionItem(FinancialTransactionItem financialTransactionItem)
        {
            if (CollectionContainsFTI(Costs, financialTransactionItem))
            {
                RemoveConsumptionItem(financialTransactionItem); 

                Costs.Remove(financialTransactionItem);
            }
        }

        #endregion


    }
}
// EOF