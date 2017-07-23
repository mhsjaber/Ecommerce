using ECommerce.Core;
using ECommerce.Core.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class MemberViewModel
    {
        public Guid MemberID { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public CustomerStatus Status { get; set; }
    }

    public class MemberModel
    {
        private ECommerceUnitOfWork _unit;
        public MemberModel()
        {
            _unit = new ECommerceUnitOfWork(new ECommerceContext());
        }

        public List<MemberViewModel> Get()
        {
            try
            {
                var members = _unit.CustomerRepository.GetAll()
                    .OrderByDescending(x => x.CreatedOn).ToList();
                var model = new List<MemberViewModel>();
                foreach (var member in members)
                {
                    model.Add(new MemberViewModel()
                    {
                        MemberID = member.ID,
                        FullName = member.FullName,
                        Mobile = member.Mobile,
                        Username = member.Username,
                        CreatedOn = member.CreatedOn,
                        Status = member.Status,
                        Email = member.Email
                    });
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}