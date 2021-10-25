using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmPackage
    {
        public int PackageId { set; get; }
        public int PackageCategoryId { set; get; }
        public string  PackageName { set; get; }
        public decimal PackageValue { set; get; }
        public decimal DailyTaskCount { set; get; }
        public int PackageDurationDays { set; get; }
        public decimal PerClickValue { set; get; }
        public decimal DailyValue { set; get; }
        public decimal WeeklyValue { set; get; }
        public decimal MonthlyValue { set; get; }
        public decimal YearlyValue { set; get; }
        public decimal ReferralEarn { set; get; }
        public decimal WorkCommission { set; get; }
        public decimal PotentialReferralEarn { set; get; }
        public decimal TargetPotentialYearlyIncome { set; get; }
        public decimal PotentialYearlyIncome { set; get; }
        public bool IsActive { set; get; }
        public bool IsPublished { set; get; }
        public decimal TCBOnMainInvestPer { set; get; }
        public string Remarks { set; get; }
        public Guid CreatedBy { set; get; }
        public DateTime CreatedDate { set; get; }
        public string PackageCategory { set; get; }
       
    }
}
