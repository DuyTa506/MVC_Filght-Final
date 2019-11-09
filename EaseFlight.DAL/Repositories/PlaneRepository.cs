using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class PlaneRepository : BaseRepository, IPlaneRepository
    {
        #region Constructors
        public PlaneRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Plane> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Planes;

            return result;
        }

        public Plane Find(int id)
        {
            var result = this.UnitOfWork.DBContext.Planes.Find(id);

            return result;
        }

        public int Insert(Plane plane)
        {
            this.UnitOfWork.DBContext.Planes.Add(plane);
            this.UnitOfWork.SaveChanges();
            return plane.ID;
        }

        public void Update(Plane plane)
        {
            var CurrentPlane = this.UnitOfWork.DBContext.Planes.Find(plane.ID);
            if (CurrentPlane != null)
            {
                CommonMethods.CopyObjectProperties(plane,CurrentPlane);
            }
        }

        public void Delete(int planeid)
        {
            var CurrentPlane = this.UnitOfWork.DBContext.Planes.Find(planeid);
            if (CurrentPlane.PlaneAirports.Count == 0 )
            {
                this.UnitOfWork.DBContext.Planes.Remove(CurrentPlane);
            }
        }
        #endregion
    }
}