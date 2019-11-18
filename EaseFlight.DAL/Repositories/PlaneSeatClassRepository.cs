using EaseFlight.Common.Utilities;
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
            var CurrentPlaneSeatClass = this.UnitOfWork.DBContext.PlaneSeatClasses.Find(planeSeatClassairport.PlaneID, planeSeatClassairport.SeatClassID);

            if (CurrentPlaneSeatClass != null)
            {
                CommonMethods.CopyObjectProperties(planeSeatClassairport, CurrentPlaneSeatClass);
            }
        }

        public void Delete(int planeid,int planeseatid)
        {
            var CurrentPlaneSeatClass = this.UnitOfWork.DBContext.PlaneSeatClasses.Find(planeid, planeseatid);

            this.UnitOfWork.DBContext.PlaneSeatClasses.Remove(CurrentPlaneSeatClass);
        }
        #endregion
    }
}