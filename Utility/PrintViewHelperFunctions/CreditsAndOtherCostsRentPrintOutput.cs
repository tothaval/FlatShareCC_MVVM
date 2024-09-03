/*  Shared Living Costs Calculator (by Stephan Kammel, Dresden, Germany, 2024)
 *  
 *  CreditsAndOtherCostsRentPrintOutput
 * 
 *  helper class for creating credits and other costs related output in PrintViewModel
 */
using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows;
using SharedLivingCostCalculator.Interfaces.Financial;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;
using SharedLivingCostCalculator.ViewModels;
using SharedLivingCostCalculator.Enums;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class CreditsAndOtherCostsRentPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private FlatViewModel _FlatViewModel { get; set; }


        private bool IsCredit { get; set; } = false;


        private PrintOutputBase _Print { get; }


        private PrintViewModel _PrintViewModel { get; }


        private int _SelectedYear { get; set; }


        private bool _ShowTenant { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public CreditsAndOtherCostsRentPrintOutput(PrintViewModel printViewModel, FlatViewModel flatViewModel, int selectedYear, bool showTenant, bool isCredit)
        {
            _PrintViewModel = printViewModel;
            _FlatViewModel = flatViewModel;
            _SelectedYear = selectedYear;
            _ShowTenant = showTenant;
            IsCredit = isCredit;

            _Print = new PrintOutputBase(_PrintViewModel, _FlatViewModel, _SelectedYear);
        }

        #endregion


        // Methods
        #region Methods

        private Section BuildDataOutputProgressionCreditsAndOtherCosts(Section documentContext, ObservableCollection<RentViewModel> RentList, DataOutputProgressionTypes SelectedDetailOption, int iterator, bool isFlat)
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
                            documentContext = WriteFTIDataToFlowDocument(documentContext, RentList, iterator, monthCounter);
                        }
                        else if (RentList[iterator].StartDate.Year == _SelectedYear
                            && monthCounter >= RentList[iterator].StartDate.Month)
                        {
                            documentContext = WriteFTIDataToFlowDocument(documentContext, RentList, iterator, monthCounter);
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
                            documentContext = WriteFTIDataToFlowDocument(documentContext, RentList, iterator, monthCounter);
                        }
                    }
                }
            }
            else
            {
                documentContext = WriteFTIDataToFlowDocument(documentContext, RentList, iterator, -1);
            }

            return documentContext;
        }



        private Section BuildNewPlanTable(Section rentOutput, RentViewModel rentViewModel, int monthCounter, bool isCredit = false)
        {
            if (IsCredit)
            {
                rentOutput.Blocks.Add(CostsAndCreditsPlanTable(rentViewModel, rentViewModel.Credits, monthCounter));
            }
            else
            {
                rentOutput.Blocks.Add(CostsAndCreditsPlanTable(rentViewModel, rentViewModel.FinancialTransactionItemViewModels, monthCounter));
            }

            return rentOutput;
        }


        public Section BuildOtherCostDetails(DataOutputProgressionTypes SelectedDetailOption)
        {
            Section rentOutput = new Section();

            ObservableCollection<RentViewModel> RentList = new PrintOutputBase(_PrintViewModel, _FlatViewModel, _SelectedYear).FindRelevantRentViewModels();

            // adds an overview of all found items
            for (int i = 0; i < RentList.Count; i++)
            {
                Section? valueChangeHeader = _Print.BuildValueChangeHeader(rentOutput, RentList, i, true);
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
                    if (!IsCredit)
                    {
                        rentOutput.Blocks.Add(_Print.BuildHeader($"Other Costs Plan {_PrintViewModel.SelectedYear}: "));
                    }
                    else
                    {
                        rentOutput.Blocks.Add(_Print.BuildHeader($"Credit Plan {_PrintViewModel.SelectedYear}: "));
                    }
                }

                rentOutput = BuildDataOutputProgressionCreditsAndOtherCosts(rentOutput, RentList, SelectedDetailOption, i, true);
            }

            return rentOutput;
        }


        private Block CostsAndCreditsPlanTable(RentViewModel rentViewModel, ObservableCollection<IFinancialTransactionItem> FTIs, int monthCounter = -1)
        {
            Table otherPlanFlatTable = _Print.OutputTableForFlat();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            double result = 0.0;

            dataRowGroup.Rows.Add(_Print.TableRowRentHeader());

            if (!_PrintViewModel.DisplaySummarySelected)
            {
                foreach (FinancialTransactionItemRentViewModel fti in FTIs)
                {
                    if (fti.Duration == Enums.TransactionDurationTypes.Ongoing)
                    {
                        dataRowGroup.Rows.Add(_Print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                        result += fti.TransactionSum;
                    }
                    else
                    {
                        if (fti.StartDate.Year == _SelectedYear && fti.EndDate.Year > _SelectedYear)
                        {
                            dataRowGroup.Rows.Add(_Print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                            result += fti.TransactionSum;
                        }
                        else if (fti.StartDate.Year == _SelectedYear && fti.EndDate.Year == _SelectedYear)
                        {
                            if (monthCounter < fti.StartDate.Month)
                            {
                                continue;
                            }

                            if (fti.StartDate.Month == monthCounter)
                            {
                                dataRowGroup.Rows.Add(_Print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                                result += fti.TransactionSum;
                            }

                            if (monthCounter > fti.StartDate.Month && monthCounter <= fti.EndDate.Month)
                            {
                                dataRowGroup.Rows.Add(_Print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                                result += fti.TransactionSum;
                            }
                        }
                    }
                }

                dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, rentViewModel, monthCounter, result, false);
            }
            else
            {
                if (IsCredit)
                {
                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, rentViewModel, monthCounter, rentViewModel.CreditSum, true);
                }
                else
                {
                    dataRowGroup = _Print.BuildTaxDisplay(dataRowGroup, rentViewModel, monthCounter, rentViewModel.OtherFTISum, true);
                }
            }

            otherPlanFlatTable.RowGroups.Add(dataRowGroup);

            return otherPlanFlatTable;
        }


        private Block CreditsAndOtherCostsPlanTableRooms(RentViewModel rentViewModel, int monthCounter = -1)
        {
            Section output = new Section();

            if (IsCredit)
            {
                output.Blocks.Add(OtherCostsPlanTableRooms(rentViewModel, rentViewModel.Credits, monthCounter));
            }
            else
            {
                output.Blocks.Add(OtherCostsPlanTableRooms(rentViewModel, rentViewModel.FinancialTransactionItemViewModels, monthCounter));
            }

            return output;
        }


        private Block OtherCostsPlanTableRooms(RentViewModel rentViewModel, ObservableCollection<IFinancialTransactionItem> FTIs, int month = -1)
        {
            Table rentPlanRoomsTable = _Print.OutputTableRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            if (_PrintViewModel.DisplaySummarySelected)
            {
                dataRowGroup.Rows.Add(_Print.SeparatorTextTableRow("monthly summary", true));

                dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());
            }

            foreach (RoomCostShareRent item in rentViewModel.RoomCostShares)
            {
                double result = 0.0;
                double sum = 0.0;

                RoomViewModel thisRoom = new RoomViewModel(item.Room, _FlatViewModel);

                bool printThisRoom = new Compute().PrintThisRoom(_PrintViewModel, thisRoom);

                if (printThisRoom)
                {
                    if (!_PrintViewModel.DisplaySummarySelected)
                    {
                        dataRowGroup.Rows.Add(_Print.RoomSeparatorTableRow(item, _ShowTenant));

                        dataRowGroup.Rows.Add(_Print.TableRowRoomHeader());

                        foreach (FinancialTransactionItemRentViewModel fti in FTIs)
                        {
                            if (fti.TransactionShareTypes == Enums.TransactionShareTypesRent.Equal)
                            {
                                sum = item.EqualShareRatio() * fti.TransactionSum;
                            }
                            else if (fti.TransactionShareTypes == Enums.TransactionShareTypesRent.Area)
                            {
                                sum = item.RentedAreaShareRatio * fti.TransactionSum;
                            }

                            if (fti.Duration == Enums.TransactionDurationTypes.Ongoing)
                            {
                                dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                result += sum;
                            }
                            else
                            {
                                if (fti.StartDate.Year == _SelectedYear && fti.EndDate.Year > _SelectedYear)
                                {
                                    dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                    result += sum;
                                }
                                else if (fti.StartDate.Year == _SelectedYear && fti.EndDate.Year == _SelectedYear)
                                {
                                    if (month < fti.StartDate.Month)
                                    {
                                        continue;
                                    }

                                    if (fti.StartDate.Month == month)
                                    {
                                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                        result += sum;
                                    }

                                    if (month > fti.StartDate.Month && month <= fti.EndDate.Month)
                                    {
                                        dataRowGroup.Rows.Add(_Print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                        result += sum;
                                    }
                                }
                            }
                        }

                        dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, rentViewModel, month, item.RoomName, result, false);
                    }
                    else
                    {
                        if (IsCredit)
                        {
                            dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, rentViewModel, month, item.RoomName, item.CreditShare, true);

                            sum += Math.Round(item.CreditShare);
                        }
                        else
                        {
                            dataRowGroup = _Print.BuildTaxDisplayRooms(dataRowGroup, rentViewModel, month, item.RoomName, item.OtherCostsShare, true);

                            sum += Math.Round(item.OtherCostsShare);
                        }
                    }
                }
            }

            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            double otherCostsSum = 0.0;
            double creditSum = 0.0;

            foreach (RoomCostShareRent item in rentViewModel.RoomCostShares)
            {
                otherCostsSum += Math.Round(item.OtherCostsShare, 2);
                creditSum += Math.Round(item.CreditShare, 2);
            }

            if (IsCredit)
            {
                dataRowGroup = _Print.WriteCheckToTableRowGroup(
                    dataRowGroup,
                    creditSum,
                    rentViewModel.CreditSum,
                    "Check: credit sum",
                    false
                    );
            }
            else
            {
                dataRowGroup = _Print.WriteCheckToTableRowGroup(
                    dataRowGroup,
                    otherCostsSum,
                    rentViewModel.OtherFTISum,
                    "Check: other costs sum",
                    false
                    );
            }

            return rentPlanRoomsTable;
        }


        private Section WriteFTIDataToFlowDocument(Section documentContext, ObservableCollection<RentViewModel> RentList, int iterator, int monthCounter)
        {

            if (_PrintViewModel.PrintAllSelected || _PrintViewModel.PrintFlatSelected)
            {
                documentContext.Blocks.Add(BuildNewPlanTable(documentContext, RentList[iterator], monthCounter));
            }

            if (_PrintViewModel.PrintExcerptSelected || _PrintViewModel.PrintRoomsSelected || _PrintViewModel.PrintAllSelected)
            {
                documentContext.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[iterator], monthCounter));
            }


            return documentContext;
        }

        #endregion


    }
}
// EOF