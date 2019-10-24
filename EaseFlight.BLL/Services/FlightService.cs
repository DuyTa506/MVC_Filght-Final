using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using EaseFlight.Models.CustomModel;
using EaseFlight.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.BLL.Services
{
    public class FlightService : BaseService, IFlightService
    {
        #region Properties
        private IFlightRepository FlightRepository { get; set; }
        private IPlaneAirportService PlaneAirportService { get; set; }
        #endregion

        #region Constructors
        public FlightService(IUnitOfWork unitOfWork,
            IFlightRepository flightRepository, IPlaneAirportService planeAirportService) : base(unitOfWork)
        {
            this.FlightRepository = flightRepository;
            this.PlaneAirportService = planeAirportService;
        }
        #endregion

        #region Functions
        public IEnumerable<FlightModel> FindAll()
        {
            var modelList = this.FlightRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Insert(FlightModel flight)
        {
            this.FlightRepository.Insert(flight.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public FlightModel Find(int id)
        {
            var model = this.FlightRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }

        public IEnumerable<FlightModel> FindFlight(AirportModel departure, AirportModel arrival, DateTime departureDate)
        {
            //Direct
            var direct = this.FindAll().Where(flight => flight.DepartureDate.Value.Date == departureDate
                        && flight.Departure.ID == departure.ID && flight.Arrival.ID == arrival.ID).ToList();
            // 1 Transit
            var resutlDeparture = this.FindAll().Where(flight => flight.DepartureDate.Value.Date == departureDate && flight.Departure.ID == departure.ID);
            var resultArrival = this.FindAll().Where(flight => flight.DepartureDate.Value.Date == departureDate && flight.Arrival.ID == arrival.ID);

            var query = from departures in resutlDeparture
                        join arrivals in resultArrival on departures.Arrival.ID equals arrivals.Departure.ID
                        let s = "From " + departures.Departure.Name + " to " + arrivals.Departure.Name + " and From " 
                        + arrivals.Departure.Name + " to " + arrivals.Arrival.Name
                        select s;

            var result = query.ToList();   

            return direct;
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

                //Get Departure and Arrival
                var planeAirportList = this.PlaneAirportService.FindByPlane(model.PlaneID.Value).ToList();

                viewModel.Departure = planeAirportList.Where(airport => airport.DepartureOrArrival != null 
                    && airport.DepartureOrArrival.Split('-').ToList().IndexOf(model.ID + ".d") != -1)
                    .Select(airport => airport.Airport).FirstOrDefault();
                viewModel.Arrival = planeAirportList.Where(airport => airport.DepartureOrArrival != null
                    && airport.DepartureOrArrival.Split('-').ToList().IndexOf(model.ID + ".a") != -1)
                    .Select(airport => airport.Airport).FirstOrDefault();
            }

            return viewModel;
        }
        #endregion
    }
}