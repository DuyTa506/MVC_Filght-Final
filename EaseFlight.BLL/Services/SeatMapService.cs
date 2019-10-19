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
    public class SeatMapService : BaseService, ISeatMapService
    {
        #region Properties
        private ISeatMapRepository SeatMapRepository { get; set; }
        #endregion

        #region Constructors
        public SeatMapService(IUnitOfWork unitOfWork,
            ISeatMapRepository seatMapRepository) : base(unitOfWork)
        {
            this.SeatMapRepository = seatMapRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<SeatMapModel> FindAll()
        {
            var modelList = this.SeatMapRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }
        #endregion

        #region Model Functions
        public SeatMapModel CreateViewModel(SeatMap model)
        {
            SeatMapModel viewModel = null;

            if (model != null)
            {
                viewModel = new SeatMapModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}