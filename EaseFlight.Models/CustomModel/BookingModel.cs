using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.Models.CustomModel
{
    public class BookingModel
    {
        public List<FlightModel> DepartFlight { get; set; }
        public List<FlightModel> ReturnFlight { get; set; }
        public AirportModel Departure { get; set; }
        public AirportModel Arrival { get; set; }
        public SeatClassModel SeatClass { get; set; }
        public List<PassengerTypeModel> PassengerType { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public double Price { get; set; }
        public string PaymentID { get; set; }
    }
}