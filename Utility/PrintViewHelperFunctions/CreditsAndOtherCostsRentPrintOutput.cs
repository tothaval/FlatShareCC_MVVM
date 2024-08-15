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
using System.Windows.Media;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class CreditsAndOtherCostsRentPrintOutput
    {

        // Properties & Fields
        #region Properties & Fields

        private FlatViewModel _FlatViewModel { get; set; }


        private int _SelectedYear { get; set; }


        private bool _ShowTenant { get; set; }

        #endregion


        // Constructors
        #region Constructors

        public CreditsAndOtherCostsRentPrintOutput(FlatViewModel flatViewModel, int selectedYear, bool showTenant)
        {
            _FlatViewModel = flatViewModel;
            _SelectedYear = selectedYear;
            _ShowTenant = showTenant;
        }

        #endregion


        // Methods
        #region Methods

        private Section BuildNewPlanTable(Section rentOutput, RentViewModel rentViewModel, int monthCounter, bool isCredit = false)
        {
            if (isCredit)
            {
                rentOutput.Blocks.Add(CostsAndCreditsPlanTable(rentViewModel, rentViewModel.Credits, monthCounter));
            }
            else
            {
                rentOutput.Blocks.Add(CostsAndCreditsPlanTable(rentViewModel, rentViewModel.FinancialTransactionItemViewModels, monthCounter));
            }

            return rentOutput;
        }


        public Section BuildOtherCostDetails(bool isCredit, string SelectedDetailOption)
        {
            Section rentOutput = new Section();

            ObservableCollection<RentViewModel> RentList = new PrintOutputBase(_FlatViewModel, _SelectedYear).FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {

                if (isCredit)
                {
                    if (!RentList[i].HasCredits)
                    {
                        continue;
                    }
                }
                else
                {
                    if (!RentList[i].HasOtherCosts)
                    {
                        continue;
                    }
                }


                if (RentList[i].StartDate.Year < _SelectedYear)
                {
                    if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < _SelectedYear)
                    {
                        continue;
                    }
                    else
                    {
                        rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                    }
                    else
                    {
                        rentOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 40, 0, 0), FontWeight = FontWeights.Bold });
                    }
                }

                PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

                Table headerTable = print.OutputTableForFlat();

                if (SelectedDetailOption.Equals("TimeScale"))
                {
                    for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                    {
                        if (isCredit)
                        {
                            if (!RentList[i].HasCredits)
                            {
                                rentOutput.Blocks.Add(new Paragraph(new Run($"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}")));
                                continue;
                            }
                        }
                        else
                        {
                            if (!RentList[i].HasOtherCosts)
                            {
                                rentOutput.Blocks.Add(new Paragraph(new Run($"end:\t\t\t{RentList[i].StartDate - TimeSpan.FromDays(1):d}")));
                                continue;
                            }
                        }


                        if (RentList[i].StartDate.Year == _SelectedYear && monthCounter < RentList[i].StartDate.Month)
                        {
                            continue;
                        }

                        if (i + 1 < RentList.Count)
                        {
                            //später im Ablauf, um Nachfolger zu berücksichtigen

                            if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                            {
                                break;
                            }

                            if (RentList[i].StartDate.Year < _SelectedYear)
                            {
                                rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], monthCounter, isCredit));
                            }
                            else if (RentList[i].StartDate.Year == _SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], monthCounter, isCredit));
                            }
                        }
                        else
                        {
                            if (RentList[i].StartDate.Year == _SelectedYear && monthCounter < RentList[i].StartDate.Month)
                            {
                                continue;
                            }
                            else
                            {
                                rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], monthCounter, isCredit));
                            }

                        }

                    }
                }
                else
                {
                    rentOutput.Blocks.Add(BuildNewPlanTable(rentOutput, RentList[i], -1, isCredit));
                }


                if (i + 1 < RentList.Count && RentList[i + 1].HasOtherCosts == false
                    || i + 1 < RentList.Count && RentList[i + 1].HasCredits == false)
                {
                    rentOutput.Blocks.Add(new Paragraph(new Run($"rent end:\t\t{RentList[i + 1].StartDate - TimeSpan.FromDays(1):d}")));

                }
            }

            rentOutput.Blocks.Add(BuildRoomDetailsOther(isCredit, SelectedDetailOption));

            return rentOutput;
        }


        private Section BuildRoomDetailsOther(bool isCredit, string SelectedDetailOption)
        {
            PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

            Section roomsOutput = new Section();

            Paragraph p = new Paragraph() { Background = new SolidColorBrush(Colors.LightGray) };

            roomsOutput.Blocks.Add(p);

            if (isCredit)
            {
                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Credit Plan Rooms {_SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            }
            else
            {
                p = new Paragraph() { Margin = new Thickness(0, 20, 0, 20) };
                p.Inlines.Add(new Run($"Other Costs Plan Rooms {_SelectedYear}: ")
                { FontWeight = FontWeights.Bold, FontSize = 16.0 });
            }

            p.Inlines.Add(new Run($"{print.BuildAddressDetails()}") { FontWeight = FontWeights.Normal, FontSize = 14.0 });
            roomsOutput.Blocks.Add(p);

            ObservableCollection<RentViewModel> RentList = print.FindRelevantRentViewModels();

            for (int i = 0; i < RentList.Count; i++)
            {
                if (RentList[i].StartDate.Year < _SelectedYear)
                {
                    if (RentList[i + 1].StartDate > RentList[i].StartDate && RentList[i + 1].StartDate.Year < _SelectedYear)
                    {
                        continue;
                    }
                    else
                    {
                        roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 0, 0, 0), FontWeight = FontWeights.Bold });
                    }
                    else
                    {
                        roomsOutput.Blocks.Add(new Paragraph(new Run($"rent change:\t\t{RentList[i].StartDate:d}")) { Margin = new Thickness(0, 40, 0, 0), FontWeight = FontWeights.Bold });
                    }
                }

                if (SelectedDetailOption.Equals("TimeScale"))
                {
                    for (int monthCounter = 1; monthCounter < 13; monthCounter++)
                    {

                        if (i + 1 < RentList.Count)
                        {
                            // später im Ablauf, um Nachfolger zu berücksichtigen

                            if (RentList[i + 1].StartDate.Month - 1 < monthCounter)
                            {
                                break;
                            }

                            if (RentList[i].StartDate.Year < _SelectedYear)
                            {
                                roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit, monthCounter));
                            }
                            else if (RentList[i].StartDate.Year == _SelectedYear
                                && monthCounter >= RentList[i].StartDate.Month)
                            {
                                roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit, monthCounter));
                            }


                        }
                        else
                        {
                            if (RentList[i].StartDate.Year == _SelectedYear && monthCounter < RentList[i].StartDate.Month)
                            {
                                continue;
                            }
                            else
                            {
                                roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit, monthCounter));
                            }

                        }

                    }
                }
                else
                {
                    roomsOutput.Blocks.Add(CreditsAndOtherCostsPlanTableRooms(RentList[i], isCredit));
                }

            }

            return roomsOutput;
        }


        private Block CostsAndCreditsPlanTable(RentViewModel rentViewModel, ObservableCollection<IFinancialTransactionItem> FTIs, int monthCounter = -1)
        {
            PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

            Table otherPlanFlatTable = print.OutputTableForFlat();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            double result = 0.0;

            dataRowGroup.Rows.Add(print.TableRowRentHeader());

            foreach (FinancialTransactionItemRentViewModel fti in FTIs)
            {
                if (fti.Duration == Enums.TransactionDurationTypes.Ongoing)
                {
                    dataRowGroup.Rows.Add(print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                    result += fti.TransactionSum;
                }
                else
                {
                    if (fti.StartDate.Year == _SelectedYear && fti.EndDate.Year > _SelectedYear)
                    {
                        dataRowGroup.Rows.Add(print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

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
                            dataRowGroup.Rows.Add(print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                            result += fti.TransactionSum;
                        }

                        if (monthCounter > fti.StartDate.Month && monthCounter <= fti.EndDate.Month)
                        {
                            dataRowGroup.Rows.Add(print.OutputTableRow(rentViewModel, fti.TransactionSum, fti.TransactionItem, monthCounter));

                            result += fti.TransactionSum;
                        }
                    }
                }
            }

            dataRowGroup.Rows.Add(print.OutputTableRow(rentViewModel, result, "sum", monthCounter, true));

            otherPlanFlatTable.RowGroups.Add(dataRowGroup);

            return otherPlanFlatTable;
        }


        private Block CreditsAndOtherCostsPlanTableRooms(RentViewModel rentViewModel, bool isCredit, int monthCounter = -1)
        {
            Section output = new Section();

            if (isCredit)
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
            PrintOutputBase print = new PrintOutputBase(_FlatViewModel, _SelectedYear);

            Table rentPlanRoomsTable = print.OutputTableRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            foreach (RoomCostShareRent item in rentViewModel.RoomCostShares)
            {
                double result = 0.0;

                dataRowGroup.Rows.Add(print.RoomSeparatorTableRow(item, _ShowTenant));

                dataRowGroup.Rows.Add(print.TableRowRoomHeader());


                foreach (FinancialTransactionItemRentViewModel fti in FTIs)
                {
                    double sum = 0.0;

                    if (fti.TransactionShareTypes == Enums.TransactionShareTypesRent.Equal)
                    {
                        sum = item.EqualShareRatio() * fti.TransactionSum;
                    }
                    else if (fti.TransactionShareTypes == Enums.TransactionShareTypesRent.Area)
                    {
                        sum = item.RentedAreaShareRatio() * fti.TransactionSum;
                    }

                    if (fti.Duration == Enums.TransactionDurationTypes.Ongoing)
                    {
                        dataRowGroup.Rows.Add(print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                        result += sum;
                    }
                    else
                    {
                        if (fti.StartDate.Year == _SelectedYear && fti.EndDate.Year > _SelectedYear)
                        {
                            dataRowGroup.Rows.Add(print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

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
                                dataRowGroup.Rows.Add(print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                result += sum;
                            }

                            if (month > fti.StartDate.Month && month <= fti.EndDate.Month)
                            {
                                dataRowGroup.Rows.Add(print.OutputTableRowRooms(rentViewModel, item.RoomName, sum, fti.TransactionItem, month));

                                result += sum;
                            }
                        }
                    }
                }

                dataRowGroup.Rows.Add(print.OutputTableRowRooms(rentViewModel, item.RoomName, result, "sum", month, true));

            }


            rentPlanRoomsTable.RowGroups.Add(dataRowGroup);

            return rentPlanRoomsTable;
        }

        #endregion


    }
}
// EOF