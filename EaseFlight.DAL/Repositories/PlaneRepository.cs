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
        #endregion
    }
}