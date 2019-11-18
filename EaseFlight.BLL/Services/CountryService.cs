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

        public int Insert(CountryModel country)
        {
            this.CountryRepository.Insert(country.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public void Update(CountryModel country)
        {
            this.CountryRepository.Update(country.GetModel());
            var result = this.UnitOfWork.SaveChanges();
        }

        public void Delete(int countryId)
        {
            this.CountryRepository.Delete(countryId);
        }

        public CountryModel Find(int id)
        {
            var model = this.CountryRepository.Find(id);
            var result = this.CreateViewModel(model);

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