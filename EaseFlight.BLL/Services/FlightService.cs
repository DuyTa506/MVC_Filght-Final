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
    public class FlightService : BaseService, IFlightService
    {
        #region Properties
        private IFlightRepository FlightRepository { get; set; }
        #endregion

        #region Constructors
        public FlightService(IUnitOfWork unitOfWork,
            IFlightRepository flightRepository) : base(unitOfWork)
        {
            this.FlightRepository = flightRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<FlightModel> FindAll()
        {
            var modelList = this.FlightRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }
        #endregion

        #region Model Functions
        public FlightModel CreateViewModel(Flight model)
        {
            FlightModel viewModel = null;

            if (model != null)
            {
                viewModel = new FlightModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}