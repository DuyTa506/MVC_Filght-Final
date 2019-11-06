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
    public class TicketService : BaseService, ITicketService
    {
        #region Properties
        private ITicketRepository TicketRepository { get; set; }
        #endregion

        #region Constructors
        public TicketService(IUnitOfWork unitOfWork,
            ITicketRepository ticketRepository) : base(unitOfWork)
        {
            this.TicketRepository = ticketRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<TicketModel> FindAll()
        {
            var modelList = this.TicketRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Insert(TicketModel ticket)
        {
            var id = this.TicketRepository.Insert(ticket.GetModel());

            return id;
        }

        public TicketModel Find(int id)
        {
            var model = this.TicketRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }

        public IEnumerable<TicketModel> FindByAccount(int accountId)
        {
            var modelList = this.TicketRepository.FindByAccount(accountId);
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public int Update(TicketModel ticket)
        {
            this.TicketRepository.Update(ticket.GetModel());
            var result = this.UnitOfWork.SaveChanges();

            return result;
        }
        #endregion

        #region Model Functions
        public TicketModel CreateViewModel(Ticket model)
        {
            TicketModel viewModel = null;

            if (model != null)
            {
                viewModel = new TicketModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}