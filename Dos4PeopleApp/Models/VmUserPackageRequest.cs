using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmUserPackageRequest
    {
        public int UserPackageRequestId { get; set; }
        public Guid UserId { get; set; }
        public int PackageId { get; set; }
        public int PaymentMethodId { get; set; }
        public string Reference { set; get; }
        public string Remarks { set; get; }

    }
}
