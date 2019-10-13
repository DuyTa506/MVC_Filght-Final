using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;

namespace EaseFlight.Models.EntityModels
{
    public class PlaneSeatClassModel
    {
        #region Properties
        public int PlaneID { get; set; }
        public int SeatClassID { get; set; }
        public Nullable<int> Chair { get; set; }
        public Nullable<double> Price { get; set; }
        public virtual Plane Plane { get; set; }
        public virtual SeatClass SeatClass { get; set; }
        #endregion

        #region Functions
        public PlaneSeatClass GetModel()
        {
            var model = new PlaneSeatClass();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
