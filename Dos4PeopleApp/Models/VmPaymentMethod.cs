using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmPaymentMethod
    {
        public int PaymentMethodId { get; set; }
        public int PaymentMethodTypeId { get; set; }
        public string PaymentMethodName { get; set; }
        public string Remarks { get; set; }

    }
}
