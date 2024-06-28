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
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Models;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.ViewModels
{
    public class FlatManagementViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public AccountingViewModel Accounting { get; }


        public CostsViewModel Cost { get; }


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


        public FlatSetupViewModel FlatSetup { get; set; }


        public bool HasFlat => _flatCollection.Count > 0;


        public RoomSetupViewModel RoomSetup { get; set; }


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


        private bool _ShowAccounting;
        public bool ShowAccounting
        {
            get { return _ShowAccounting; }
            set
            {
                _ShowAccounting = value;

                if (_ShowAccounting)
                {
                    ShowCosts = false;
                }
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

                if (_ShowCosts)
                {
                    ShowAccounting = false;
                }
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

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler FlatViewModelChange;
        
        #endregion event properties & fields


        // collections
        #region collections

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

        #endregion collections


        // commands
        #region commands

        public ICommand DeleteFlatCommand { get; }

        public ICommand NewFlatCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection)
        {
            Accounting = new AccountingViewModel(this);
            Cost = new CostsViewModel(Accounting);
            FlatSetup = new FlatSetupViewModel(this);
            RoomSetup = new RoomSetupViewModel(this);

            NewFlatCommand = new RelayCommand((s) => CreateFlat(), (s) => true);

            DeleteFlatCommand = new ExecuteDeleteFlatCommand(flatCollection);

            _flatCollection = flatCollection;

            _flatCollection.CollectionChanged += _flatCollection_CollectionChanged;

            ShowFlatManagement = true;

            SelectFirstFlatCollectionItem();

            LoadData();
        }

        #endregion constructors


        // async methods
        #region async methods

        private async Task<ApplicationData?> GetApplicationData()
        {
            ApplicationData? data = null;

            string folder = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\appdata\\";
            string filter = "*.xml";

            List<string> files = Directory.GetFiles(folder, filter, SearchOption.TopDirectoryOnly).ToList();

            foreach (string file in files)
            {
                string pathlessFile = Path.GetFileName(file);

                if (pathlessFile.Equals("appdata.xml"))
                {
                    var xmlSerializer = new XmlSerializer(typeof(ApplicationData));

                    using (var writer = new StreamReader(file))
                    {
                        try
                        {
                            data = (ApplicationData)xmlSerializer.Deserialize(writer);

                            break;
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return data;
        }


        private async void LoadData()
        {
            Task<ApplicationData?> GetStateData = GetApplicationData();

            ApplicationData? applicationData = await GetStateData;

            if (applicationData != null)
            {
                ShowAccounting = applicationData.Accounting_Shown;

                ShowCosts = applicationData.ShowCosts_Shown;

                ShowFlatManagement = applicationData.FlatManagement_Shown;

                ShowFlatSetup = applicationData.FlatSetup_Shown;

                ShowManual = applicationData.Manual_Shown;
                ShowRoomSetup = applicationData.RoomSetup_Shown;
                ShowSettings = applicationData.Settings_Shown;

                _flatCollection.CollectionChanged += LoadUp;
            }
        }

        #endregion async methods


        // methods
        #region methods

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

        #endregion methods

  
        // events
        #region events

        private void _flatCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            FlatCollectionFilled = FlatCollection.Count > 0;

            SelectFirstFlatCollectionItem();

            OnPropertyChanged(nameof(HasFlat));
        }


        private async void LoadUp(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Task<ApplicationData?> GetStateData = GetApplicationData();

            ApplicationData? applicationData = await GetStateData;

            if (applicationData.FlatViewModelSelectedIndex >= 0
                && FlatCollection.Count > 0 && applicationData.FlatViewModelSelectedIndex < FlatCollection.Count)
            {
                SelectedItem = FlatCollection[applicationData.FlatViewModelSelectedIndex];
                _flatCollection.CollectionChanged -= LoadUp;
            }
        }

        #endregion events


    }
}
// EOF
