using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class FlightModel
    {
        #region Properties
        public int ID { get; set; }
        public Nullable<int> PlaneID { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<System.DateTime> ArrivalDate { get; set; }
        public Nullable<double> Price { get; set; }
        public string Status { get; set; }
        public virtual Plane Plane { get; set; }
        public virtual ICollection<TicketFlight> TicketFlights { get; set; }
        #endregion

        #region Functions
        public Flight GetModel()
        {
            var model = new Flight();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}