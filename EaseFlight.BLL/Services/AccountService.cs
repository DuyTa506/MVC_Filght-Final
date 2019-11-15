using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using EaseFlight.Models.EntityModels;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.BLL.Services
{
    public class AccountService : BaseService, IAccountService
    {
        #region Properties
        private IAccountRepository AccountRepository { get; set; }
        #endregion

        #region Constructors
        public AccountService(IUnitOfWork unitOfWork,
            IAccountRepository accountRepository) : base(unitOfWork)
        {
            this.AccountRepository = accountRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<AccountModel> FindAll()
        {
            var modelList = this.AccountRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public AccountModel Find(int id)
        {
            var model = this.AccountRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }

        public AccountModel FindByUsername(string username)
        {
            var model = this.AccountRepository.FindByUsername(username);
            var result = this.CreateViewModel(model);

            return result;
        }

        public AccountModel FindByEmail(string email)
        {
            var model = this.AccountRepository.FindByEmail(email);
            var result = this.CreateViewModel(model);

            return result;
        }

        public int Insert(AccountModel account)
        {
            this.AccountRepository.Insert(account.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public int Update(AccountModel account)
        {
            this.AccountRepository.Update(account.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public AccountModel CheckUsernameExists(string username, string email)
        {
            var userModelByUsername = this.FindByUsername(username);
            var userModelByEmail = this.FindByEmail(email);

            return userModelByUsername ?? userModelByEmail;
        }

        public int Delete(int accountId)
        {
            var result = this.AccountRepository.Delete(accountId);

            return result;
        }
        #endregion

        #region Model Functions
        public AccountModel CreateViewModel(Account model)
        {
            AccountModel viewModel = null;

            if (model != null)
            {
                viewModel = new AccountModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}
