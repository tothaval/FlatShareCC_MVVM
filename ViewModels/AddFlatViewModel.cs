using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMietkosten.Models;

namespace WGMietkosten.ViewModels
{
    class AddFlatViewModel : ViewModelBase
    {
        private readonly FlatSetup _flatSetup;

        public int ID => _flatSetup.ID;
        public string Address => _flatSetup.Address;
        public string Details => _flatSetup.FlatName;  
        public double Area => _flatSetup.Area;
        public int Rooms => _flatSetup.RoomCount;
        public string Tenants => _flatSetup.Tenants;

        public AddFlatViewModel(FlatSetup flatSetup)
        {
            _flatSetup = flatSetup;           
        }

        /// <summary>
        /// returns the FlatSetup object.
        /// </summary>
        /// <returns></returns>
        public FlatSetup GetFlatSetup() { return _flatSetup; }                
    }
}
