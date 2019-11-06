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
    public class TicketFlightService : BaseService, ITicketFlightService
    {
        #region Properties
        private ITicketFlightRepository TicketFlightRepository { get; set; }
        #endregion

        #region Constructors
        public TicketFlightService(IUnitOfWork unitOfWork,
            ITicketFlightRepository ticketFlightRepository) : base(unitOfWork)
        {
            this.TicketFlightRepository = ticketFlightRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<TicketFlightModel> FindAll()
        {
            var modelList = this.TicketFlightRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Insert(TicketFlightModel ticketFlight)
        {
            this.TicketFlightRepository.Insert(ticketFlight.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public IEnumerable<TicketFlightModel> FindByFlight(int flightId)
        {
            var modelList = this.TicketFlightRepository.FindByFlight(flightId);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public IEnumerable<TicketFlightModel> FindByTicket(int ticketId)
        {
            var modelList = this.TicketFlightRepository.FindByTicket(ticketId);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }
        #endregion

        #region Model Functions
        public TicketFlightModel CreateViewModel(TicketFlight model)
        {
            TicketFlightModel viewModel = null;

            if (model != null)
            {
                viewModel = new TicketFlightModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}