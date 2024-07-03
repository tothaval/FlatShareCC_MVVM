using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Interfaces.Financial;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models.Financial
{
    [Serializable]
    public class BillingCredit : ICredit
    {
        public OccurrenceQuantity Quantity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Credit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Duration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ObservableCollection<int> RoomIDs { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime Begin { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime End { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BillingCredit()
        {
            RoomIDs = new ObservableCollection<int>() { };

        }


        public BillingCredit(OccurrenceQuantity occurrenceQuantity, double credit, int duration)
        {
            Quantity = occurrenceQuantity;
            Credit = credit;
            Duration = duration;

            RoomIDs = new ObservableCollection<int>() { };
        }
    }
}
