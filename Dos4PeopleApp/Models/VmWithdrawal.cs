using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmWithdrawal
    {
        public int WithdrawId { get; set; }
        public string PaymentMethod { get; set; }
        public string WalletAddress { get; set; }
        public decimal WithdrawAmount { get; set; }
        public string Password { get; set; }
        public string Remark { get; set; }
        public bool isMainBalance { set; get; }
        public bool isCommission { set; get; }
        public Guid UserId { get; set; }
        public string WithdrawBalanceType { set; get; }
        public string StringCreateDate { get; set; }
        public string WithdrawStatus { set; get; }
    }
}
