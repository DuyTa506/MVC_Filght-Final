using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IPlaneService
    {
        IEnumerable<PlaneModel> FindAll();
        PlaneModel Find(int id);
        int Insert(PlaneModel plane);
        void Update(PlaneModel plane);
        int Delete(int planeid);
    }
}