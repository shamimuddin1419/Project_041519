using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmUserPackageRequest
    {
        public int UserPackageRequestId { get; set; }
        public string UserFullName { set; get; }
        public Guid UserId { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public string Reference { set; get; }
        public string Remarks { set; get; }
        public bool IsApproved { set; get; }
        public Guid ApprovedBy { set; get; }
        public DateTime ApprovedDate { set; get; }
        public string PaymentMethodTypeName { set; get; }
        public string StringCreateDate { get; set; }
        public decimal Amount { get; set; }
        public Guid RejectBy { set; get; }

    }
}
