using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VMIncomeHistory
    {
        public int TransactionMasterId { get; set; }
        public string TransactionDate { get; set; }
        public decimal TransactionAmt { get; set; }
        public string Remarks { get; set; }
        public string JoiningDate { get; set; }
        public int CurrentDuration { get; set; }
        //public string PackageValidityDate { get; set; }
        public string ExpiryDate { get; set; }
    }
}
