using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class PlaneSeatClassRepository : BaseRepository, IPlaneSeatClassRepository
    {
        #region Constructors
        public PlaneSeatClassRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<PlaneSeatClass> FindAll()
        {
            var result = this.UnitOfWork.DBContext.PlaneSeatClasses;

            return result;
        }

        public PlaneSeatClass Find(int planeId, int seatId)
        {
            var result = this.UnitOfWork.DBContext.PlaneSeatClasses.Find(planeId, seatId);

            return result;
        }

        public IEnumerable<PlaneSeatClass> FindByPlane(int planeId)
        {
            var result = this.UnitOfWork.DBContext.PlaneSeatClasses.Where(planeseat => planeseat.PlaneID == planeId).OrderBy(planeseat => planeseat.Order);

            return result;
        }

        public void Insert(PlaneSeatClass planeSeatClass)
        {
            this.UnitOfWork.DBContext.PlaneSeatClasses.Add(planeSeatClass);
            this.UnitOfWork.SaveChanges();

        }

        public void Update(PlaneSeatClass planeSeatClassairport)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int planeseatid)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}