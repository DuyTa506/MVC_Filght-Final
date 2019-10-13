using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using EaseFlight.Models.EntityModels;

namespace EaseFlight.BLL.Services
{
    public class AccountTypeService : BaseService, IAccountTypeService
    {
        #region Properties
        private IAccountTypeRepository AccountTypeRepository { get; set; }
        #endregion

        #region Constructors
        public AccountTypeService(IUnitOfWork unitOfWork,
            IAccountTypeRepository accountTypeRepository) : base(unitOfWork)
        {
            this.AccountTypeRepository = accountTypeRepository;
        }
        #endregion

        #region Functions
        public AccountTypeModel FindByName(string name)
        {
            var model = this.AccountTypeRepository.FindByName(name);
            var result = this.CreateViewModel(model);

            return result;
        }
        #endregion

        #region Model Functions
        public AccountTypeModel CreateViewModel(AccountType model)
        {
            AccountTypeModel viewModel = null;

            if (model != null)
            {
                viewModel = new AccountTypeModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}
