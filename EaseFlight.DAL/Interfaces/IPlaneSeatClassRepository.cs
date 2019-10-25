using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPlaneSeatClassRepository
    {
        IEnumerable<PlaneSeatClass> FindAll();
        PlaneSeatClass Find(int planeId, int seatId);
        IEnumerable<PlaneSeatClass> FindByPlane(int planeId);
    }
}