using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.Models.CustomModel
{
    public class AirportRegionModel
    {
        public string Region { get; set; }
        public IEnumerable<AirportModel> Airports {get; set;}
    }
}