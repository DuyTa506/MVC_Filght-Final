using System;

namespace EaseFlight.Models.CustomModel
{
    public class BookingSuccessModel
    {
        public string PaymentId { get; set; }
        public DateTime DepartDate { get; set; }
        public string Customer { get; set; }
        public string Flight { get; set; }
        public string Passenger { get; set; }
        public string SeatCode { get; set; }
        public double Price { get; set; }
        public int TicketId { get; set; }
    }
}