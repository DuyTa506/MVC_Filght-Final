using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;

namespace EaseFlight.Models.EntityModels
{
    public class FlightRoadScheduleModel
    {
        #region Properties
        public int ScheduleID { get; set; }
        public int FlightRoadID { get; set; }
        public int PlaneID { get; set; }
        public Nullable<int> Order { get; set; }
        public virtual FlightRoad FlightRoad { get; set; }
        public virtual Plane Plane { get; set; }
        public virtual Schedule Schedule { get; set; }
        #endregion

        #region Functions
        public FlightRoadSchedule GetModel()
        {
            var model = new FlightRoadSchedule();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
