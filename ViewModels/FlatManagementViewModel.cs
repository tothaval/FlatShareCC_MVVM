/*  Shared Living Cost Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FlatManagementViewModel  : BaseViewModel
 * 
 *  viewmodel
 *  
 *  purpose:
 *      -> display existing flats
 *      -> create, edit, delete flats
 *      -> open Settings view
 *      -> display most recent rent of selected flat
 *      
 *      -> application main view
 */

using SharedLivingCostCalculator.Commands;
using SharedLivingCostCalculator.Interfaces;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.Views.Windows;
using System.Collections.ObjectModel;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SharedLivingCostCalculator.ViewModels
{
    internal class FlatManagementViewModel : BaseViewModel
    {        
        public AccountingViewModel Accounting { get; }

        public CostsViewModel Cost { get; }

        public FlatSetupViewModel FlatSetup { get; set; }


        public RoomSetupViewModel RoomSetup { get; set; }


        public event EventHandler FlatViewModelChange;


        private ObservableCollection<FlatViewModel> _flatCollection;
        public ObservableCollection<FlatViewModel> FlatCollection
        {
            get { return _flatCollection; }
            set
            {
                if (_flatCollection == value) return;
                _flatCollection = value;

                OnPropertyChanged(nameof(FlatCollection));                
            }
        }


        private FlatViewModel _SelectedItem;
        public FlatViewModel SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                if (_SelectedItem == value) return;
                _SelectedItem = value;

                OnPropertyChanged(nameof(SelectedItem));

                if (_flatCollection.Count > 0 && _SelectedItem != null)
                {
                    _SelectedItem.ConnectRooms();
                    _SelectedItem.SetMostRecentCosts();

                    FlatViewModelChange?.Invoke(this, EventArgs.Empty);
                }

            }
        }


        public bool HasFlat => _flatCollection.Count > 0;


        private bool _FlatCollectionFilled;
        public bool FlatCollectionFilled
        {
            get { return _FlatCollectionFilled; }
            set
            {
                _FlatCollectionFilled = value;
                OnPropertyChanged(nameof(FlatCollectionFilled));
            }
        }


        public ICommand NewFlatCommand { get; }


        public ICommand SettingsCommand { get; }


        public ICommand AccountingCommand { get; }


        public ICommand EditFlatCommand { get; }


        public ICommand DeleteFlatCommand { get; }


        public ICommand TriggerVisibilityCommand { get; }


        private bool _ShowAccounting;
        public bool ShowAccounting
        {
            get { return _ShowAccounting; }
            set
            {
                _ShowAccounting = value;
                OnPropertyChanged(nameof(ShowAccounting));
            }
        }


        private bool _ShowCosts;
        public bool ShowCosts
        {
            get { return _ShowCosts; }
            set
            {
                _ShowCosts = value;
                OnPropertyChanged(nameof(ShowCosts));
            }
        }

        private bool _ShowFlatManagement;
        public bool ShowFlatManagement
        {
            get { return _ShowFlatManagement; }
            set
            {
                _ShowFlatManagement = value;

                if (_ShowFlatManagement)
                {
                    ShowManual = false;
                    ShowSettings = false;
                }

                OnPropertyChanged(nameof(ShowFlatManagement));
            }
        }


        private bool _ShowFlatSetup;
        public bool ShowFlatSetup
        {
            get { return _ShowFlatSetup; }
            set
            {
                _ShowFlatSetup = value;
                
                if (_ShowFlatSetup)
                {
                    ShowRoomSetup = false;
                }

                OnPropertyChanged(nameof(ShowFlatSetup));
            }
        }


        private bool _ShowManual;
        public bool ShowManual
        {
            get { return _ShowManual; }
            set
            {
                _ShowManual = value;

                if (_ShowManual)
                {
                    ShowFlatManagement = false;
                    ShowSettings = false;
                }

                OnPropertyChanged(nameof(ShowManual));
            }
        }


        private bool _ShowRoomSetup;
        public bool ShowRoomSetup
        {
            get { return _ShowRoomSetup; }
            set
            {
                _ShowRoomSetup = value;

                if (_ShowRoomSetup)
                {
                    ShowFlatSetup = false;
                }

                OnPropertyChanged(nameof(ShowRoomSetup));
            }
        }


        private bool _ShowSettings;
        public bool ShowSettings
        {
            get { return _ShowSettings; }
            set
            {
                _ShowSettings = value;

                if (_ShowSettings)
                {
                    ShowManual = false;
                    ShowFlatManagement = false;
                }

                OnPropertyChanged(nameof(ShowSettings));
            }
        }


        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection)
        {
            Accounting = new AccountingViewModel(this);
            Cost = new CostsViewModel(Accounting);
            FlatSetup = new FlatSetupViewModel(this);
            RoomSetup = new RoomSetupViewModel(this);

            NewFlatCommand = new RelayCommand((s) => CreateFlat(), (s) => true);

            DeleteFlatCommand = new ExecuteDeleteFlatCommand(flatCollection);

            TriggerVisibilityCommand = new RelayCommand((s) => TriggerVisibility(s), (s) => true);

            _flatCollection = flatCollection;

            _flatCollection.CollectionChanged += _flatCollection_CollectionChanged;

            ShowFlatManagement = true;

            SelectFirstFlatCollectionItem();

        }

        private void CreateFlat()
        {
            FlatViewModel flatViewModel = new FlatViewModel(new Flat());

            flatViewModel.RentUpdates.Add(new RentViewModel(flatViewModel, new Rent()));

            SelectedItem = flatViewModel;

            _flatCollection.Add(flatViewModel);

            ShowFlatSetup = true;
            
            SelectedItem = _flatCollection?.Last();

            FlatCollectionFilled = true;

            OnPropertyChanged(nameof(FlatSetup));
        }


        private void SelectFirstFlatCollectionItem()
        {
            if (_flatCollection.Count > 0)
            {
                SelectedItem = _flatCollection?.First();
                FlatCollectionFilled = true;
            }
        }


        private void TriggerVisibility(object s)
        {
            if (s.GetType() == typeof(string))
            {
                switch (s)
                {
                    case "Accounting":
                        ShowAccounting = !ShowAccounting;
                        break;

                    case "FlatManagement":
                        ShowFlatManagement = !ShowFlatManagement;
                        ShowSettings = false;
                        ShowManual = false;
                        break;

                    case "FlatSetup":
                        ShowFlatSetup = !ShowFlatSetup;
                        ShowRoomSetup = false;
                        break;

                    case "Manual":
                        ShowManual = !ShowManual;
                        ShowFlatManagement = false;
                        ShowSettings = false;
                        break;

                    case "RoomSetup":
                        ShowRoomSetup = !ShowRoomSetup;
                        ShowFlatSetup = false;
                        break;

                    case "Settings":
                        ShowSettings = !ShowSettings;
                        ShowFlatManagement = false;
                        ShowManual = false;
                        break;

                    case "ShowCosts":
                        ShowCosts = !ShowCosts;
                        break;

                    default:
                        break;
                }
            }
        }


        private void _flatCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FlatCollectionFilled = FlatCollection.Count > 0;

            SelectFirstFlatCollectionItem();

            OnPropertyChanged(nameof(HasFlat));
        }


    }
}
// EOF