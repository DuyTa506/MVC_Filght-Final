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
    public class PassengerTicketService : BaseService, IPassengerTicketService
    {
        #region Properties
        private IPassengerTicketRepository PassengerTicketRepository { get; set; }
        #endregion

        #region Constructors
        public PassengerTicketService(IUnitOfWork unitOfWork,
            IPassengerTicketRepository passengerTicketRepository) : base(unitOfWork)
        {
            this.PassengerTicketRepository = passengerTicketRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<PassengerTicketModel> FindAll()
        {
            var modelList = this.PassengerTicketRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Insert(PassengerTicketModel passengerTicket)
        {
            this.PassengerTicketRepository.Insert(passengerTicket.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }

        public IEnumerable<PassengerTicketModel> FindByTicket(int ticketId)
        {
            var modelList = this.PassengerTicketRepository.FindByTicket(ticketId);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public PassengerTicketModel Find(int passengerTicketId)
        {
            var model = this.PassengerTicketRepository.Find(passengerTicketId);
            var result = this.CreateViewModel(model);

            return result;
        }

        public int Update(PassengerTicketModel passengerTicket)
        {
            this.PassengerTicketRepository.Update(passengerTicket.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }
        #endregion

        #region Model Functions
        public PassengerTicketModel CreateViewModel(PassengerTicket model)
        {
            PassengerTicketModel viewModel = null;

            if (model != null)
            {
                viewModel = new PassengerTicketModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}