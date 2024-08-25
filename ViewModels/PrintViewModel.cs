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
using System.Windows.Media;
using SharedLivingCostCalculator.Utility.PrintViewHelperFunctions;
using SharedLivingCostCalculator.Models.Contract;

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


        private bool _AnnualRentCostsSelected;
        public bool AnnualRentCostsSelected
        {
            get { return _AnnualRentCostsSelected; }
            set
            {
                _AnnualRentCostsSelected = value;
                OnPropertyChanged(nameof(AnnualRentCostsSelected));
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


        private bool _CreditOutputSelected;
        public bool CreditOutputSelected
        {
            get { return _CreditOutputSelected; }
            set
            {
                _CreditOutputSelected = value;
                OnPropertyChanged(nameof(CreditOutputSelected));
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


        private bool _OtherOutputSelected;
        public bool OtherOutputSelected
        {
            get { return _OtherOutputSelected; }
            set
            {
                _OtherOutputSelected = value;
                OnPropertyChanged(nameof(OtherOutputSelected));
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


        private string _SelectedDetailOption;
        public string SelectedDetailOption
        {
            get { return _SelectedDetailOption; }
            set
            {
                _SelectedDetailOption = value;
                OnPropertyChanged(nameof(SelectedDetailOption));
            }
        }


        private int _SelectedYear;
        public int SelectedYear
        {
            get { return _SelectedYear; }
            set
            {
                _SelectedYear = value;
                OnPropertyChanged(nameof(SelectedYear));
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

        public ObservableCollection<string> DetailOptions { get; set; } = new ObservableCollection<string>();


        public ObservableCollection<int> TimeScale { get; set; } = new ObservableCollection<int>();

        #endregion


        #region Commands

        public ICommand CreatePrintOutputCommand { get; }

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

            DetailOptions.Add("TimeScale");
            DetailOptions.Add("ValueChange");

            SelectedDetailOption = "ValueChange";

            RentOutputSelected = true;
            ContractCostsSelected = true;
            RoomAreaDataSelected = true;

            Update();
        }

        #endregion


        // methods
        #region methods

        private void BuildFlowDocument()
        {
            ActiveFlowDocument = new FlowDocument();
            ActiveFlowDocument.TextAlignment = TextAlignment.Left;
            ActiveFlowDocument.PageHeight = 640;
            ActiveFlowDocument.PageWidth = 640;

            if (FlatViewModel != null && SelectedYear > 0)
            {
                PrintOutputBase print = new PrintOutputBase(FlatViewModel, SelectedYear);

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


                if (RoomAreaDataSelected)
                {
                    ActiveFlowDocument.Blocks.Add(print.BuildRoomAreaData()); 
                }

                
                if (BillingOutputSelected)
                {

                    BillingViewModel? billingViewModel = new PrintOutputBase(FlatViewModel, SelectedYear).SearchForBillingViewModel();

                    if (billingViewModel != null)
                    {
                        p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                        ActiveFlowDocument.Blocks.Add(p);

                        p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };

                        p.Inlines.Add(new Run($"Annual Billing {SelectedYear}: ")
                        { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                        p.Inlines.Add(new Run($"{print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                        //p.Style = headerParagraph;
                        ActiveFlowDocument.Blocks.Add(p);


                        BillingPrintOutput billingPrintOutput = new BillingPrintOutput(this, billingViewModel, SelectedYear, TenantSelected);

                        ActiveFlowDocument.Blocks.Add(billingPrintOutput.BuildBillingDetails());
                    }

                }

                if (RentOutputSelected)
                {

                    p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                    ActiveFlowDocument.Blocks.Add(p);

                    p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };

                    p.Inlines.Add(new Run($"Rent Plan Flat {SelectedYear}: ")
                    { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                    p.Inlines.Add(new Run($"{print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                    //p.Style = headerParagraph;
                    ActiveFlowDocument.Blocks.Add(p);


                    RentPrintOutput rentPrintOutput = new RentPrintOutput(this, FlatViewModel, SelectedYear, TenantSelected);

                    ActiveFlowDocument.Blocks.Add(rentPrintOutput.BuildRentDetails(SelectedDetailOption));
                }

                if (OtherOutputSelected)
                {
                    p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                    ActiveFlowDocument.Blocks.Add(p);

                    p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                    p.Inlines.Add(new Run($"Other Costs Plan {SelectedYear}: ")
                    { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                    p.Inlines.Add(new Run($"{print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                    //p.Style = headerParagraph;
                    ActiveFlowDocument.Blocks.Add(p);

                    CreditsAndOtherCostsRentPrintOutput creditsAndOther = new CreditsAndOtherCostsRentPrintOutput(this, FlatViewModel, SelectedYear, TenantSelected);

                    ActiveFlowDocument.Blocks.Add(creditsAndOther.BuildOtherCostDetails(false, SelectedDetailOption));
                }

                if (CreditOutputSelected)
                {
                    p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };
                    ActiveFlowDocument.Blocks.Add(p);

                    p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                    p.Inlines.Add(new Run($"Credit Plan {SelectedYear}: ")
                    { FontWeight = FontWeights.Bold, FontSize = 16.0 });
                    p.Inlines.Add(new Run($"{print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
                    p.Style = headerParagraph;
                    ActiveFlowDocument.Blocks.Add(p);

                    ActiveFlowDocument.Blocks.Add(new CreditsAndOtherCostsRentPrintOutput(this, FlatViewModel, SelectedYear, TenantSelected).BuildOtherCostDetails(true, SelectedDetailOption));
                }

            }
        }


        private void BuildTimeScale()
        {
            TimeScale.Clear();

            foreach (RentViewModel item in _FlatViewModel.RentUpdates)
            {
                if (!TimeScale.Contains(item.StartDate.Year))
                {
                    TimeScale.Add(item.StartDate.Year);
                }
            }

            foreach (BillingViewModel item in _FlatViewModel.AnnualBillings)
            {
                if (!TimeScale.Contains(item.StartDate.Year))
                {
                    TimeScale.Add(item.StartDate.Year);
                }
            }

            TimeScale = new ObservableCollection<int>(TimeScale.OrderBy(i => i));

            if (TimeScale.Count > 0)
            {
                SelectedYear = TimeScale.Last();
            }

            OnPropertyChanged(nameof(SelectedYear));
            OnPropertyChanged(nameof(TimeScale));
        }


        public void Update()
        {
            ActiveFlowDocument = null;

            if (Accounting.FlatViewModel != null)
            {
                _FlatViewModel = Accounting.FlatViewModel;

                BuildTimeScale();

                if (TimeScale.Count > 0)
                {
                    SelectedYear = TimeScale.First();
                }

                ActiveFlowDocument = new FlowDocument();
            }

            if (_FlatManagementViewModel.SelectedItem != null)
            {
                _FlatViewModel = _FlatManagementViewModel.SelectedItem;
            }

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