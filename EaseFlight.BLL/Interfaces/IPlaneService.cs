using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPlaneService
    {
        IEnumerable<PlaneModel> FindAll();
    }
}