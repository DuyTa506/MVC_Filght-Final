using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;

namespace EaseFlight.Models.EntityModels
{
    public class FlightRoadAirportModel
    {
        #region Properties
        public int FlightRoadID { get; set; }
        public int AirportID { get; set; }
        public Nullable<bool> PlaceTakeoff { get; set; }
        public virtual Airport Airport { get; set; }
        public virtual FlightRoad FlightRoad { get; set; }
        #endregion

        #region Functions
        public FlightRoadAirport GetModel()
        {
            var model = new FlightRoadAirport();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
