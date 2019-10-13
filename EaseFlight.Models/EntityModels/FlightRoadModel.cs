using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class FlightRoadModel
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<FlightRoadAirport> FlightRoadAirports { get; set; }
        public virtual ICollection<FlightRoadSchedule> FlightRoadSchedules { get; set; }
        public virtual ICollection<Plane> Planes { get; set; }
        #endregion

        #region Functions
        public FlightRoad GetModel()
        {
            var model = new FlightRoad();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
