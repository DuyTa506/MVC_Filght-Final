using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        #region Constructors
        public AccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Account> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Accounts;

            return result;
        }

        public Account Find(int id)
        {
            var result = this.UnitOfWork.DBContext.Accounts.Find(id);

            return result;
        }

        public Account FindByUsername(string username)
        {
            var result = this.UnitOfWork.DBContext.Accounts
                .Where(account => account.Username.Equals(username)).FirstOrDefault();

            return result;
        }

        public Account FindByEmail(string email)
        {
            var result = this.UnitOfWork.DBContext.Accounts
                .Where(account => account.Email.Equals(email)).FirstOrDefault();

            return result;
        }

        public int Insert(Account account)
        {
            this.UnitOfWork.DBContext.Accounts.Add(account);

            return account.ID;
        }

        public void Update(Account account)
        {
            var currentAccount = this.UnitOfWork.DBContext.Accounts.Find(account.ID);

            if (currentAccount != null)
                CommonMethods.CopyObjectProperties(account, currentAccount);
        }

        public int Delete(int accountId)
        {
            var currentAccount = this.UnitOfWork.DBContext.Accounts.Find(accountId);
            var result = 0;

            if (currentAccount.Tickets.Count == 0)
            {
                this.UnitOfWork.DBContext.Accounts.Remove(currentAccount);
                this.UnitOfWork.SaveChanges();
                result = 1;
            }

            return result;
        }
        #endregion
    }
}
