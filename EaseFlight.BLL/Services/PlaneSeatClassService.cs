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

        public PlaneSeatClassModel Find(int planeId, int seatId)
        {
            var model = this.PlaneSeatClassRepository.Find(planeId, seatId);
            var result = this.CreateViewModel(model);

            return result;
        }

        public IEnumerable<PlaneSeatClassModel> FindByPlane(int planeId)
        {
            var modelList = this.PlaneSeatClassRepository.FindByPlane(planeId);
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

        public void Insert(PlaneSeatClassModel planeSeatClass)
        {
            this.PlaneSeatClassRepository.Insert(planeSeatClass.GetModel());

        }

        public void Update(PlaneSeatClassModel planeSeatClassairport)
        {
            this.PlaneSeatClassRepository.Update(planeSeatClassairport.GetModel());
            this.UnitOfWork.SaveChanges();

        }

        public void Delete(int planeid,int planeseatid)
        {
            this.PlaneSeatClassRepository.Delete(planeid,planeseatid);
            this.UnitOfWork.DBContext.SaveChanges();
        }
        #endregion
    }
}