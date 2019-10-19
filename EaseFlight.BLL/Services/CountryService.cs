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
    public class CountryService : BaseService, ICountryService
    {
        #region Properties
        private ICountryRepository CountryRepository { get; set; }
        #endregion

        #region Constructors
        public CountryService(IUnitOfWork unitOfWork,
            ICountryRepository countryRepository) : base(unitOfWork)
        {
            this.CountryRepository = countryRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<CountryModel> FindAll()
        {
            var modelList = this.CountryRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }
        #endregion

        #region Model Functions
        public CountryModel CreateViewModel(Country model)
        {
            CountryModel viewModel = null;

            if (model != null)
            {
                viewModel = new CountryModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}