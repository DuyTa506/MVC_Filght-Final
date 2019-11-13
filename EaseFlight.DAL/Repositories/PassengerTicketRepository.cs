using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class PassengerTicketRepository : BaseRepository, IPassengerTicketRepository
    {
        #region Constructors
        public PassengerTicketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<PassengerTicket> FindAll()
        {
            var result = this.UnitOfWork.DBContext.PassengerTickets;

            return result;
        }

        public int Insert(PassengerTicket passenger)
        {
            this.UnitOfWork.DBContext.PassengerTickets.Add(passenger);

            return passenger.ID;
        }

        public IEnumerable<PassengerTicket> FindByTicket(int ticketId)
        {
            var result = this.UnitOfWork.DBContext.PassengerTickets.Where(passengerTicket => passengerTicket.TicketID == ticketId);

            return result;
        }

        public PassengerTicket Find(int passengerTicketId)
        {
            var result = this.UnitOfWork.DBContext.PassengerTickets.Find(passengerTicketId);

            return result;
        }

        public void Update(PassengerTicket passengerTicket)
        {
            var currentPassenger = this.UnitOfWork.DBContext.PassengerTickets.Find(passengerTicket.ID);

            if (currentPassenger != null)
            {
                CommonMethods.CopyObjectProperties(passengerTicket, currentPassenger);
            }
        }
        #endregion
    }
}