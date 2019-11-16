using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPlaneSeatClassRepository
    {
        IEnumerable<PlaneSeatClass> FindAll();
        PlaneSeatClass Find(int planeId, int seatId);
        IEnumerable<PlaneSeatClass> FindByPlane(int planeId);
        void Insert(PlaneSeatClass planeSeatClass);
        void Update(PlaneSeatClass planeSeatClassairport);
        void Delete(int planeid, int planeseatid);
    }
}