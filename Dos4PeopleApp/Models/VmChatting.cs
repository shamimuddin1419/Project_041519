using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmChatting
    {
        public Guid SenderID { get; set; }
        public Guid ReceiverID { get; set; }
        public string MessageBody { set; get; }
        public bool IsSeen { set; get; }
        public DateTime CreatedDate { set; get; }
        public string ReceiverName { set; get; }
        public string SenderName { set; get; }
        public Guid CreatedBy { get; set; }
        public string searchValue { set; get; }
    }
}
