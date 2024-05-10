using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGMietkosten.Exceptions;

namespace WGMietkosten.Models
{
    class FlatManagement
    {
        private ObservableCollection<FlatSetup> _flats;

        public FlatManagement()
        {
            _flats = new ObservableCollection<FlatSetup>();
        }

        public IEnumerable<FlatSetup> GetFlats() { return _flats; }

        public void AddFlat(FlatSetup flatSetup)
        {
            foreach (FlatSetup existingFlat in _flats)
            {
                if (existingFlat.Conflicts(flatSetup))
                {
                    throw new FlatManagementConflictException(existingFlat, flatSetup);
                }
            }

            _flats.Add(flatSetup);
        }

        public void RemoveFlat(int selectedIndex)
        {
            if (selectedIndex > 0 && selectedIndex < _flats.Count)
            {
                _flats.RemoveAt(selectedIndex);
            }
        }

        public void RemoveFlat(FlatSetup flatSetup)
        {
            _flats.Remove(flatSetup);
        }
    }
}
