using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;

namespace EaseFlight.Models.EntityModels
{
    public class TicketScheduleModel
    {
        #region Properties
        public int TicketID { get; set; }
        public int ScheduleID { get; set; }
        public Nullable<bool> RoundTrip { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual Ticket Ticket { get; set; }
        #endregion

        #region Functions
        public TicketSchedule GetModel()
        {
            var model = new TicketSchedule();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
