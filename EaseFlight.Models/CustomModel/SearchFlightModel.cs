using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.Models.CustomModel
{
    public class SearchFlightModel
    {
        public List<FlightModel> FlightList { get; set; }
        public List<string> AirportCode { get; set; }
        public double Price { get; set; }
    }
}