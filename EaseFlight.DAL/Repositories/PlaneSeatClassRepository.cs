using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

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
        #endregion
    }
}