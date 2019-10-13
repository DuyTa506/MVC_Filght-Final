using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class AirportModel
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public virtual ICollection<FlightRoadAirport> FlightRoadAirports { get; set; }
        #endregion

        #region Functions
        public Airport GetModel()
        {
            var model = new Airport();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
