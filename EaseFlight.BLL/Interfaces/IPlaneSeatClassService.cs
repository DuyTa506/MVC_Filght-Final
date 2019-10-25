using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPlaneSeatClassService
    {
        IEnumerable<PlaneSeatClassModel> FindAll();
        PlaneSeatClassModel Find(int planeId, int seatId);
        IEnumerable<PlaneSeatClassModel> FindByPlane(int planeId);
    }
}