/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  PrintViewModel  : BaseViewModel
 * 
 *  viewmodel for PrintView
 *  
 *  PrintView generates overviews of rent and billing data
 *  the data is formated via FlowDocument to create an easy to read and copyable output
 *  
 *  the output offers an overview over the costs of the selected period of time
 *  as well as detailed output for every room
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Contract;
using SharedLivingCostCalculator.ViewModels.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using SharedLivingCostCalculator.Commands;
using System.Collections.ObjectModel;
using SharedLivingCostCalculator.Utility.PrintViewHelperFunctions;
using SharedLivingCostCalculator.Enums;
using SharedLivingCostCalculator.Utility;

namespace SharedLivingCostCalculator.ViewModels
{
    public class PrintViewModel : BaseViewModel
    {

        // Properties & Fields
        #region Properties & Fields

        private readonly AccountingViewModel _AccountingViewModel;
        public AccountingViewModel Accounting => _AccountingViewModel;


        private FlowDocument? _ActiveFlowDocument;
        public FlowDocument? ActiveFlowDocument
        {
            get { return _ActiveFlowDocument; }
            set
            {
                _ActiveFlowDocument = value;
                OnPropertyChanged(nameof(ActiveFlowDocument));
            }
        }


        private bool _AllCostsSelected;
        public bool AllCostsSelected
        {
            get { return _AllCostsSelected; }
            set
            {
                _AllCostsSelected = value;
                OnPropertyChanged(nameof(AllCostsSelected));
            }
        }


        private bool _AnnualCostsSelected;
        public bool AnnualCostsSelected
        {
            get { return _AnnualCostsSelected; }
            set
            {
                _AnnualCostsSelected = value;
                OnPropertyChanged(nameof(AnnualCostsSelected));
            }
        }


        private bool _AppendContractCostsDetailsSelected;
        public bool AppendContractCostsDetailsSelected
        {
            get { return _AppendContractCostsDetailsSelected; }
            set
            {
                _AppendContractCostsDetailsSelected = value;
                OnPropertyChanged(nameof(AppendContractCostsDetailsSelected));
            }
        }


        private bool _BillingOutputSelected;
        public bool BillingOutputSelected
        {
            get { return _BillingOutputSelected; }
            set
            {
                _BillingOutputSelected = value;
                OnPropertyChanged(nameof(BillingOutputSelected));
            }
        }


        private BillingViewModel? _BillingViewModel;
        public BillingViewModel? BillingViewModel
        {
            get { return _BillingViewModel; }
            set
            {
                _BillingViewModel = value;
                OnPropertyChanged(nameof(BillingViewModel));
            }
        }


        private bool _ConsumptionSelected;
        public bool ConsumptionSelected
        {
            get { return _ConsumptionSelected; }
            set
            {
                _ConsumptionSelected = value;
                OnPropertyChanged(nameof(ConsumptionSelected));
            }
        }


        private bool _ContractCostsIncludeCreditsSelected;
        public bool ContractCostsIncludeCreditsSelected
        {
            get { return _ContractCostsIncludeCreditsSelected; }
            set
            {
                _ContractCostsIncludeCreditsSelected = value;
                OnPropertyChanged(nameof(ContractCostsIncludeCreditsSelected));
            }
        }


        private bool _ContractCostsSelected;
        public bool ContractCostsSelected
        {
            get { return _ContractCostsSelected; }
            set
            {
                _ContractCostsSelected = value;
                OnPropertyChanged(nameof(ContractCostsSelected));
            }
        }


        private bool _ContractDataOutputSelected;
        public bool ContractDataOutputSelected
        {
            get { return _ContractDataOutputSelected; }
            set
            {
                _ContractDataOutputSelected = value;
                OnPropertyChanged(nameof(ContractDataOutputSelected));
            }
        }


        private bool _CreditOutputSelected;
        public bool CreditOutputSelected
        {
            get { return _CreditOutputSelected; }
            set
            {
                _CreditOutputSelected = value;
                OnPropertyChanged(nameof(CreditOutputSelected));
                OnPropertyChanged(nameof(NonBillingOutputSelected));
            }
        }


        private bool _DataOutputProgressionSelected;
        public bool DataOutputProgressionSelected
        {
            get { return _DataOutputProgressionSelected; }
            set
            {
                _DataOutputProgressionSelected = value;
                OnPropertyChanged(nameof(DataOutputProgressionSelected));
            }
        }


        private bool _DetailedContractItemsSelected;
        public bool DetailedContractCostsSelected
        {
            get { return _DetailedContractItemsSelected; }
            set
            {
                _DetailedContractItemsSelected = value;
                OnPropertyChanged(nameof(DetailedContractCostsSelected));
            }
        }


        private bool _DetailedNonContractItemsSelected;
        public bool DetailedNonContractItemsSelected
        {
            get { return _DetailedNonContractItemsSelected; }
            set
            {
                _DetailedNonContractItemsSelected = value;
                OnPropertyChanged(nameof(DetailedNonContractItemsSelected));
            }
        }


        private bool _DisplaySummarySelected;
        public bool DisplaySummarySelected
        {
            get { return _DisplaySummarySelected; }
            set
            {
                _DisplaySummarySelected = value;
                OnPropertyChanged(nameof(DisplaySummarySelected));
            }
        }


        private FlatManagementViewModel _FlatManagementViewModel;


        private FlatViewModel _FlatViewModel;
        public FlatViewModel FlatViewModel => _FlatViewModel;


        private bool _IncludeTaxesSelected;
        public bool IncludeTaxesSelected
        {
            get { return _IncludeTaxesSelected; }
            set
            {
                _IncludeTaxesSelected = value;
                OnPropertyChanged(nameof(IncludeTaxesSelected));
            }
        }


        public int _LastSelectedYear { get; set; }


        public bool NonBillingOutputSelected => RentOutputSelected || OtherOutputSelected || CreditOutputSelected;



        private bool _OtherOutputSelected;
        public bool OtherOutputSelected
        {
            get { return _OtherOutputSelected; }
            set
            {
                _OtherOutputSelected = value;
                OnPropertyChanged(nameof(OtherOutputSelected));
                OnPropertyChanged(nameof(NonBillingOutputSelected));
            }
        }


        private PrintOutputBase _Print { get; set; }


        private bool _PrintAllSelected;
        public bool PrintAllSelected
        {
            get { return _PrintAllSelected; }
            set
            {
                _PrintAllSelected = value;

                if (value)
                {
                    _PrintExcerptSelected = false;
                    _PrintFlatSelected = false;
                    _PrintRoomsSelected = false;
                }


                OnPropertyChanged(nameof(PrintAllSelected));
                OnPropertyChanged(nameof(PrintExcerptSelected));
                OnPropertyChanged(nameof(PrintFlatSelected));
                OnPropertyChanged(nameof(PrintRoomsSelected));
            }
        }


        private bool _PrintExcerptSelected;
        public bool PrintExcerptSelected
        {
            get { return _PrintExcerptSelected; }
            set
            {
                _PrintExcerptSelected = value;

                PrintAllSelected = false;

                if (value)
                {
                    _PrintRoomsSelected = false;
                    OnPropertyChanged(nameof(PrintRoomsSelected));
                }

                if (value == false && PrintFlatSelected == false && PrintRoomsSelected == false)
                {
                    PrintAllSelected = true;
                }

                OnPropertyChanged(nameof(PrintExcerptSelected));
            }
        }


        private bool _PrintFlatSelected;
        public bool PrintFlatSelected
        {
            get { return _PrintFlatSelected; }
            set
            {
                _PrintFlatSelected = value;

                PrintAllSelected = false;

                if (value == false && PrintExcerptSelected == false && PrintRoomsSelected == false)
                {
                    PrintAllSelected = true;
                }

                OnPropertyChanged(nameof(PrintFlatSelected));
            }
        }


        private bool _PrintItemsPerYearSelected;
        public bool PrintItemsPerYearSelected
        {
            get { return _PrintItemsPerYearSelected; }
            set
            {
                _PrintItemsPerYearSelected = value;

                if (_PrintSelectedItemSelected && value == true)
                {
                    _PrintSelectedItemSelected = false;

                    OnPropertyChanged(nameof(PrintSelectedItemSelected));
                }

                if (_PrintMostRecentSelected && value == true)
                {
                    _PrintMostRecentSelected = false;

                    OnPropertyChanged(nameof(PrintMostRecentSelected));
                }

                OnPropertyChanged(nameof(PrintItemsPerYearSelected));
            }
        }


        private bool _PrintMostRecentSelected;
        public bool PrintMostRecentSelected
        {
            get { return _PrintMostRecentSelected; }
            set
            {
                _PrintMostRecentSelected = value;

                if (_PrintSelectedItemSelected && value == true)
                {
                    _PrintSelectedItemSelected = false;

                    OnPropertyChanged(nameof(PrintSelectedItemSelected));
                }

                if (_PrintItemsPerYearSelected && value == true)
                {
                    _PrintItemsPerYearSelected = false;

                    OnPropertyChanged(nameof(PrintItemsPerYearSelected));
                }

                OnPropertyChanged(nameof(PrintMostRecentSelected));
            }
        }


        private bool _PrintRoomsSelected;
        public bool PrintRoomsSelected
        {
            get { return _PrintRoomsSelected; }
            set
            {
                _PrintRoomsSelected = value;

                PrintAllSelected = false;

                if (value)
                {
                    _PrintExcerptSelected = false;
                    OnPropertyChanged(nameof(PrintExcerptSelected));
                }

                if (value == false && PrintExcerptSelected == false && PrintFlatSelected == false)
                {
                    PrintAllSelected = true;
                }

                OnPropertyChanged(nameof(PrintRoomsSelected));
            }
        }


        private bool _PrintSelectedItemSelected;
        public bool PrintSelectedItemSelected
        {
            get { return _PrintSelectedItemSelected; }
            set
            {
                _PrintSelectedItemSelected = value;


                if (_PrintItemsPerYearSelected && value == true)
                {
                    _PrintItemsPerYearSelected = false;

                    OnPropertyChanged(nameof(PrintItemsPerYearSelected));
                }


                if (_PrintMostRecentSelected && value == true)
                {
                    _PrintMostRecentSelected = false;

                    OnPropertyChanged(nameof(PrintMostRecentSelected));
                }

                OnPropertyChanged(nameof(PrintSelectedItemSelected));
            }
        }


        private bool _PrintWarningsSelected;
        public bool PrintWarningsSelected
        {
            get { return _PrintWarningsSelected; }
            set
            {
                _PrintWarningsSelected = value;
                OnPropertyChanged(nameof(PrintWarningsSelected));
            }
        }


        private bool _RentCostsOnBillingBalanceSelected;
        public bool RentCostsOnBillingBalanceSelected
        {
            get { return _RentCostsOnBillingBalanceSelected; }
            set
            {
                _RentCostsOnBillingBalanceSelected = value;
                OnPropertyChanged(nameof(RentCostsOnBillingBalanceSelected));
            }
        }


        private bool _RentCostsOutputOnBillingSelected;
        public bool RentCostsOutputOnBillingSelected
        {
            get { return _RentCostsOutputOnBillingSelected; }
            set
            {
                _RentCostsOutputOnBillingSelected = value;
                OnPropertyChanged(nameof(RentCostsOutputOnBillingSelected));
            }
        }


        private bool _RentOutputSelected;
        public bool RentOutputSelected
        {
            get { return _RentOutputSelected; }
            set
            {
                _RentOutputSelected = value;
                OnPropertyChanged(nameof(RentOutputSelected));
                OnPropertyChanged(nameof(NonBillingOutputSelected));
            }
        }


        private bool _RoomAreaDataSelected;
        public bool RoomAreaDataSelected
        {
            get { return _RoomAreaDataSelected; }
            set
            {
                _RoomAreaDataSelected = value;
                OnPropertyChanged(nameof(RoomAreaDataSelected));
            }
        }


        private BillingViewModel _SelectedAnnualBilling;
        public BillingViewModel SelectedAnnualBilling
        {
            get { return _SelectedAnnualBilling; }
            set
            {
                _SelectedAnnualBilling = value;
                OnPropertyChanged(nameof(SelectedAnnualBilling));
            }
        }


        private DataOutputProgressionTypes _SelectedDetailOption;
        public DataOutputProgressionTypes SelectedDetailOption
        {
            get { return _SelectedDetailOption; }
            set
            {
                _SelectedDetailOption = value;
                OnPropertyChanged(nameof(SelectedDetailOption));
            }
        }


        private RentViewModel _SelectedRentChange;
        public RentViewModel SelectedRentChange
        {
            get { return _SelectedRentChange; }
            set
            {
                _SelectedRentChange = value;
                OnPropertyChanged(nameof(SelectedRentChange));
            }
        }


        private RoomViewModel _SelectedRoom;
        public RoomViewModel SelectedRoom
        {
            get { return _SelectedRoom; }
            set
            {
                _SelectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
            }
        }


        private int _SelectedYear;
        public int SelectedYear
        {
            get { return _SelectedYear; }
            set
            {
                _SelectedYear = value;

                _LastSelectedYear = value;

                OnPropertyChanged(nameof(SelectedYear));
            }
        }


        private TaxOptionTypes _SelectedTaxOption;
        public TaxOptionTypes SelectedTaxOption
        {
            get { return _SelectedTaxOption; }
            set
            {
                _SelectedTaxOption = value;
                OnPropertyChanged(nameof(SelectedTaxOption));
            }
        }


        private double _TaxValue;
        public double TaxValue
        {
            get { return _TaxValue; }
            set
            {
                _TaxValue = value;
                OnPropertyChanged(nameof(TaxValue));
            }
        }


        private bool _TenantSelected;
        public bool TenantSelected
        {
            get { return _TenantSelected; }
            set
            {
                _TenantSelected = value;
                OnPropertyChanged(nameof(TenantSelected));
            }
        }

        #endregion


        #region Collections

        public ObservableCollection<int> TimeScale { get; set; } = new ObservableCollection<int>();

        #endregion


        #region Commands

        public ICommand CreatePrintOutputCommand { get; }


        public ICommand ResetMenuCommand { get; }

        #endregion


        // Constructors
        #region Constructors

        public PrintViewModel(FlatManagementViewModel flatManagementViewModel)
        {
            _FlatManagementViewModel = flatManagementViewModel;

            if (_FlatManagementViewModel.SelectedItem != null)
            {
                _FlatViewModel = flatManagementViewModel.SelectedItem;
                _FlatManagementViewModel.PropertyChanged += _FlatManagementViewModel_PropertyChanged;
            }

            _AccountingViewModel = flatManagementViewModel.Accounting;

            Accounting.AccountingChanged += _AccountingViewModel_AccountingChanged;

            CreatePrintOutputCommand = new RelayCommand((s) => BuildFlowDocument(), (s) => true);
            ResetMenuCommand = new RelayCommand((s) => ResetMenu(), (s) => true);

            ResetMenu();

            Update();
        }

        #endregion


        // methods
        #region methods

        private void BuildFlowDocument()
        {
            // there is a memory allocation happening if print button is repeatedly clicked.
            // this shall serve as a reminder to the fact, besides the issue already being written on my todo
            // only rent, credit and other cost print output affected,
            // as of now, i am unsure where the cause is located, because the flow document is newly initiaded
            // on every click, gonnna look into this every now and then, as of now it is not critical,
            // the allocation is 2-20 MB per click, beginning at 90 - 100 MB after start of the application
            // it was a lot higher not so long ago, but it is still too much, considering the fact that 
            // the objects should dissapear completely or being garbage collected on each click

            ActiveFlowDocument = new FlowDocument() { FontFamily = new System.Windows.Media.FontFamily("Verdana")};
            ActiveFlowDocument.TextAlignment = TextAlignment.Left;
            ActiveFlowDocument.PageHeight = 640;
            ActiveFlowDocument.PageWidth = 640;

            if (FlatViewModel != null && SelectedYear > 0)
            {

                Style textParagraph = new Style();
                Style headerParagraph = new Style();

                try
                {
                    textParagraph = Application.Current.FindResource("TextParagraph") as Style;
                    headerParagraph = Application.Current.FindResource("HeaderParagraph") as Style;

                }
                catch (Exception)
                {

                }

                Paragraph p;
                p = new Paragraph(new Run("Shared Living Cost Calculator: print output")) { Style = headerParagraph };
                ActiveFlowDocument.Blocks.Add(p);
                //ActiveFlowDocument.Blocks.Add(BuildAddressDetails(headerParagraph, textParagraph));

                if (BillingOutputSelected)
                {
                    BillingViewModel? billingViewModel = null;


                    if (PrintMostRecentSelected)
                    {
                        billingViewModel = new Compute().SearchForMostRecentBillingViewModel(FlatViewModel);
                    }
                    else if (PrintSelectedItemSelected)
                    {
                        if (SelectedAnnualBilling != null)
                        {
                            billingViewModel = SelectedAnnualBilling; 
                        }
                    }
                    else
                    {
                        billingViewModel =new Compute().SearchForBillingViewModel(FlatViewModel,SelectedYear);
                    }

                    if (billingViewModel != null)
                    {
                        BillingPrintOutput billingPrintOutput = new BillingPrintOutput(this, billingViewModel, SelectedYear, TenantSelected);

                        ActiveFlowDocument.Blocks.Add(billingPrintOutput.BuildBillingDetails());
                    }

                }

                if (RentOutputSelected)
                {
                    RentPrintOutput rentPrintOutput = new RentPrintOutput(this, FlatViewModel, SelectedYear, TenantSelected);

                    ActiveFlowDocument.Blocks.Add(rentPrintOutput.BuildRentDetails(SelectedDetailOption));
                }

                if (OtherOutputSelected)
                {
                    CreditsAndOtherCostsRentPrintOutput creditsAndOther = new CreditsAndOtherCostsRentPrintOutput(this, FlatViewModel, SelectedYear, TenantSelected, false);

                    ActiveFlowDocument.Blocks.Add(creditsAndOther.BuildOtherCostDetails(SelectedDetailOption));
                }

                if (CreditOutputSelected)
                {
                    CreditsAndOtherCostsRentPrintOutput creditsAndOther = new CreditsAndOtherCostsRentPrintOutput(this, FlatViewModel, SelectedYear, TenantSelected, true);

                    ActiveFlowDocument.Blocks.Add(creditsAndOther.BuildOtherCostDetails(SelectedDetailOption));
                }

            }
        }


        private void BuildTimeScale()
        {
            TimeScale.Clear();

            if (FlatViewModel != null)
            {
                TimeScale.Add(FlatViewModel.InitialRent.StartDate.Year);

                foreach (RentViewModel item in FlatViewModel.RentUpdates)
                {
                    if (!TimeScale.Contains(item.StartDate.Year))
                    {
                        TimeScale.Add(item.StartDate.Year);
                    }
                }

                foreach (BillingViewModel item in FlatViewModel.AnnualBillings)
                {
                    if (!TimeScale.Contains(item.Year))
                    {
                        TimeScale.Add(item.Year);
                    }
                }

                TimeScale = new ObservableCollection<int>(TimeScale.OrderBy(i => i));

                if (FlatViewModel.RentUpdates.Count > 0)
                {
                    for (int i = FlatViewModel.InitialRent.StartDate.Year; i < FlatViewModel.RentUpdates.Last().StartDate.Year; i++)
                    {
                        if (!TimeScale.Contains(i))
                        {
                            TimeScale.Add(i);
                        }
                    }
                }

                if (FlatViewModel.AnnualBillings.Count > 0)
                {
                    for (int i = FlatViewModel.InitialRent.StartDate.Year; i < FlatViewModel.AnnualBillings.Last().StartDate.Year; i++)
                    {
                        if (!TimeScale.Contains(i))
                        {
                            TimeScale.Add(i);
                        }
                    }
                }

                TimeScale = new ObservableCollection<int>(TimeScale.OrderBy(i => i));

                if (TimeScale.Count > 0)
                {
                    if (_LastSelectedYear != 0)
                    {
                        SelectedYear = _LastSelectedYear;
                    }
                    else
                    {
                        SelectedYear = TimeScale.Last();
                    }
                }
            }

            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(TimeScale));
        }


        private void ResetMenu()
        {
            // default
            ConsumptionSelected = true;

            ContractCostsSelected = true;

            ContractDataOutputSelected = true;

            RentOutputSelected = true;

            RoomAreaDataSelected = true;

            PrintAllSelected = true;

            SelectedDetailOption = DataOutputProgressionTypes.ValueChange;


            // deactivate
            AllCostsSelected = false;

            AnnualCostsSelected = false;

            BillingOutputSelected = false;

            ContractCostsIncludeCreditsSelected = false;

            CreditOutputSelected = false;

            DataOutputProgressionSelected = false;

            DetailedNonContractItemsSelected = false;

            DisplaySummarySelected = false;

            IncludeTaxesSelected = false;

            PrintWarningsSelected = false;

            OtherOutputSelected = false;

            RentCostsOnBillingBalanceSelected = false;

            RentCostsOutputOnBillingSelected = false;

            TenantSelected = false;
        }


        public void Update()
        {
            // there is a memory allocation happening if print button is repeatedly clicked.
            // this is not the fix for that, but shall serve as a reminder to also look out for events as possible cause.
            // only rent, credit and other cost print output affected, as of now, i am unsure where the cause is located.
            if (_FlatManagementViewModel != null)
            {
                _FlatManagementViewModel.PropertyChanged -= _FlatManagementViewModel_PropertyChanged;
                _FlatManagementViewModel.PropertyChanged += _FlatManagementViewModel_PropertyChanged;
            }

            ActiveFlowDocument = null;

            if (Accounting.FlatViewModel != null)
            {
                _FlatViewModel = Accounting.FlatViewModel;

                ActiveFlowDocument = new FlowDocument();
            }

            if (_FlatManagementViewModel.SelectedItem != null)
            {
                _FlatViewModel = _FlatManagementViewModel.SelectedItem;
            }

            _Print = new PrintOutputBase(this, FlatViewModel, SelectedYear);

            BuildTimeScale();

            OnPropertyChanged(nameof(BillingViewModel));
            OnPropertyChanged(nameof(FlatViewModel));
            OnPropertyChanged(nameof(ActiveFlowDocument));
            OnPropertyChanged(nameof(SelectedYear));
        }

        #endregion methods


        // events
        #region events

        private void _AccountingViewModel_AccountingChanged(object? sender, EventArgs e)
        {
            Accounting.Rents.SelectedItemChange -= Rents_SelectedItemChange;

            Accounting.Rents.SelectedItemChange += Rents_SelectedItemChange;

            Update();
        }


        private void _FlatManagementViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Update();
        }


        private void Rents_SelectedItemChange(object? sender, EventArgs e)
        {
            Update();
        }

        #endregion events


    }
}
// EOF