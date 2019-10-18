using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class SeatMapModel
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Columns { get; set; }
        public string RowWithoutSeat { get; set; }
        public Nullable<int> Capacity { get; set; }
        public virtual ICollection<Plane> Planes { get; set; }
        #endregion

        #region Functions
        public SeatMap GetModel()
        {
            var model = new SeatMap();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}