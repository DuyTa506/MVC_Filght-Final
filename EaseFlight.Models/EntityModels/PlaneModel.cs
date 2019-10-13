using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class PlaneModel
    {
        #region Properties
        public int ID { get; set; }
        public string Airline { get; set; }
        public string Status { get; set; }
        public virtual ICollection<FlightRoadSchedule> FlightRoadSchedules { get; set; }
        public virtual ICollection<PlaneSeatClass> PlaneSeatClasses { get; set; }
        public virtual ICollection<FlightRoad> FlightRoads { get; set; }
        #endregion

        #region Functions
        public Plane GetModel()
        {
            var model = new Plane();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
