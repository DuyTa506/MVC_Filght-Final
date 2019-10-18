using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class SeatClassModel
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<PlaneSeatClass> PlaneSeatClasses { get; set; }
        #endregion

        #region Functions
        public SeatClass GetModel()
        {
            var model = new SeatClass();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
