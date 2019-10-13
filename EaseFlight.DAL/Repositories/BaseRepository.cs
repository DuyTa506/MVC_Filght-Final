using EaseFlight.DAL.UnitOfWorks;

namespace EaseFlight.DAL.Repositories
{
    public abstract class BaseRepository
    {
        #region Properties
        public IUnitOfWork UnitOfWork { get; set; }
        #endregion

        #region Constructors
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        #endregion
    }
}
