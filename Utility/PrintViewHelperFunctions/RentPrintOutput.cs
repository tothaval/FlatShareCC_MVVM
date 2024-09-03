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

        private TableRowGroup AllCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs"));

                dataRowGroup = ContractCostsDisplay(dataRowGroup, viewModel, month);

                if (!_PrintViewModel.DetailedNonContractItemsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.OtherFTISum, "other", month));
                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit", month));
                }
                else
                {
                    foreach (FinancialTransactionItemRentViewModel item in viewModel.FinancialTransactionItemViewModels)
                    {
                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, item.TransactionSum, item.TransactionItem, month));
                    }

                    foreach (FinancialTransactionItemRentViewModel item in viewModel.Credits)
                    {
                        dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, -1 * item.TransactionSum, item.TransactionItem, month));
                    }
                }

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsAndCredits, false);

                if (_PrintViewModel.AnnualCostsSelected)
                {
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year"));

                    dataRowGroup = ContractCostsDisplayUntilEndOfYear(dataRowGroup, viewModel, month);

                    dataRowGroup.Rows.Add(_Print.OutputTableRow(viewModel, viewModel.FirstYearOtherFTISum, "other"));

                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, -1, viewModel.FirstYearCompleteCosts, false);
                }
            }
            else
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs summary"));

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
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs", true));

                    dataRowGroup = ContractCostsRoomsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);

                    if (!_PrintViewModel.DetailedNonContractItemsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.OtherCostsShare, "other", month));

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, -1 * roomCostShareRent.CreditShare, "credit", month));
                    }
                    else
                    {
                        foreach (FinancialTransactionItemRentViewModel item in roomCostShareRent.FinancialTransactionItemViewModels)
                        {
                            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, item.TransactionSum, item.TransactionItem, month));

                        }
                        foreach (FinancialTransactionItemRentViewModel item in roomCostShareRent.Credits)
                        {
                            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, -1 * item.TransactionSum, item.TransactionItem, month));
                        }
                    }

                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.CostAndCreditShare, false);

                    if (_PrintViewModel.AnnualCostsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs until end of year", true));

                        dataRowGroup = ContractCostsRoomsDisplayUntilEndOfYear(dataRowGroup, viewModel, roomCostShareRent, month);

                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearOtherCostsShare, "other", month));

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

            ObservableCollection<RentViewModel> RentList = new PrintOutputBase(_PrintViewModel, _FlatViewModel, _SelectedYear).FindRelevantRentViewModels();

            // adds an overview of all found items
            for (int i = 0; i < RentList.Count; i++)
            {
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(rentOutput, RentList, i, true);

                if (valueChangeHeader == null)
                {
                    continue;
                }

                rentOutput = valueChangeHeader;
            }

            // displays the contents of each item according to options set in print menu
            for (int i = 0; i < RentList.Count; i++)
            {
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(rentOutput, RentList, i, false);

                if (valueChangeHeader == null)
                {
                    continue;
                }

                rentOutput = valueChangeHeader;

                if (_PrintViewModel.PrintAllSelected || _PrintViewModel.PrintFlatSelected)
                {
                    rentOutput.Blocks.Add(_Print.BuildHeader($"Rent Plan Flat {_PrintViewModel.SelectedYear}: "));
                }


                rentOutput = BuildDataOutputProgression(rentOutput, RentList, SelectedDetailOption, i, true);
            }


            return rentOutput;
        }


        private TableRowGroup ContractCosts(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (!_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs"));

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
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs summary"));

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, viewModel, month, viewModel.CostsTotal, true);
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsDetailsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, int month)
        {
            if (_PrintViewModel.AppendContractCostsDetailsSelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("monthly contract costs advance details"));
            }

            if (_PrintViewModel.AppendContractCostsDetailsSelected || _PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                                        viewModel,
                                        viewModel.ProRataCostsAdvance,
                                        viewModel.Rent.ProRataCostsAdvance.TransactionItem,
                                        month));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel,
                        viewModel.BasicHeatingCostsAdvance,
                        viewModel.Rent.BasicHeatingCostsAdvance.TransactionItem,
                        month));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel,
                        viewModel.ConsumptionHeatingCostsAdvance,
                        viewModel.Rent.ConsumptionHeatingCostsAdvance.TransactionItem,
                        month));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel,
                        viewModel.ColdWaterCostsAdvance,
                        viewModel.Rent.ColdWaterCostsAdvance.TransactionItem,
                        month));

                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel,
                        viewModel.WarmWaterCostsAdvance,
                        viewModel.Rent.WarmWaterCostsAdvance.TransactionItem,
                        month));
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
            dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.ColdRent,
                    viewModel.Rent.ColdRent.TransactionItem,
                    month));

            if (!_PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(
                    _Print.OutputTableRow(
                        viewModel,
                        viewModel.Advance,
                        viewModel.Rent.Advance.TransactionItem,
                        month));
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
                            viewModel,
                            viewModel.Advance,
                            viewModel.Rent.Advance.TransactionItem,
                            month));
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
            dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.FirstYearRent,
                    viewModel.Rent.ColdRent.TransactionItem,
                    month));

            dataRowGroup.Rows.Add(
                _Print.OutputTableRow(
                    viewModel,
                    viewModel.FirstYearAdvance,
                    viewModel.Rent.Advance.TransactionItem,
                    month));

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
                    dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs", true));

                    dataRowGroup = ContractCostsRoomsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);

                    dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, viewModel, month, roomCostShareRent.RoomName, roomCostShareRent.PriceShare, false);

                    if (_PrintViewModel.AnnualCostsSelected)
                    {
                        dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs until end of year", true));

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
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("monthly contract costs advance details", true));
            }

            if (_PrintViewModel.AppendContractCostsDetailsSelected || _PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.ProRataAdvanceShare, viewModel.Rent.ProRataCostsAdvance.TransactionItem, month));
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.BasicHeatingCostsAdvanceShare, viewModel.Rent.BasicHeatingCostsAdvance.TransactionItem, month));
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.ConsumptionHeatingCostsAdvanceShare, viewModel.Rent.ConsumptionHeatingCostsAdvance.TransactionItem, month));
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.ColdWaterCostsAdvanceShare, viewModel.Rent.ColdWaterCostsAdvance.TransactionItem, month));
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.WarmWaterCostsAdvanceShare, viewModel.Rent.WarmWaterCostsAdvance.TransactionItem, month));
            }

            dataRowGroup.Rows.Add(_Print.SeparatorLineTableRow(true));

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDisplay(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.RentShare, viewModel.Rent.ColdRent.TransactionItem, month));

            if (!_PrintViewModel.DetailedContractCostsSelected)
            {
                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.ContractCostsAdvanceShare, viewModel.Rent.Advance.TransactionItem, month));
            }
            else
            {
                if (!_PrintViewModel.AppendContractCostsDetailsSelected)
                {
                    dataRowGroup = ContractCostsRoomsDetailsDisplay(dataRowGroup, viewModel, roomCostShareRent, month);
                }
                else
                {
                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.ContractCostsAdvanceShare, viewModel.Rent.Advance.TransactionItem, month));
                }
            }

            return dataRowGroup;
        }


        private TableRowGroup ContractCostsRoomsDisplayUntilEndOfYear(TableRowGroup dataRowGroup, RentViewModel viewModel, RoomCostShareRent roomCostShareRent, int month)
        {
            dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearRentShare, viewModel.Rent.ColdRent.TransactionItem, month));

            dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(viewModel, roomCostShareRent.RoomName, roomCostShareRent.FirstYearAdvanceShare, viewModel.Rent.Advance.TransactionItem, month));

            return dataRowGroup;
        }


        private Block RentPlanTable(RentViewModel viewModel, int month = -1)
        {
            Table rentPlanFlatTable = _Print.OutputTableForFlat();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

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

                TableRowGroup dataRowGroup = new TableRowGroup();
                dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

                documentContext.Blocks.Add(_Print.BuildHeader($"Rent Plan Rooms {_SelectedYear}: "));

                if (viewModel.RoomCostShares != null)
                {

                    if (_PrintViewModel.DisplaySummarySelected)
                    {
                        if (_PrintViewModel.ContractCostsSelected)
                        {
                            dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("contract costs summary", true));

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
                                "check: contract costs");
                        }

                        if (_PrintViewModel.AllCostsSelected)
                        {
                            dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("all costs summary", true));

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
                                "check: all costs");
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
                                dataRowGroup.Rows.Add(_Print.RoomSeparatorTableRow(item, _ShowTenant));
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
                                "check: contract costs");
                        }

                        if (_PrintViewModel.AllCostsSelected)
                        {
                            dataRowGroup = _Print.WriteCheckToTableRowGroup(
                                dataRowGroup,
                                allCosts,
                                viewModel.CompleteCosts,
                                "check: all costs");
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