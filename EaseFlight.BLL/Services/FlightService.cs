using EaseFlight.BLL.Interfaces;
using EaseFlight.Common.Constants;
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
        private IPlaneService PlaneService { get; set; }
        #endregion

        #region Constructors
        public FlightService(IUnitOfWork unitOfWork,
            IFlightRepository flightRepository, IPlaneAirportService planeAirportService,
            IPlaneService planeService) : base(unitOfWork)
        {
            this.FlightRepository = flightRepository;
            this.PlaneAirportService = planeAirportService;
            this.PlaneService = planeService;
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
            var result = this.FlightRepository.Insert(flight.GetModel());

            return result;
        }

        public FlightModel Find(int id)
        {
            var model = this.FlightRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }

        public IEnumerable<SearchFlightModel> FindFlight(AirportModel departure, AirportModel arrival, DateTime departureDate)
        {
            //Direct
            var result = this.FindAll().Where(flight => flight.DepartureDate.Value.Date == departureDate
                        && flight.Departure.ID == departure.ID && flight.Arrival.ID == arrival.ID && flight.Status.Equals(Constant.CONST_FLIGHT_STATUS_READY))
                        .Select(flight => new SearchFlightModel { FlightList = new List<FlightModel> { flight }, Price = flight.Price.Value }).ToList();
                        
            // 1 Transit
            var resultDeparture = this.FindAll().Where(flight => flight.DepartureDate.Value.Date == departureDate && flight.Departure.ID == departure.ID
                                    && flight.Status.Equals(Constant.CONST_FLIGHT_STATUS_READY));
            var resultArrival = this.FindAll().Where(flight => flight.DepartureDate.Value.Date == departureDate && flight.Arrival.ID == arrival.ID
                                    && flight.Status.Equals(Constant.CONST_FLIGHT_STATUS_READY));

            var flightTransit = from departures in resultDeparture
                                join arrivals in resultArrival on departures.Arrival.ID equals arrivals.Departure.ID
                        where arrivals.DepartureDate > departures.ArrivalDate
                        let flight = new List<FlightModel> { departures, arrivals}
                        select flight;

            foreach(var flightList in flightTransit)
                result.Add(new SearchFlightModel { FlightList = flightList, Price = flightList.Select(flight => flight.Price.Value).Sum() });

            return result;
        }

        public IEnumerable<FlightModel> FindByTicket(int ticketId, bool roundTrip = false)
        {
            var modelList = this.FlightRepository.FindByTicket(ticketId, roundTrip);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Update(FlightModel flight)
        {
            this.FlightRepository.Update(flight.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public IEnumerable<FlightModel> FindByDateAndPlane(int planeId, DateTime departDate)
        {
            var modelList = this.FlightRepository.FindByDateAndPlane(planeId, departDate);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Delete(int flightId)
        {
            var result = this.FlightRepository.Delete(flightId);

            return result;
        }

        public void UpdateFlightDone()
        {
            var flights = FindAll().Where(flight => !flight.Status.Equals(Constant.CONST_FLIGHT_STATUS_DONE)).ToList();

            foreach(var flight in flights)
            {
                var currentPlane = this.PlaneService.Find(flight.PlaneID.Value);

                if (DateTime.Now >= flight.DepartureDate && DateTime.Now <= flight.ArrivalDate)
                {
                    flight.Status = Constant.CONST_FLIGHT_STATUS_ONLINE;
                    currentPlane.Status = Constant.CONST_PLANE_STATUS_ONLINE;

                    this.PlaneService.Update(currentPlane);
                    Update(flight);
                }else if(DateTime.Now >= flight.ArrivalDate)
                {
                    flight.Status = Constant.CONST_FLIGHT_STATUS_DONE;
                    currentPlane.Status = Constant.CONST_PLANE_STATUS_READY;

                    this.PlaneService.Update(currentPlane);
                    Update(flight);
                }
            }
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