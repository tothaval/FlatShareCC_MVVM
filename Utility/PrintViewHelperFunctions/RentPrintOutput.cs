/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  RentPrintOutput
 * 
 *  helper class for creating rent related output in PrintViewModel
 */
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.Enums;
using System;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class RentPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private FlatViewModel _FlatViewModel { get; }


        private PrintOutputBase _Print { get; }


        private PrintViewModel _PrintViewModel { get; }


        private int _SelectedYear { get; }


        private bool _ShowTenant { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public RentPrintOutput(PrintViewModel printViewModel, FlatViewModel flatViewModel, int selectedYear, bool showTenant)
        {
            _PrintViewModel = printViewModel;
            _FlatViewModel = flatViewModel;
            _SelectedYear = selectedYear;
            _ShowTenant = showTenant;

            _Print = new PrintOutputBase(_PrintViewModel, _FlatViewModel, _SelectedYear);
        }

        #endregion


        // Methods
        #region Methods

        private TableRowGroup AddDateInformationToSeparatorText(TableRowGroup dataRowGroup, RentViewModel viewModel, int month, string separatorText)
        {
            if (month == -1 && viewModel.StartDate.Year == _PrintViewModel.SelectedYear)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow($"{separatorText}\t> {viewModel.StartDate.Month}/{_PrintViewModel.SelectedYear}"));
            }
            else if (month == -1 && viewModel.StartDate.Year < _PrintViewModel.SelectedYear)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow($"{separatorText}\t> 1/{_PrintViewModel.SelectedYear}"));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow($"{separatorText}\t{month}/{_PrintViewModel.SelectedYear}"));
            }

            return dataRowGroup;
        }


        private TableRowGroup AllCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup = AddDateInformationToSeparatorText(
                    dataRowGroup,
                    viewModel,
                    month,
                    "all costs"
                    );

                dataRowGroup = ContractCostsDisplay(dataRowGroup, viewModel, month);

                if (!_PrintViewModel.DetailedNonContractItemsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel.OtherFTISum, "other", ""));
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(-1 * viewModel.CreditSum, "credit", ""));
                }
                else
                {
                    foreach (FinancialTransactionItemRentViewModel item in viewModel.FinancialTransactionItemViewModels)
                    {
                        dataRowGroup.Rows.Add(_Print.OutputTableRow(
                            item.TransactionSum,
                            item.TransactionItem,
                            item.TransactionShareTypes.ToString()));
                    }

                    foreach (FinancialTransactionItemRentViewModel item in viewModel.Credits)
                    {
                        dataRowGroup.Rows.Add(
                            _Print.OutputTableRow(
                                -1 * item.TransactionSum,
                                item.TransactionItem,
                                item.TransactionShareTypes.ToString()));
                    }
                }

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsAndCredits, false);

                if (_PrintViewModel.AnnualCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year"));

                    dataRowGroup = ContractCostsDisplayUntilEndOfYear(dataRowGroup, viewModel, month);

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel.FirstYearOtherFTISum, "other", ""));

                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, -1, viewModel.FirstYearCompleteCosts, false);
                }
            }
            else
            {
                dataRowGroup = AddDateInformationToSeparatorText(
                    dataRowGroup,
                    viewModel,
                    month,
                    "all costs summary"
                    );

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsAndCredits, true);
            }

            return dataRowGroup;
        }


        private TableRowGroup AllCostsRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            RoomViewModel thisRoom = new RoomViewModel(roomCostShareRent.Room, _FlatViewModel);

            bool printThisRoom = new Compute().PrintThisRoom(_PrintViewModel, thisRoom);

            if (printThisRoom)
            {
                if (!_PrintViewModel.DisplaySummarySelected)
                {
                    dataRowGroup = AddDateInformationToSeparatorText(
                        dataRowGroup,
                        viewModel,
                        month,
                        "all costs"
                        );

                    dataRowGroup = ContractCostsRoomsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);

                    if (!_PrintViewModel.DetailedNonContractItemsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.OutputTableRow(roomCostShareRent.OtherCostsShare, "other costs", ""));

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(-1 * roomCostShareRent.CreditShare, "credit", ""));
                    }
                    else
                    {
                        foreach (FinancialTransactionItemRentViewModel item in roomCostShareRent.FinancialTransactionItemViewModels)
                        {
                            dataRowGroup.Rows.Add(_Print.OutputTableRow(item.TransactionSum, item.TransactionItem, item.TransactionShareTypes.ToString()));

                        }
                        foreach (FinancialTransactionItemRentViewModel item in roomCostShareRent.Credits)
                        {
                            dataRowGroup.Rows.Add(_Print.OutputTableRow(-1 * item.TransactionSum, item.TransactionItem, item.TransactionShareTypes.ToString()));
                        }
                    }

                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, false);

                    if (_PrintViewModel.AnnualCostsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year"));

                        dataRowGroup = ContractCostsRoomsDisplayUntilEndOfYear(dataRowGroup, viewModel, roomCostShareRent, month);

                        dataRowGroup.Rows.Add(_Print.OutputTableRow(roomCostShareRent.FirstYearOtherCostsShare, "other", ""));

                        dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.FirstYearCompleteCostShare, false);
                    }
                }
                else
                {
                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, true);
                }
            }

            return dataRowGroup;
        }


        /// <summary>
        /// prints output rows according to selected data output progression
        /// and adds them to the documentContext
        /// </summary>
        /// <param name="documentContext">the context within the flow document where this should be added</param>
        /// <param name="RentList"></param>
        /// <param name="SelectedDetailOption"></param>
        /// <param name="iterator"></param>
        /// <returns>documentContext</returns>
        private Section BuildDataOutputProgression(Section documentContext, ObservableCollection<RentViewModel> RentList, DataOutputProgressionTypes SelectedDetailOption, int iterator, bool isFlat)
        {
            if (SelectedDetailOption == DataOutputProgressionTypes.TimeChange)
            {
                for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                {
                    if (iterator + 1 < RentList.Count)
                    {
                        // später im Ablauf, um Nachfolger zu berücksichtigen

                        if (RentList[iterator + 1].StartDate.Month - 1 < monthCounter)
                        {
                            break;
                        }

                        if (RentList[iterator].StartDate.Year < _SelectedYear)
                        {
                            documentContext = WriteRentDataToFlowDocument(documentContext, RentList, iterator, monthCounter);
                        }
                        else if (RentList[iterator].StartDate.Year == _SelectedYear
                            && monthCounter >= RentList[iterator].StartDate.Month)
                        {
                            documentContext = WriteRentDataToFlowDocument(documentContext, RentList, iterator, monthCounter);
                        }

                    }
                    else
                    {
                        if (RentList[iterator].StartDate.Year == _SelectedYear && monthCounter < RentList[iterator].StartDate.Month)
                        {
                            continue;
                        }
                        else
                        {
                            documentContext = WriteRentDataToFlowDocument(documentContext, RentList, iterator, monthCounter);
                        }

                    }

                }
            }
            else
            {
                documentContext = WriteRentDataToFlowDocument(documentContext, RentList, iterator, -1);
            }

            return documentContext;
        }


        public Section BuildRentDetails(DataOutputProgressionTypes SelectedDetailOption)
        {
            Section rentOutput = new Section();

            rentOutput.BreakPageBefore = true;

            ObservableCollection<RentViewModel> RentList = _Print.FillRentList();
                
            // adds an overview of all found items
            for (int i = 0; i < RentList.Count; i++)
            {
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(rentOutput, RentList, i, "Rent Change Found", true);

                if (valueChangeHeader == null)
                {
                    continue;
                }

                rentOutput = valueChangeHeader;
            }

            // displays the contents of each item according to options set in print menu
            for (int i = 0; i < RentList.Count; i++)
            {
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(rentOutput, RentList, i, "Rent Change", false);

                if (valueChangeHeader == null)
                {
                    continue;
                }

                rentOutput = valueChangeHeader;

                rentOutput = BuildDataOutputProgression(rentOutput, RentList, SelectedDetailOption, i, true);
            }


            return rentOutput;
        }


        private TableRowGroup ContractCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup = AddDateInformationToSeparatorText(
                    dataRowGroup,
                    viewModel,
                    month,
                    "contract costs"
                    );


                dataRowGroup = ContractCostsDisplay(dataRowGroup, viewModel, month);

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsTotal, false);

                if (_PrintViewModel.AnnualCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year"));

                    dataRowGroup = ContractCostsDisplayUntilEndOfYear(dataRowGroup, viewModel, month);

                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.FirstYearCostsTotal, false);
                }
            }
            else
            {
                dataRowGroup = AddDateInformationToSeparatorText(
                    dataRowGroup,
                    viewModel,
                    month,
                    "contract costs summary"
                    );

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsTotal, true);
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsDetailsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (_PrintViewModel.AppendContractCostsDetailsSelected)
            {
                dataRowGroup = AddDateInformationToSeparatorText(
                    dataRowGroup,
                    viewModel,
                    month,
                    "contract costs advance details"
                    );
            }

            if (_PrintViewModel.AppendContractCostsDetailsSelected || _PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                                        viewModel.ProRataCostsAdvance,
                                        viewModel.Rent.ProRataCostsAdvance.TransactionItem,
                                        viewModel.Rent.ProRataCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel.BasicHeatingCostsAdvance,
                        viewModel.Rent.BasicHeatingCostsAdvance.TransactionItem,
                        viewModel.Rent.BasicHeatingCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel.ConsumptionHeatingCostsAdvance,
                        viewModel.Rent.ConsumptionHeatingCostsAdvance.TransactionItem,
                        viewModel.Rent.ConsumptionHeatingCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel.ColdWaterCostsAdvance,
                        viewModel.Rent.ColdWaterCostsAdvance.TransactionItem,
                        viewModel.Rent.ColdWaterCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel.WarmWaterCostsAdvance,
                        viewModel.Rent.WarmWaterCostsAdvance.TransactionItem,
                        viewModel.Rent.WarmWaterCostsAdvance.TransactionShareTypes.ToString()));
            }

            return dataRowGroup;
        }



        /// <summary>
        /// creates table header and cold rent and advance rows in print output
        /// for the entire flat.
        /// </summary>
        /// <param name="dataRowGroup">the surrounding table context</param>
        /// <param name="viewModel">the viewmodel that shall be displayed</param>
        /// <param name="month">-1 if ValueChange is selected as data output progression, else the month that shall be displayed</param>
        /// <returns>the modified surrounding table context</returns>
        private TableRowGroup ContractCostsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowHeader(true));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel.ColdRent,
                    viewModel.Rent.ColdRent.TransactionItem,
                    viewModel.Rent.ColdRent.TransactionShareTypes.ToString()));

            if (!_PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel.Advance,
                        viewModel.Rent.Advance.TransactionItem,
                        ""));
            }
            else
            {
                if (!_PrintViewModel.AppendContractCostsDetailsSelected)
                {
                    dataRowGroup = ContractCostsDetailsDisplay(dataRowGroup, viewModel, month);
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.OutputTableRow(
                            viewModel.Advance,
                            viewModel.Rent.Advance.TransactionItem,
                            ""));
                }
            }

            return dataRowGroup;
        }


        /// <summary>
        /// creates table header, cold rent and advance rows in print output
        /// for the entire flat, displaying costs until the end of the selected year
        /// 
        /// on TimeChange data output progression selected, the displayed values
        /// will currently not shrink with higher month value, as one might expect.
        /// </summary>
        /// <param name="dataRowGroup">the surrounding table context</param>
        /// <param name="viewModel">the viewmodel that shall be displayed</param>
        /// <param name="month">-1 if ValueChange is selected as data output progression, else the month that shall be displayed</param>
        /// <returns>the modified surrounding table context</returns>
        private TableRowGroup ContractCostsDisplayUntilEndOfYear(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowHeader(true));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel.FirstYearRent,
                    viewModel.Rent.ColdRent.TransactionItem,
                    viewModel.Rent.ColdRent.TransactionShareTypes.ToString()));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel.FirstYearAdvance,
                    viewModel.Rent.Advance.TransactionItem,
                    ""));

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRooms(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            RoomViewModel thisRoom = new RoomViewModel(roomCostShareRent.Room, _FlatViewModel);

            bool printThisRoom = new Compute().PrintThisRoom(_PrintViewModel, thisRoom);

            if (printThisRoom)
            {
                if (!_PrintViewModel.DisplaySummarySelected)
                {
                    dataRowGroup = AddDateInformationToSeparatorText(
                        dataRowGroup,
                        viewModel,
                        month,
                        "contract costs"
                        );

                    dataRowGroup = ContractCostsRoomsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);

                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, false);

                    if (_PrintViewModel.AnnualCostsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year"));

                        dataRowGroup = ContractCostsRoomsDisplayUntilEndOfYear(dataRowGroup, viewModel, roomCostShareRent, month);

                        dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.FirstYearContractCostsShare, false);
                    }
                }
                else
                {
                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, true);
                }
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDetailsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            if (_PrintViewModel.AppendContractCostsDetailsSelected)
            {
                dataRowGroup = AddDateInformationToSeparatorText(
                    dataRowGroup,
                    viewModel,
                    month,
                    "contract costs advance details"
                    );

                dataRowGroup.Rows.Add(_Print.TableRowHeader(true));
            }

            if (_PrintViewModel.AppendContractCostsDetailsSelected || _PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        roomCostShareRent.ProRataAdvanceShare,
                        viewModel.Rent.ProRataCostsAdvance.TransactionItem,
                        viewModel.Rent.ProRataCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        roomCostShareRent.BasicHeatingCostsAdvanceShare,
                        viewModel.Rent.BasicHeatingCostsAdvance.TransactionItem,
                        viewModel.Rent.BasicHeatingCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        roomCostShareRent.ConsumptionHeatingCostsAdvanceShare,
                        viewModel.Rent.ConsumptionHeatingCostsAdvance.TransactionItem,
                        viewModel.Rent.ConsumptionHeatingCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        roomCostShareRent.ColdWaterCostsAdvanceShare,
                        viewModel.Rent.ColdWaterCostsAdvance.TransactionItem,
                        viewModel.Rent.ColdWaterCostsAdvance.TransactionShareTypes.ToString()));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        roomCostShareRent.WarmWaterCostsAdvanceShare,
                        viewModel.Rent.WarmWaterCostsAdvance.TransactionItem,
                        viewModel.Rent.WarmWaterCostsAdvance.TransactionShareTypes.ToString()));
            }

            if (_PrintViewModel.AppendContractCostsDetailsSelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));
            }


            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowHeader(true));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    roomCostShareRent.RentShare,
                    viewModel.Rent.ColdRent.TransactionItem,
                    viewModel.Rent.ColdRent.TransactionShareTypes.ToString()));

            if (!_PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        roomCostShareRent.ContractCostsAdvanceShare,
                        viewModel.Rent.Advance.TransactionItem,
                        ""));
            }
            else
            {
                if (!_PrintViewModel.AppendContractCostsDetailsSelected)
                {
                    dataRowGroup = ContractCostsRoomsDetailsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);
                }
                else
                {
                    dataRowGroup.Rows.Add(
                        _Print.OutputTableRow(
                            roomCostShareRent.ContractCostsAdvanceShare,
                            viewModel.Rent.Advance.TransactionItem,
                            ""));
                }
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDisplayUntilEndOfYear(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowHeader(true));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    roomCostShareRent.FirstYearRentShare,
                    viewModel.Rent.ColdRent.TransactionItem,
                    viewModel.Rent.ColdRent.TransactionShareTypes.ToString()));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    roomCostShareRent.FirstYearAdvanceShare,
                    viewModel.Rent.Advance.TransactionItem,
                    ""));

            return dataRowGroup;
        }


        private Block RentPlanTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = _Print.OutputTable();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            if (month > 0 && month < 13)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow($"\nFlat {viewModel.GetFlatViewModel().Area}m²\n" +
                    $"Costs\t\t\t> {month}/{_PrintViewModel.SelectedYear}\n", false, 14.0, FontWeights.UltraBlack));
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow($"\nFlat {viewModel.GetFlatViewModel().Area}m²\n" +
                    $"Costs\t\t\t> {viewModel.StartDate:d}\n", false, 14.0, FontWeights.UltraBlack));
            }

            if (_PrintViewModel.ContractCostsSelected)
            {
                dataRowGroup = ContractCosts(dataRowGroup, viewModel, month);
            }

            if (_PrintViewModel.AllCostsSelected)
            {
                dataRowGroup = AllCosts(dataRowGroup, viewModel, month);
            }

            if (_PrintViewModel.AppendContractCostsDetailsSelected)
            {
                dataRowGroup = ContractCostsDetailsDisplay(dataRowGroup, viewModel, month);
            }

            rentPlanFlatTable.RowGroups.Add(dataRowGroup);

            return rentPlanFlatTable;
        }


        private Section RentPlanTableRooms(Section documentContext, RentViewModel viewModel, int month = -1)
        {
            if (_PrintViewModel.PrintExcerptSelected || _PrintViewModel.PrintAllSelected || _PrintViewModel.PrintRoomsSelected)
            {
                Table rentPlanRoomsTable = _Print.OutputTableRooms();

                rentPlanRoomsTable.BreakPageBefore = true;

                TableRowGroup dataRowGroup = new TableRowGroup();
                dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

                if (month > 0 && month < 13)
                {
                    documentContext.Blocks.Add(_Print.BuildHeader($"Costs per month\n" +
                        $"Rooms\t\t\t> {month}/{_SelectedYear}"));
                }
                else
                {
                    documentContext.Blocks.Add(_Print.BuildHeader($"Costs per month\n" +
                        $"Rooms\t\t\t> {viewModel.StartDate:d}"));
                }

                if (viewModel.RoomCostShares != null)
                {

                    if (_PrintViewModel.DisplaySummarySelected)
                    {

                        if (_PrintViewModel.ContractCostsSelected)
                        {
                            dataRowGroup = AddDateInformationToSeparatorText(
                                dataRowGroup,
                                viewModel,
                                month,
                                "contract costs summary"
                                );


                            double sum = 0.0;

                            foreach (RoomCostShareRent item in viewModel.RoomCostShares)
                            {
                                dataRowGroup = ContractCostsRooms(dataRowGroup, viewModel, item, month);

                                sum += Math.Round(item.PriceShare, 2);
                            }

                            dataRowGroup = _Print.WriteCheckToTableRowGroup(
                                dataRowGroup,
                                sum,
                                viewModel.CostsTotal,
                                "check: contract costs",
                                false);
                        }

                        if (_PrintViewModel.AllCostsSelected)
                        {
                            dataRowGroup = AddDateInformationToSeparatorText(
                                dataRowGroup,
                                viewModel,
                                month,
                                "all costs summary"
                                );


                            double sum = 0.0;

                            foreach (RoomCostShareRent item in viewModel.RoomCostShares)
                            {
                                dataRowGroup = AllCostsRooms(dataRowGroup, viewModel, item, month);

                                sum += Math.Round(item.CompleteCostShare, 2);
                            }

                            dataRowGroup = _Print.WriteCheckToTableRowGroup(
                                dataRowGroup,
                                sum,
                                viewModel.CostsTotal,
                                "check: all costs",
                                false);
                        }
                    }
                    else
                    {
                        double contractCosts = 0.0;
                        double allCosts = 0.0;

                        foreach (RoomCostShareRent item in viewModel.RoomCostShares)
                        {
                            RoomViewModel thisRoom = new RoomViewModel(item.Room, _FlatViewModel);

                            bool printThisRoom = new Compute().PrintThisRoom(_PrintViewModel, thisRoom);

                            if (printThisRoom)
                            {
                                dataRowGroup.Rows.Add(
                                    _Print.RoomSeparatorTableRow(item, _ShowTenant, $"Costs per month\n" +
                                $"Rent Change\t\t> {viewModel.StartDate:d}"));
                            }

                            if (_PrintViewModel.ContractCostsSelected)
                            {
                                dataRowGroup = ContractCostsRooms(dataRowGroup, viewModel, item, month);

                                contractCosts += Math.Round(item.PriceShare, 2);
                            }

                            if (_PrintViewModel.AllCostsSelected)
                            {
                                dataRowGroup = AllCostsRooms(dataRowGroup, viewModel, item, month);
                                allCosts += Math.Round(item.CompleteCostShare, 2);
                            }

                            if (_PrintViewModel.AppendContractCostsDetailsSelected)
                            {
                                dataRowGroup = ContractCostsRoomsDetailsDisplay(dataRowGroup, viewModel, item, month);
                            }
                        }

                        if (_PrintViewModel.ContractCostsSelected)
                        {
                            dataRowGroup = _Print.WriteCheckToTableRowGroup(
                                dataRowGroup,
                                contractCosts,
                                viewModel.CostsTotal,
                                "check: contract costs",
                                false);
                        }

                        if (_PrintViewModel.AllCostsSelected)
                        {
                            dataRowGroup = _Print.WriteCheckToTableRowGroup(
                                dataRowGroup,
                                allCosts,
                                viewModel.CompleteCosts,
                                "check: all costs",
                                false);
                        }
                    }
                }

                rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

                documentContext.Blocks.Add(rentPlanRoomsTable);

                return documentContext;
            }

            return documentContext;
        }


        private Section WriteRentDataToFlowDocument(Section documentContext, ObservableCollection<RentViewModel> RentList, int iterator, int monthCounter)
        {
            if (_PrintViewModel.PrintAllSelected || _PrintViewModel.PrintFlatSelected)
            {
                documentContext.Blocks.Add(RentPlanTable(RentList[iterator], monthCounter));
            }

            if (_PrintViewModel.PrintExcerptSelected || _PrintViewModel.PrintRoomsSelected || _PrintViewModel.PrintAllSelected)
            {
                documentContext.Blocks.Add(RentPlanTableRooms(documentContext, RentList[iterator], monthCounter));
            }

            return documentContext;
        }

        #endregion


    }
}
// EOF