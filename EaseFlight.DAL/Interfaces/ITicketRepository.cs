using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> FindAll();
    }
}