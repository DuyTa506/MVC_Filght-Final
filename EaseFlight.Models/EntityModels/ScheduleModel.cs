using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class ScheduleModel
    {
        #region Properties
        public int ID { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public string Status { get; set; }
        public virtual ICollection<FlightRoadSchedule> FlightRoadSchedules { get; set; }
        public virtual ICollection<TicketSchedule> TicketSchedules { get; set; }
        #endregion

        #region Functions
        public Schedule GetModel()
        {
            var model = new Schedule();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
