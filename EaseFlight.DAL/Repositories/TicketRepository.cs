using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class TicketRepository : BaseRepository, ITicketRepository
    {
        #region Constructors
        public TicketRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Ticket> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Tickets;

            return result;
        }

        public int Insert(Ticket ticket)
        {
            this.UnitOfWork.DBContext.Tickets.Add(ticket);
            this.UnitOfWork.SaveChanges();

            return ticket.ID;
        }

        public Ticket Find(int id)
        {
            var result = this.UnitOfWork.DBContext.Tickets.Find(id);

            return result;
        }

        public IEnumerable<Ticket> FindByAccount(int accountId)
        {
            var result = this.UnitOfWork.DBContext.Tickets.Where(ticket => ticket.AccountID == accountId);

            return result;
        }

        public void Update(Ticket ticket)
        {
            var currentTicket = this.UnitOfWork.DBContext.Tickets.Find(ticket.ID);

            ticket.PassengerTickets = currentTicket.PassengerTickets;
            ticket.TicketFlights = currentTicket.TicketFlights;

            if (currentTicket != null)
                CommonMethods.CopyObjectProperties(ticket, currentTicket);
        }
        #endregion
    }
}