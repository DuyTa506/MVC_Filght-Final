using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class SeatClassRepository : BaseRepository, ISeatClassRepository
    {
        #region Constructors
        public SeatClassRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<SeatClass> FindAll()
        {
            var result = this.UnitOfWork.DBContext.SeatClasses;

            return result;
        }

        public SeatClass Find(int id)
        {
            var result = this.UnitOfWork.DBContext.SeatClasses.Find(id);

            return result;
        }
        #endregion
    }
}