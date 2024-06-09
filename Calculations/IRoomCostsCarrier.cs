using SharedLivingCostCalculator.Navigation;
using SharedLivingCostCalculator.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Calculations
{
    public interface IRoomCostsCarrier
    {
        void GenerateRoomCosts();

        FlatViewModel GetFlatViewModel();

        event PropertyChangedEventHandler DataChange;
    }
}
