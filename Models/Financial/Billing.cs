﻿/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  Billing 
 * 
 *  data model class for _BillingViewModel
 */
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Xml.Serialization;


namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class Billing
    {

        // Properties & Fields
        #region Properties & Fields

        public double ActualAdvancePerPeriod { get; set; } = 0.0;

                
        public FinancialTransactionItemBilling BasicHeatingCosts { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Basic Heating Costs",
            TransactionShareTypes = TransactionShareTypesBilling.Area,
            TransactionSum = 0.0
        };


        public double BasicHeatingCostsPercentage { get; set; } = 0.0;


        public FinancialTransactionItemBilling ColdWaterCosts { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Cold Water Costs",
            TransactionShareTypes = TransactionShareTypesBilling.Equal,
            TransactionSum = 0.0
        };

        public FinancialTransactionItemBilling ConsumptionHeatingCosts { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Consumption Heating Costs",
            TransactionShareTypes = TransactionShareTypesBilling.Consumption,
            TransactionSum = 0.0
        };


        public double ConsumptionHeatingCostsPercentage { get; set; } = 0.0;


        public DateTime BillingDate { get; set; } = DateTime.Now;


        public bool CostsHasDataLock { get; set; } = false;


        // end of billing period
        public DateTime EndDate { get; set; } = DateTime.Now;


        public bool HasCredits { get; set; } = false;


        public bool HasDataLock { get; set; } = false;
        

        public bool HasOtherCosts { get; set; } = false;


        public bool HasPayments { get; set; } = false;


        // begin of billing period
        public DateTime StartDate { get; set; } = DateTime.Now - TimeSpan.FromDays(365);


        // combined costs of fixed costs and heating costs
        // costs need to take RoomPayments per room into consideration
        public double TotalCostsPerPeriod { get; set; } = 0.0;


        // Pro Rata
        // all annual costs that are not rent, heating or water.
        // can be calculated per room using
        // (((room area) + (shared space)/(amount of Rooms))/(total area)) * fixed costs
        public FinancialTransactionItemBilling ProRataCosts { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Pro Rata",
            TransactionShareTypes = TransactionShareTypesBilling.Area,
            TransactionSum = 0.0
        };


        // Fixed Amount
        // warm and cold water costs and heating costs
        // heating costs are comprised of basic heating costs and consumption heating costs        
        //
        // shared space heating costs can be devided by the number of Rooms
        // room based heating costs must take heating units consumption into account
        public FinancialTransactionItemBilling HeatingCosts { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Fixed Amount",
            TransactionShareTypes = TransactionShareTypesBilling.Consumption,
            TransactionSum = 0.0
        };


        public FinancialTransactionItemBilling WarmWaterCosts { get; set; } = new FinancialTransactionItemBilling()
        {
            TransactionItem = "Warm Water Costs",
            TransactionShareTypes = TransactionShareTypesBilling.Equal,
            TransactionSum = 0.0
        };

        #endregion


        // Collections
        #region Collections

        // storing the costs of each room
        // per billing period and the consumption of heating units per billing period
        [XmlArray("Consumptions")]
        public ObservableCollection<ConsumptionItem> ConsumptionItems { get; set; } = new ObservableCollection<ConsumptionItem>();


        // storing TransactionItems in case of other costs being factored in into rent calculation
        [XmlArray("Costs")]
        public ObservableCollection<FinancialTransactionItemBilling> Costs { get; set; } = new ObservableCollection<FinancialTransactionItemBilling>();


        // storing TransactionItems in case of credits being factored in into rent calculation
        [XmlArray("Credits")]
        public ObservableCollection<FinancialTransactionItemRent> Credits { get; set; } = new ObservableCollection<FinancialTransactionItemRent>();


        // storing the payments of each room
        // per billing period
        [XmlArray("Payments")]
        public ObservableCollection<RoomPayments> RoomPayments { get; set; } = new ObservableCollection<RoomPayments>();

        #endregion


        // Constructors
        #region Constructors

        public Billing()
        {
            Costs.CollectionChanged += Costs_CollectionChanged;
        }


        public Billing(
                FlatViewModel model
                )
        {

            Costs.CollectionChanged += Costs_CollectionChanged;
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
            ProRataCosts.TransactionSum = totalFixedCostsPerPeriod;
            HeatingCosts.TransactionSum = totalHeatingCostsPerPeriod;


            Costs.CollectionChanged += Costs_CollectionChanged;
            AddRoomPayments(model);
        }

        #endregion


        // Methods
        #region Methods

        public void AddConsumptionItem(FinancialTransactionItemBilling financialTransactionItem)
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


        public void AddCredit(FinancialTransactionItemRent financialTransactionItem)
        {
            if (IsNewFTI(financialTransactionItem))
            {
                Credits.Add(financialTransactionItem);
            }

            if (!CollectionContainsFTI(Credits, financialTransactionItem))
            {
                Credits.Add(financialTransactionItem);
            }
        }


        public void AddFinacialTransactionItem(FinancialTransactionItemBilling financialTransactionItem)
        {
            if (IsNewFTI(financialTransactionItem))
            {
                if (financialTransactionItem.TransactionShareTypes == TransactionShareTypesBilling.Consumption)
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


        public void ClearCosts()
        {
            Costs.Clear();
        }

        public void ClearCredits()
        {
            Credits.Clear();
        }


        private bool CollectionContainsFTI(ICollection<FinancialTransactionItemBilling> FTIs, FinancialTransactionItemBilling item)
        {
            foreach (FinancialTransactionItemBilling fti in FTIs)
            {
                if (CompareFTI(fti, item))
                {
                    return true;
                }
            }

            return false;
        }


        private bool CollectionContainsFTI(ICollection<FinancialTransactionItemRent> FTIs, FinancialTransactionItemRent item)
        {
            foreach (FinancialTransactionItemRent fti in FTIs)
            {
                if (CompareFTI(fti, item))
                {
                    return true;
                }
            }

            return false;
        }


        private bool CompareFTI(FinancialTransactionItemBilling fti, FinancialTransactionItemBilling item)
        {
            if (item.TransactionItem == fti.TransactionItem
                && item.TransactionSum == fti.TransactionSum
                && item.TransactionShareTypes == item.TransactionShareTypes)
            {
                return true;
            }

            return false;
        }


        private bool CompareFTI(FinancialTransactionItemRent fti, FinancialTransactionItemRent item)
        {
            if (item.TransactionItem == fti.TransactionItem
                && item.TransactionSum == fti.TransactionSum
                && item.TransactionShareTypes == item.TransactionShareTypes)
            {
                return true;
            }

            return false;
        }


        public void Check4HeatingCosts()
        {
            HeatingCosts.TransactionShareTypes = TransactionShareTypesBilling.Consumption;
            HeatingCosts.TransactionItem = "Heating";

            int heatingCostsCount = 0;
            bool hasForeignObject = false;

            foreach (ConsumptionItem item in ConsumptionItems)
            {
                if (item.ConsumptionCause.TransactionItem.Equals(HeatingCosts.TransactionItem))
                {
                    heatingCostsCount++;

                    if (heatingCostsCount > 1)
                    {
                        ConsumptionItems.Remove(item);

                        break;
                    }
                }

                if (item.ConsumptionCause.TransactionShareTypes != TransactionShareTypesBilling.Consumption)
                {
                    ConsumptionItems.Remove(item);
                    hasForeignObject = true;
                    break;
                }
            }

            if (heatingCostsCount == 0)
            {
                ConsumptionItems.Add(new ConsumptionItem(HeatingCosts));
            }
            else if (heatingCostsCount > 1)
            {
                Check4HeatingCosts();
            }
            else if (hasForeignObject)
            {
                Check4HeatingCosts();
            }
        }


        private bool IsNewFTI(FinancialTransactionItemBilling item)
        {
            if (item.TransactionSum == 0.0
                && item.TransactionItem.Equals("other cost item"))
            {
                return true;
            }

            return false;
        }


        private bool IsNewFTI(FinancialTransactionItemRent item)
        {
            if (item.TransactionSum == 0.0
                && item.TransactionItem.Equals("other cost item"))
            {
                return true;
            }

            return false;
        }


        private void RemoveConsumptionItem(FinancialTransactionItemBilling financialTransactionItem)
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


        public void RemoveCredit(FinancialTransactionItemRent financialTransactionItem)
        {
            if (CollectionContainsFTI(Credits, financialTransactionItem))
            {
                Credits.Remove(financialTransactionItem);
            }
        }


        public void RemoveFinacialTransactionItem(FinancialTransactionItemBilling financialTransactionItem)
        {
            if (CollectionContainsFTI(Costs, financialTransactionItem))
            {
                RemoveConsumptionItem(financialTransactionItem);

                Costs.Remove(financialTransactionItem);
            }
        }

        #endregion


        // Events
        #region Events

        private void Costs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HasOtherCosts = (Costs.Count == 0) ? HasOtherCosts = false : HasOtherCosts = true;
        }

        #endregion


    }
}
// EOF