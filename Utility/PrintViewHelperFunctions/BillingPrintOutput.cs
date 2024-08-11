using SharedLivingCostCalculator.ViewModels.Financial.ViewLess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using SharedLivingCostCalculator.Models.Financial;
using SharedLivingCostCalculator.ViewModels.Contract.ViewLess;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class BillingPrintOutput
    {

        private BillingViewModel BillingViewModel { get; }


        private int SelectedYear {  get; set; }


        public BillingPrintOutput(BillingViewModel billingViewModel, int selectedYear)
        {
            BillingViewModel = billingViewModel;
            SelectedYear = selectedYear;
        }


        private Block BillingPlanTable(BillingViewModel viewModel)
        {
            PrintOutputBase print = new PrintOutputBase(viewModel.GetFlatViewModel(), SelectedYear);
            
            Table billingPlanTable = print.OutputTableForFlatBilling();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalFixedCostsPerPeriod, viewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem));
            dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalHeatingCostsPerPeriod, viewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalCostsPerPeriod, "sum", true));

            dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.OtherFTISum, "other"));
            dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, -1 * viewModel.CreditSum, "credit"));

            dataRowGroup.Rows.Add(BillingOutputTableRow(viewModel, viewModel.TotalCostsPerPeriod + viewModel.OtherFTISum + viewModel.CreditSum, "sum", true));

            billingPlanTable.RowGroups.Add(dataRowGroup);

            return billingPlanTable;
        }


        private Table BillingOutputTableForRooms()
        {
            Table dataOutputTable = new Table();

            TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn RoomNameColumn = new TableColumn() { Width = new GridLength(150) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(200) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(150) };

            dataOutputTable.Columns.Add(DateColumn);
            dataOutputTable.Columns.Add(RoomNameColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }


        private Table BillingOutputTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            Table billingTableRooms = BillingOutputTableForRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            TableRow tableRowFixedCostsShare = BillingOutputTableRowRooms(
                    BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.FixedCostsAnnualCostsShare,
                    BillingViewModel.Billing.TotalFixedCostsPerPeriod.TransactionItem);

            dataRowGroup.Rows.Add(tableRowFixedCostsShare);

            TableRow tableRowHeatingCostsShare = BillingOutputTableRowRooms(
                    BillingViewModel,
                    roomCostShareBilling.RoomName,
                    roomCostShareBilling.HeatingCostsAnnualCostsShare,
                    BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem);

            dataRowGroup.Rows.Add(tableRowHeatingCostsShare);

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.ExtraCostsAnnualShare,
                "sum\n", true));



            //paymentParagraph = new Paragraph();
            //Payment.Blocks.Add(paymentParagraph);


            foreach (FinancialTransactionItemBillingViewModel item in roomCostShareBilling.FinancialTransactionItemViewModels)
            {
                dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                    BillingViewModel,
                    roomCostShareBilling.RoomName,
                    item.TransactionSum,
                    item.TransactionItem));
            }


            billingTableRooms.RowGroups.Add(dataRowGroup);

            dataRowGroup.Rows.Add(BillingOutputTableRowRooms(
                 BillingViewModel,
                 roomCostShareBilling.RoomName,
                 roomCostShareBilling.TotalCostsAnnualCostsShare,
                 "sum", true));

            return billingTableRooms;
        }


        private TableRow BillingOutputTableRow(BillingViewModel viewModel, double payment, string item, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell StartTime = new TableCell();
            StartTime.TextAlignment = TextAlignment.Right;

            TableCell EndTime = new TableCell();
            EndTime.TextAlignment = TextAlignment.Right;

            TableCell Item = new TableCell();
            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            //StartTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}-{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });
            StartTime.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });
            EndTime.Blocks.Add(new Paragraph(new Run($"{viewModel.EndDate:d}")) { Margin = new Thickness(0, 0, 10, 0) });

            Item.Blocks.Add(new Paragraph(new Run(item)));

            if (FontWeightBold)
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }
            else
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }

            dataRow.Cells.Add(StartTime);
            dataRow.Cells.Add(EndTime);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        private TableRow BillingOutputTableRowRooms(BillingViewModel viewModel, string roomname, double payment, string item, bool FontWeightBold = false)
        {
            TableRow dataRow = new TableRow();

            TableCell Year = new TableCell();
            Year.TextAlignment = TextAlignment.Right;

            TableCell RoomName = new TableCell();
            TableCell Item = new TableCell();

            TableCell Payment = new TableCell();
            Payment.TextAlignment = TextAlignment.Right;

            Year.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate.Year}")));
            RoomName.Blocks.Add(new Paragraph(new Run(roomname)));
            Item.Blocks.Add(new Paragraph(new Run(item)));

            if (FontWeightBold)
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(0, 1, 0, 0),
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }
            else
            {
                Paragraph paymentParagraph = new Paragraph(new Run($"{payment:C2}"))
                {
                    Margin = new Thickness(0, 0, 10, 0)
                };

                Payment.Blocks.Add(paymentParagraph);
            }

            dataRow.Cells.Add(Year);
            dataRow.Cells.Add(RoomName);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Payment);

            return dataRow;
        }


        public Section BuildBillingDetails()
        {
            Section billingOutput = new Section();

            if (BillingViewModel != null)
            {
                billingOutput.Blocks.Add(BillingPlanTable(BillingViewModel));

                foreach (RoomViewModel item in BillingViewModel.FlatViewModel.Rooms)
                {
                    RoomCostShareBilling roomCostShareBilling = new RoomCostShareBilling(item.GetRoom, BillingViewModel);


                    Paragraph newSegment = new Paragraph(new Run($"{roomCostShareBilling.RoomName} {roomCostShareBilling.RoomArea}m²"))
                    { Background = new SolidColorBrush(Colors.LightGray) };


                    billingOutput.Blocks.Add(newSegment);

                    billingOutput.Blocks.Add(OutputConsumptionTableRooms(roomCostShareBilling));

                    billingOutput.Blocks.Add(BillingOutputTableRooms(roomCostShareBilling));

                }
            }

            return billingOutput;
        }


        private TableRow ConsumptionaOutputTableRowRooms(BillingViewModel viewModel, string roomname, double consumption, double totalConsumption, string item)
        {
            TableRow dataRow = new TableRow();

            TableCell Year = new TableCell();
            Year.TextAlignment = TextAlignment.Right;

            TableCell RoomName = new TableCell();
            TableCell Item = new TableCell();

            TableCell Consumption = new TableCell();
            Consumption.TextAlignment = TextAlignment.Right;

            Year.Blocks.Add(new Paragraph(new Run($"{viewModel.StartDate.Year}")));
            RoomName.Blocks.Add(new Paragraph(new Run(roomname)));
            Item.Blocks.Add(new Paragraph(new Run(item)));

            Paragraph paymentParagraph = new Paragraph(new Run($"{consumption:N2} ({consumption / totalConsumption * 100:N2}%)"))
            {
                BorderBrush = new SolidColorBrush(Colors.Black),
                Margin = new Thickness(0, 0, 10, 0)
            };

            Consumption.Blocks.Add(paymentParagraph);

            dataRow.Cells.Add(Year);
            dataRow.Cells.Add(RoomName);
            dataRow.Cells.Add(Item);
            dataRow.Cells.Add(Consumption);

            return dataRow;
        }


        private Block OutputConsumptionTableRooms(RoomCostShareBilling roomCostShareBilling)
        {
            Table billingTableRooms = BillingOutputTableForRooms();

            TableRowGroup dataRowGroup = new TableRowGroup();
            dataRowGroup.Style = Application.Current.FindResource("DataRowStyle") as Style;

            dataRowGroup.Rows.Add(ConsumptionaOutputTableRowRooms(
                BillingViewModel,
                roomCostShareBilling.RoomName,
                roomCostShareBilling.HeatingUnitsTotalConsumptionShare,
                BillingViewModel.Billing.ConsumptionItems[0].ConsumedUnits,
                BillingViewModel.Billing.TotalHeatingCostsPerPeriod.TransactionItem));

            foreach (RoomConsumptionViewModel roomConsumptionViewModel in roomCostShareBilling.ConsumptionItemViewModels)
            {
                if (roomConsumptionViewModel.RoomArea == roomCostShareBilling.RoomArea
                    && roomConsumptionViewModel.RoomName.Equals(roomCostShareBilling.RoomName))
                {
                    dataRowGroup.Rows.Add(ConsumptionaOutputTableRowRooms(
                        BillingViewModel,
                        roomCostShareBilling.RoomName,
                        roomConsumptionViewModel.TotalConsumptionValue,
                        roomConsumptionViewModel.ConsumptionItemViewModel.ConsumedUnits,
                        roomConsumptionViewModel.ConsumptionCause));
                }

            }


            billingTableRooms.RowGroups.Add(dataRowGroup);

            return billingTableRooms;
        }
    }
}
