using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class TicketModel
    {
        #region Properties
        public int ID { get; set; }
        public Nullable<int> AccountID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Discount { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string PaymentID { get; set; }
        public string Status { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<TicketFlight> TicketFlights { get; set; }
        #endregion

        #region Functions
        public Ticket GetModel()
        {
            var model = new Ticket();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}