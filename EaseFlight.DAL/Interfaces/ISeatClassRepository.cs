using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface ISeatClassRepository
    {
        IEnumerable<SeatClass> FindAll();
        SeatClass Find(int id);
    }
}
