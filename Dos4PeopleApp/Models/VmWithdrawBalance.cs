using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmWithdrawBalance
    {
        public decimal? AvailableTaskEarn { set; get; }
        public decimal? AvailableCommissionEarn { set; get; }
        public decimal? MaxWithdrawableCommission { set; get; }
        public decimal? MaxWithdrawableEarn { set; get; }
        public decimal? EarnWithdrawDay { set; get; }
        public decimal? CommissionWithdrawDay { set; get; }
    }
}
