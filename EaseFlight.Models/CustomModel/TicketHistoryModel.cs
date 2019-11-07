using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.Models.CustomModel
{
    public class TicketHistoryModel
    {
        public TicketModel Ticket { get; set; }
        public AirportModel From { get; set; }
        public AirportModel To { get; set; }
        public SeatClassModel SeatClass { get; set; }
        public List<FlightModel> DepartFlight { get; set; }
        public List<FlightModel> ReturnFlight { get; set; }
        public List<PassengerTicketModel> Passengers { get; set; }
        public List<TicketFlightModel> TicketFlightList { get; set; }
    }
}
