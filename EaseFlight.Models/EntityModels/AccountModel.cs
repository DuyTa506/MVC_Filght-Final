using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class AccountModel
    {
        #region Properties
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IDCardOrPassport { get; set; }
        public Nullable<System.DateTime> DateIssueOrExpiry { get; set; }
        public string PlaceIssue { get; set; }
        public string Photo { get; set; }
        public string ResetPasswordToken { get; set; }
        public Nullable<System.DateTime> ExpireToken { get; set; }
        public Nullable<int> AccountTypeID { get; set; }
        public Nullable<bool> Status { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        #endregion

        #region Functions
        public Account GetModel()
        {
            var model = new Account();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }

        public bool IsThirdLogin()
        {
            return string.IsNullOrEmpty(this.Password);
        }
        #endregion
    }
}
