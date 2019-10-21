using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.Models.EntityModels
{
    public class CountryModel
    {
        #region Properties
        public int ID { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public virtual ICollection<Airport> Airports { get; set; }
        #endregion

        #region Functions
        public Country GetModel()
        {
            var model = new Country();
            CommonMethods.CopyObjectProperties(this, model);

            return model;
        }
        #endregion
    }
}