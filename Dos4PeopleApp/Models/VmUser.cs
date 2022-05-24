using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.Models
{
    public class VmUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { set; get; }
        public string CurrentPassword { set; get; }
        public string ReferrelUserName { get; set; }
        public string Status { set; get; }
        public string ImagePath { set; get; }
        public string RequestedIP { set; get; }
        public string Sponsored { set; get; }
        public string Package { set; get; }
        public string JoinDate { set; get; }
        public string Duration { set; get; }
        public string Expire { set; get; }
        public string CountryId { get; set; }
        public bool IsSendEmail { get; set; }
        public IFormFile? Image { get; set; }
    }
}
