using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmReturnType
    {
        public string ID { set; get; }
        public string ErrCode { set; get; }
        public string UserMsg { set; get; }
        public bool Status { set; get; }
    }
}
