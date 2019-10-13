using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class AccountTypeRepository : BaseRepository, IAccountTypeRepository
    {
        #region Constructors
        public AccountTypeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public AccountType FindByName(string name)
        {
            var result = this.UnitOfWork.DBContext.AccountTypes
                .Where(role => role.Name.Equals(name)).FirstOrDefault();

            return result;
        }
        #endregion
    }
}
