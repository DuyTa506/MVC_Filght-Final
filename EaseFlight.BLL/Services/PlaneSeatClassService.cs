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
    public class PlaneSeatClassService : BaseService, IPlaneSeatClassService
    {
        #region Properties
        private IPlaneSeatClassRepository PlaneSeatClassRepository { get; set; }
        #endregion

        #region Constructors
        public PlaneSeatClassService(IUnitOfWork unitOfWork,
            IPlaneSeatClassRepository planeSeatClassRepository) : base(unitOfWork)
        {
            this.PlaneSeatClassRepository = planeSeatClassRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<PlaneSeatClassModel> FindAll()
        {
            var modelList = this.PlaneSeatClassRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }
        #endregion

        #region Model Functions
        public PlaneSeatClassModel CreateViewModel(PlaneSeatClass model)
        {
            PlaneSeatClassModel viewModel = null;

            if (model != null)
            {
                viewModel = new PlaneSeatClassModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
            }

            return viewModel;
        }
        #endregion
    }
}