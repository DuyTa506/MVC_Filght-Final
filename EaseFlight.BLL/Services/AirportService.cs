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
    public class AirportService : BaseService, IAirportService
    {
        #region Properties
        private IAirportRepository AirportRepository { get; set; }
        #endregion

        #region Constructors
        public AirportService(IUnitOfWork unitOfWork,
            IAirportRepository airportRepository) : base(unitOfWork)
        {
            this.AirportRepository = airportRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<AirportModel> FindAll()
        {
            var modelList = this.AirportRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public AirportModel Find(int id)
        {
            var model = this.AirportRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }
        #endregion

        #region Model Functions
        public AirportModel CreateViewModel(Airport model)
        {
            AirportModel viewModel = null;

            if (model != null)
            {
                viewModel = new AirportModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }

        public int Insert(AirportModel airport)
        {
            this.AirportRepository.Insert(airport.GetModel());
            var result = this.UnitOfWork.SaveChanges();
            return result;
        }

        public void Update(AirportModel airport)
        {
            this.AirportRepository.Update(airport.GetModel());
            var result = this.UnitOfWork.SaveChanges();
        }

        public void Delete(int airportId)
        {
            this.AirportRepository.Delete(airportId);
        }

       

        #endregion
    }
}