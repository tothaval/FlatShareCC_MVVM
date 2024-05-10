using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WGMietkosten.Commands;
using WGMietkosten.Models;
using WGMietkosten.Navigation;

namespace WGMietkosten.ViewModels
{
    internal class FlatSetupViewModel : ViewModelBase
    {
		private string address;

		public string Address
		{
			get { return address; }
			set { address = value; OnPropertyChanged(nameof(Address));	}
		}

        private string details;

        public string Details
        {
            get { return details; }
            set { details = value; OnPropertyChanged(nameof(Details)); }
        }

        private double area;

        public double Area
        {
            get { return area; }
            set { area = value; OnPropertyChanged(nameof(Area)); }
        }

        private int rooms;

        public int Rooms
        {
            get { return rooms; }
            set { rooms = value; OnPropertyChanged(nameof(Rooms)); }
        }

        private string tenants;

        public string Tenants
        {
            get { return tenants; }
            set { tenants = value; OnPropertyChanged(nameof(Tenants)); }
        }

        public ICommand FinishSetupCommand { get; }
        public ICommand CancelSetupCommand { get; }

        public FlatSetupViewModel(NavigationStore navigationStore)
        {
            FinishSetupCommand = new FinishFlatSetupCommand(navigationStore, this);
            CancelSetupCommand = new CancelFlatSetupCommand(navigationStore);
                
        }
    }
}
