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

        internal MemberViewModel GetDetails(Guid id)
        {
            var member = _unit.CustomerRepository.GetAll()
                    .Where(x => x.ID == id).FirstOrDefault();
            var model = new MemberViewModel()
            {
                MemberID = member.ID,
                FullName = member.FullName,
                Mobile = member.Mobile,
                Username = member.Username,
                CreatedOn = member.CreatedOn,
                Status = member.Status,
                Email = member.Email,
                Password = member.Password,
                Address = member.Address
            };
            return model;
        }

        internal void Delete(Guid id)
        {
            var customer = _unit.CustomerRepository.GetById(id);
            _unit.CustomerRepository.Delete(customer);
            _unit.Save();
        }

        internal void Update(MemberViewModel model)
        {
            var member = _unit.CustomerRepository.GetAll()
                    .Where(x => x.ID == model.MemberID).FirstOrDefault();
            member.Address = model.Address;
            member.FullName = model.FullName;
            member.Mobile = model.Mobile;
            member.Status = model.Status;
            member.Username = model.Username;
            _unit.CustomerRepository.Update(member);
            _unit.Save();
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