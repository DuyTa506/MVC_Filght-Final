using EaseFlight.DAL.UnitOfWorks;

namespace EaseFlight.BLL.Services
{
    public abstract class BaseService
    {
        #region Properties
        public IUnitOfWork UnitOfWork { get; set; }
        #endregion

        #region Constructors
        public BaseService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
        }
        #endregion
    }
}
