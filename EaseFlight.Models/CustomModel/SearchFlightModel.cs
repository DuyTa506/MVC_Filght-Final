using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.Models.CustomModel
{
    public class SearchFlightModel
    {
        public IEnumerable<FlightModel> FlightList { get; set; }
    }
}