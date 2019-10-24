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
    public class SeatClassService : BaseService, ISeatClassService
    {
        #region Properties
        private ISeatClassRepository SeatClassRepository { get; set; }
        #endregion

        #region Constructors
        public SeatClassService(IUnitOfWork unitOfWork,
            ISeatClassRepository seatClassRepository) : base(unitOfWork)
        {
            this.SeatClassRepository = seatClassRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<SeatClassModel> FindAll()
        {
            var modelList = this.SeatClassRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public SeatClassModel Find(int id)
        {
            var model = this.SeatClassRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }
        #endregion

        #region Model Functions
        public SeatClassModel CreateViewModel(SeatClass model)
        {
            SeatClassModel viewModel = null;

            if (model != null)
            {
                viewModel = new SeatClassModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}