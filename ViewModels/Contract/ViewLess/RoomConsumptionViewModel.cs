﻿using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.ViewModels.Contract.ViewLess
{
    public class RoomConsumptionViewModel : BaseViewModel
    {

        public double ConsumptionValue
        {
            get { return _RoomConsumption.ConsumptionValue; }
            set {
                _RoomConsumption.ConsumptionValue = value;
                OnPropertyChanged(nameof(ConsumptionValue));
            }
        }


        public double RoomArea => _RoomConsumption.Room.RoomArea;


        private RoomConsumption _RoomConsumption;
        public RoomConsumption RoomConsumption => _RoomConsumption;


        public string RoomName => _RoomConsumption.Room.RoomName;


        public RoomConsumptionViewModel(RoomConsumption roomConsumption)
        {
            _RoomConsumption = roomConsumption;
        }

    }
}
