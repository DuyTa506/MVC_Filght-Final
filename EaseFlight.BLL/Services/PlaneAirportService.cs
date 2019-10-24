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
    public class PlaneAirportService : BaseService, IPlaneAirportService
    {
        #region Properties
        private IPlaneAirportRepository PlaneAirportRepository { get; set; }
        #endregion

        #region Constructors
        public PlaneAirportService(IUnitOfWork unitOfWork,
            IPlaneAirportRepository planeAirportRepository) : base(unitOfWork)
        {
            this.PlaneAirportRepository = planeAirportRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<PlaneAirportModel> FindAll()
        {
            var modelList = this.PlaneAirportRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Insert(PlaneAirportModel planeAirportModel)
        {
            this.PlaneAirportRepository.Insert(planeAirportModel.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public PlaneAirportModel Find(int planeId, int airportId)
        {
            var model = this.PlaneAirportRepository.Find(planeId, airportId);
            var result = this.CreateViewModel(model);

            return result;
        }

        public int Update(PlaneAirportModel planeAirportModel)
        {
            this.PlaneAirportRepository.Update(planeAirportModel.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public IEnumerable<PlaneAirportModel> FindByPlane(int planeId)
        {
            var modelList = this.PlaneAirportRepository.FindByPlane(planeId);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public void UpdateDepartureOrArrival(int planeId, int airportId, int flightId, bool departure)
        {
            var result = new List<string>();
            var planeAirport = this.Find(planeId, airportId);
            var departureOrArrival = planeAirport.DepartureOrArrival;

            if (departureOrArrival != null)
                result = departureOrArrival.Split('-').ToList();
            if (departure)
                result.Add(flightId + ".d");
            else result.Add(flightId + ".a");

            planeAirport.DepartureOrArrival = string.Join("-", result);

            this.Update(planeAirport);
        }
        #endregion

        #region Model Functions
        public PlaneAirportModel CreateViewModel(PlaneAirport model)
        {
            PlaneAirportModel viewModel = null;

            if (model != null)
            {
                viewModel = new PlaneAirportModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}