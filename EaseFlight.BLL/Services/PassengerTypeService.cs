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
    public class PassengerTypeService : BaseService, IPassengerTypeService
    {
        #region Properties
        private IPassengerTypeRepository PassengerTypeRepository { get; set; }
        #endregion

        #region Constructors
        public PassengerTypeService(IUnitOfWork unitOfWork,
            IPassengerTypeRepository passengerTypeRepository) : base(unitOfWork)
        {
            this.PassengerTypeRepository = passengerTypeRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<PassengerTypeModel> FindAll()
        {
            var modelList = this.PassengerTypeRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public PassengerTypeModel FindByName(string name)
        {
            var model = this.PassengerTypeRepository.FindByName(name);
            var result = this.CreateViewModel(model);

            return result;
        }
        #endregion

        #region Model Functions
        public PassengerTypeModel CreateViewModel(PassengerType model)
        {
            PassengerTypeModel viewModel = null;

            if (model != null)
            {
                viewModel = new PassengerTypeModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}