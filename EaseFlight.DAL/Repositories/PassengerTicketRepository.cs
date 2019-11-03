using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

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
        #endregion
    }
}