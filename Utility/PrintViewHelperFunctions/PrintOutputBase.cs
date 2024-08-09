using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;

namespace SharedLivingCostCalculator.Utility.PrintViewHelperFunctions
{
    public class PrintOutputBase
    {
        public Table GetOutputTableForFlat => OutputTableForFlat();

        public PrintOutputBase()
        {
            
        }


        private Table OutputTableForFlat()
        {

            Table dataOutputTable = new Table();

            TableColumn DateColumn = new TableColumn() { Width = new GridLength(100) };
            TableColumn ItemColumn = new TableColumn() { Width = new GridLength(250) };
            TableColumn CostColumn = new TableColumn() { Width = new GridLength(100) };

            dataOutputTable.Columns.Add(DateColumn);
            dataOutputTable.Columns.Add(ItemColumn);
            dataOutputTable.Columns.Add(CostColumn);

            return dataOutputTable;
        }
    }
}
