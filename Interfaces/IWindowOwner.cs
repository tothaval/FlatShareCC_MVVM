﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Interfaces
{
    public interface IWindowOwner
    {
        public void OwnedWindow_Closed(object? sender, EventArgs e);
    }
}