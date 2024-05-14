using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLivingCostCalculator.Models
{
    internal class Accounting
    {
        public DateTime AccountingStartDate { get; set; }
        public DateTime AccountingEndDate { get; set; }

        public ObservableCollection<BillingPeriod> billingPeriods;

        // brücke bauen zwischen flat als immer bestehende einheit und
        // dem wechselnden accounting, irgendwo die mieter zur einheit
        // hinzufügen, etc. jetzt erstmal die grundlegende Wohnung anlegen
        // ändern, speichern und löschen können, dann weiter.

    }
}
