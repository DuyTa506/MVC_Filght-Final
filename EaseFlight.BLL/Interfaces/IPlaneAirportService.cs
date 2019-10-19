using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPlaneAirportService
    {
        IEnumerable<PlaneAirportModel> FindAll();
    }
}