/*  Shared Living TransactionSum Calculator (by Stephan Kammel, Dresden, Germany, 2024)
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
using SharedLivingCostCalculator.Models.Contract;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.Utility;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace SharedLivingCostCalculator.ViewModels.Contract
{
    public class FlatManagementViewModel : BaseViewModel
    {

        // properties & fields
        #region properties & fields

        public AccountingViewModel Accounting { get; }


        public CostDisplayViewModel Cost { get; }


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
                    _SelectedItem.ActiveTenantListConversion();

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
                    ShowManual = false;
                    ShowFlatManagement = false;
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
                    ShowManual = false;
                    ShowFlatManagement = false;
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
                    ShowAccounting = false;
                    ShowCosts = false;
                    ShowManual = false;
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
                    ShowTenantSetup = false;
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
                    ShowAccounting = false;
                    ShowCosts = false;
                    ShowFlatManagement = false;
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
                    ShowTenantSetup = false;
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

                //if (_ShowSettings)
                //{
                //    ShowAccounting = false;
                //    ShowCosts = false;
                //    ShowManual = false;
                //    ShowFlatManagement = false;
                //}

                OnPropertyChanged(nameof(ShowSettings));
            }
        }


        private bool _ShowTenantSetup;
        public bool ShowTenantSetup
        {
            get { return _ShowTenantSetup; }
            set
            {
                _ShowTenantSetup = value;

                if (_ShowTenantSetup)
                {
                    ShowFlatSetup = false;
                    ShowRoomSetup = false;
                }

                OnPropertyChanged(nameof(ShowTenantSetup));
            }
        }


        public TenantSetupViewModel TenantSetup { get; set; }

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


        public ICommand DuplicateFlatCommand { get; }


        public ICommand NewFlatCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection)
        {
            Accounting = new AccountingViewModel(this);
            Cost = new CostDisplayViewModel(this);
            FlatSetup = new FlatSetupViewModel(this);
            RoomSetup = new RoomSetupViewModel(this);
            TenantSetup = new TenantSetupViewModel(this);

            DuplicateFlatCommand = new RelayCommand((s) => DuplicateFlat(), (s) => true);
            NewFlatCommand = new RelayCommand((s) => CreateFlat(), (s) => true);

            DeleteFlatCommand = new ExecuteDeleteFlatCommand(flatCollection, this);

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



            OnPropertyChanged(nameof(FlatSetup));
            OnPropertyChanged(nameof(RoomSetup));
            OnPropertyChanged(nameof(TenantSetup));
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

        #endregion async methods


        // methods
        #region methods

        private void CreateFlat()
        {
            FlatViewModel flatViewModel = new FlatViewModel(new Flat());

            flatViewModel.RentUpdates.Add(new RentViewModel(flatViewModel, new Rent()));

            _flatCollection.Add(flatViewModel);


            SelectedItem = _flatCollection.First();

            ShowFlatSetup = true;

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(HasFlat));
            OnPropertyChanged(nameof(FlatSetup));
            OnPropertyChanged(nameof(RoomSetup));
            OnPropertyChanged(nameof(TenantSetup));
        }


        private void DuplicateFlat()
        {
            // the object that is to be duplicated must be cloned or value changes
            // will affect the origin object as well. right now, duplicate duplicates
            // all flat data and room data, tenants, tenantConfigurations or rents are
            // not duplicated, if necessary, expand below code to clone other data as well.


            Flat flat = new Flat();

            double area = (double)SelectedItem.Area;
            int roomCount = (int)SelectedItem.RoomCount;
            string address = (string)SelectedItem.Address;
            string details = (string)SelectedItem.Details;
            string flatNotes = (string)SelectedItem.FlatNotes;

            flat.Area = area;
            flat.Address = address;
            flat.Details = details;
            flat.FlatNotes = flatNotes;
            flat.RoomCount = roomCount;

            foreach (RoomViewModel item in SelectedItem.Rooms)
            {
                double roomArea = item.RoomArea;
                string roomName = item.RoomName;

                flat.Rooms.Add(new RoomViewModel(new Room(roomName, roomArea)));
            }

            FlatViewModel flatViewModel = new FlatViewModel(flat, true);
                                                
            SelectedItem = flatViewModel;

            _flatCollection.Add(flatViewModel);

            ShowFlatSetup = true;

            SelectedItem = _flatCollection?.Last();

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(HasFlat));
            OnPropertyChanged(nameof(FlatSetup));
            OnPropertyChanged(nameof(RoomSetup));
            OnPropertyChanged(nameof(TenantSetup));
        }


        private void ResetVisibility()
        {
            if (_flatCollection.Count == 0)
            {
                ShowAccounting = false;
                ShowCosts = false;
                ShowFlatSetup = false;
                ShowRoomSetup = false;
                ShowTenantSetup = false;


                if (Application.Current.MainWindow != null)
                {
                    foreach (Window item in Application.Current.MainWindow.OwnedWindows)
                    {
                        item.Close();
                    } 
                }

                SelectedItem = null;

                FlatSetup = new FlatSetupViewModel(this);
                RoomSetup = new RoomSetupViewModel(this);
                TenantSetup = new TenantSetupViewModel(this);

                OnPropertyChanged(nameof(FlatSetup));
                OnPropertyChanged(nameof(RoomSetup));
                OnPropertyChanged(nameof(TenantSetup));
            }
        }


        private void SelectFirstFlatCollectionItem()
        {
            if (_flatCollection.Count > 0)
            {
                SelectedItem = _flatCollection?.First();
            }
            else
            {
                ResetVisibility();
            }
        }

        #endregion methods


        // events
        #region events

        private void _flatCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectFirstFlatCollectionItem();

            ResetVisibility();

            OnPropertyChanged(nameof(HasFlat));

            OnPropertyChanged(nameof(FlatSetup));
            OnPropertyChanged(nameof(RoomSetup));
            OnPropertyChanged(nameof(TenantSetup));
        }

        #endregion events


    }
}
// EOF
