/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  FlatManagementViewModel  : BaseViewModel
 * 
 *  viewmodel
 *  
 *  purpose:
 *      -> display existing flats
 *      -> create, edit, delete flats
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


        private BillingPeriodViewModel _AnnualBilling;
        public BillingPeriodViewModel AnnualBilling
        {
            get { return _AnnualBilling; }
            set
            {
                _AnnualBilling = value;
                OnPropertyChanged(nameof(AnnualBilling));
            }
        }


        public FlatSetupViewModel FlatSetup { get; set; }


        public bool HasFlat => _FlatCollection.Count > 0;


        public InitialRentSetupViewModel InitialRentSetup { get; set; }


        private bool _InitialValuesFinalized;
        public bool InitialValuesFinalized
        {
            get { return _InitialValuesFinalized; }
            set
            {
                _InitialValuesFinalized = value;
                OnPropertyChanged(nameof(InitialValuesFinalized));
            }
        }


        public PrintViewModel Print { get; }


        public RoomSetupViewModel RoomSetup { get; set; }


        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
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

                if (_FlatCollection.Count > 0 && _SelectedItem != null)
                {
                    _SelectedItem.ConnectRooms();
                    _SelectedItem.ActiveTenantListConversion();

                    FlatViewModelChange?.Invoke(this, EventArgs.Empty);

                    InitialRentSetup = new InitialRentSetupViewModel(_SelectedItem.InitialRent);

                    _SelectedItem.UseRoomCosts4InitialRent();

                    if (_SelectedItem.InitialRent.Rent.UseRoomCosts4InitialRent)
                    {
                        UseRoomCostsChecked = true;
                    }
                    else
                    {
                        UseFlatCostsChecked = true;
                    }

                    InitialValuesFinalized = !SelectedItem.InitialValuesFinalized;

                    OnPropertyChanged(nameof(InitialRentSetup));
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
                    ShowAnnualBilling = false;
                    ShowManual = false;
                    ShowFlatManagement = false;
                    ShowPrintView = false;
                }
                OnPropertyChanged(nameof(ShowAccounting));
            }
        }


        private bool _ShowAnnualBilling;
        public bool ShowAnnualBilling
        {
            get { return _ShowAnnualBilling; }
            set
            {
                _ShowAnnualBilling = value;

                if (_ShowAnnualBilling)
                {
                    ShowAccounting = false;
                    ShowFlatManagement = false;
                    ShowManual = false;
                    ShowPrintView = false;
                }
                OnPropertyChanged(nameof(ShowAnnualBilling));
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
                    ShowAnnualBilling = false;
                    ShowManual = false;
                    ShowPrintView = false;
                }

                OnPropertyChanged(nameof(ShowFlatManagement));
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
                    ShowAnnualBilling = false;
                    ShowFlatManagement = false;
                    ShowPrintView = false;
                }

                OnPropertyChanged(nameof(ShowManual));
            }
        }


        private bool _ShowPrintView;
        public bool ShowPrintView
        {
            get { return _ShowPrintView; }
            set
            {

                if (value == true && SelectedItem != null)
                {
                    SelectedItem.InitialRent.RecalculateRoomCosts();

                    foreach (RentViewModel item in SelectedItem.RentUpdates)
                    {
                        item.RecalculateRoomCosts();
                    }

                    Print.Update();
                }

                _ShowPrintView = value;

                if (_ShowPrintView)
                {
                    ShowAccounting = false;
                    ShowAnnualBilling = false;
                    ShowFlatManagement = false;
                    ShowManual = false;



                }
                OnPropertyChanged(nameof(ShowPrintView));
            }
        }


        private bool _ShowSettings;
        public bool ShowSettings
        {
            get { return _ShowSettings; }
            set
            {
                _ShowSettings = value;

                OnPropertyChanged(nameof(ShowSettings));
            }
        }


        public TenantAssignmentViewModel TenantAssignment { get; set; }


        public TenantSetupViewModel TenantSetup { get; set; }


        private bool _UseFlatCostsChecked;
        public bool UseFlatCostsChecked
        {
            get { return _UseFlatCostsChecked; }
            set
            {
                _UseFlatCostsChecked = value;

                if (SelectedItem != null)
                {
                    SelectedItem.InitialRent.NoDataLock = value;
                }

                if (UseRoomCostsChecked)
                {
                    UseRoomCostsChecked = false;
                }


                OnPropertyChanged(nameof(UseFlatCostsChecked));
            }
        }


        private bool _UseRoomCostsChecked;
        public bool UseRoomCostsChecked
        {
            get { return _UseRoomCostsChecked; }
            set
            {
                _UseRoomCostsChecked = value;

                _SelectedItem.InitialRent.Rent.UseRoomCosts4InitialRent = value;

                if (UseFlatCostsChecked)
                {
                    UseFlatCostsChecked = false;
                }

                OnPropertyChanged(nameof(UseRoomCostsChecked));
            }
        }

        #endregion properties & fields


        // event properties & fields
        #region event properties & fields

        public event EventHandler FlatViewModelChange;

        #endregion event properties & fields


        // collections
        #region collections

        private ObservableCollection<FlatViewModel> _FlatCollection;
        public ObservableCollection<FlatViewModel> FlatCollection
        {
            get { return _FlatCollection; }
            set
            {
                if (_FlatCollection == value) return;
                _FlatCollection = value;

                OnPropertyChanged(nameof(FlatCollection));
            }
        }

        #endregion collections


        // commands
        #region commands

        public ICommand DeleteFlatCommand { get; }


        public ICommand FinalizeInitialFlatDataSetupCommand { get; }


        public ICommand LeftPressCommand { get; }


        public ICommand NewFlatCommand { get; }


        public ICommand UseFlatCostsCommand { get; }


        public ICommand UseRoomCostsCommand { get; }

        #endregion commands


        // constructors
        #region constructors

        public FlatManagementViewModel(ObservableCollection<FlatViewModel> flatCollection)
        {
            Accounting = new AccountingViewModel(this);
            FlatSetup = new FlatSetupViewModel(this);
            Print = new PrintViewModel(this);
            RoomSetup = new RoomSetupViewModel(this);
            TenantAssignment = new TenantAssignmentViewModel(this);
            TenantSetup = new TenantSetupViewModel(this);

            NewFlatCommand = new RelayCommand((s) => CreateFlat(), (s) => true);

            DeleteFlatCommand = new ExecuteDeleteFlatCommand(flatCollection, this);

            FinalizeInitialFlatDataSetupCommand = new RelayCommand((s) => FinalizeDataSetup(), (s) => true);
            
            LeftPressCommand = new RelayCommand((s) => Move(), (s) => true);

            UseFlatCostsCommand = new RelayCommand((s) => UseFlatCosts(), (s) => true);

            UseRoomCostsCommand = new RelayCommand((s) => UseRoomCosts(), (s) => true);

            _FlatCollection = flatCollection;

            _FlatCollection.CollectionChanged += _flatCollection_CollectionChanged;

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

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

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

                ShowAnnualBilling = applicationData.AnnualBilling_Shown;

                ShowFlatManagement = applicationData.FlatManagement_Shown;

                ShowManual = applicationData.Manual_Shown;

                ShowPrintView = applicationData.Print_Shown;

                ShowSettings = applicationData.Settings_Shown;

                _FlatCollection.CollectionChanged += LoadUp;
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

                // to refire print calculation if it has not happend the first time when set in the LoadData method
                ShowPrintView = ShowPrintView;

                _FlatCollection.CollectionChanged -= LoadUp;
            }
        }

        #endregion async methods


        // methods
        #region methods

        private void CreateFlat()
        {
            FlatViewModel flatViewModel = new FlatViewModel(new Flat());

            flatViewModel.RentUpdates.Add(new RentViewModel(flatViewModel, new Rent()));

            flatViewModel.HasDataLock = true;

            _FlatCollection.Add(flatViewModel);

            SelectedIndex = FlatCollection.Count;

            OnPropertyChanged(nameof(FlatCollection));

            //SelectFirstFlatCollectionItem();

            OnPropertyChanged(nameof(SelectedItem));
            OnPropertyChanged(nameof(HasFlat));
            OnPropertyChanged(nameof(FlatSetup));
            OnPropertyChanged(nameof(RoomSetup));
            OnPropertyChanged(nameof(TenantSetup));
        }


        private void FinalizeDataSetup()
        {
            if (SelectedItem != null)
            {
                if (SelectedItem.InitialValuesFinalized == false)
                {
                    SelectedItem.InitialValuesFinalized = true;

                    InitialValuesFinalized = !SelectedItem.InitialValuesFinalized;
                }
            }
        }


        private void Move()
        {
            Application.Current.MainWindow.DragMove();
        }


        private void ResetVisibility()
        {
            if (_FlatCollection.Count == 0)
            {
                ShowAccounting = false;
                ShowPrintView = false;

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
            if (_FlatCollection.Count > 0)
            {
                SelectedIndex = 0;

                //SelectedItem = FlatCollection.Last();

                AnnualBilling = new BillingPeriodViewModel(this, SelectedItem);
            }
            else
            {
                ResetVisibility();
            }
        }


        private void UseFlatCosts()
        {
            UseRoomCostsChecked = false;
            UseFlatCostsChecked = true;

            if (SelectedItem != null)
            {
                SelectedItem.UseRoomCosts4InitialRent();
            }
        }


        private void UseRoomCosts()
        {
            UseFlatCostsChecked = false;
            UseRoomCostsChecked = true;

            if (SelectedItem != null)
            {
                SelectedItem.UseRoomCosts4InitialRent();
            }
        }

        #endregion methods


        // events
        #region events

        private void _flatCollection_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SelectFirstFlatCollectionItem();

            OnPropertyChanged(nameof(HasFlat));
            OnPropertyChanged(nameof(FlatSetup));
            OnPropertyChanged(nameof(RoomSetup));
            OnPropertyChanged(nameof(TenantSetup));
        }

        #endregion events


    }
}
// EOF
