using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VMDashboardFirstCardData
    {
        public decimal CurrentBalance { get; set; }
        public string CurrentPackageName { get; set; }
        public decimal CurrentPackageValue { get; set; }
        public decimal TodaysEarn { get; set; }
        public decimal TotalReferrelCommission { get; set; }
        public decimal TotalTaskEarn { get; set; }
        public decimal TotalWorkCommission { get; set; }
    }
}
