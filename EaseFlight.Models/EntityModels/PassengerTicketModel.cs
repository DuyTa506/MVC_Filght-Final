using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;

namespace EaseFlight.Models.EntityModels
{
    public class PassengerTicketModel
    {
        #region Properties
        public int ID { get; set; }
        public Nullable<int> TicketID { get; set; }
        public string FullName { get; set; }
        public Nullable<bool> Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<int> PassengerTypeID { get; set; }
        public virtual PassengerType PassengerType { get; set; }
        public virtual Ticket Ticket { get; set; }
        #endregion

        #region Functions
        public PassengerTicket GetModel()
        {
            var model = new PassengerTicket();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}