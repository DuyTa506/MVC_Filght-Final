using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System;

namespace EaseFlight.Models.EntityModels
{
    public class PassengerTypeModel
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Discount { get; set; }
        #endregion

        #region Functions
        public PassengerType GetModel()
        {
            var model = new PassengerType();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}
