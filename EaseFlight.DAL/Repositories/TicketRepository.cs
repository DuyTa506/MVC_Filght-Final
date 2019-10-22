﻿using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

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
        #endregion
    }
}