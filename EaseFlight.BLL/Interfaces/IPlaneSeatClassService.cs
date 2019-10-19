using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPlaneSeatClassService
    {
        IEnumerable<PlaneSeatClassModel> FindAll();
    }
}