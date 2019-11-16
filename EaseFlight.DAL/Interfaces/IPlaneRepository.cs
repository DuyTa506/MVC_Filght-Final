using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IPlaneRepository
    {
        IEnumerable<Plane> FindAll();
        Plane Find(int id);
        int Insert(Plane plane);
        void Update(Plane plane);
        int Delete(int planeid);
    }
}