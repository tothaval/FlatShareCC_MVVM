using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMietkosten.Models;
using WGMietkosten.ViewModels;

namespace WGMietkosten.Navigation
{
    class NavigationStore
    {
        private FlatManagement _flatManagement;

        private ViewModelBase _currentViewModel;

        public FlatManagement FlatManager => _flatManagement;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }

        public NavigationStore(ref FlatManagement flatManagement)
        {
                _flatManagement = flatManagement;
        }

        public event Action CurrentViewModelChanged;
    }
}
