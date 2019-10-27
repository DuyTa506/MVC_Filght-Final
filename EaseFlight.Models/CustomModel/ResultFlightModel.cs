using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.Models.CustomModel
{
    public class ResultFlightModel
    {
        public List<SearchFlightModel> DepartureData { get; set; }
        public List<SearchFlightModel> ReturnData { get; set; }
        public AirportModel From { get; set; }
        public AirportModel To { get; set; }
        public SeatClassModel SeatClass { get; set; }
        public List<int> PageSize { get; set; }
    }
}