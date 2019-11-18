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
    public class PlaneService : BaseService, IPlaneService
    {
        #region Properties
        private IPlaneRepository PlaneRepository { get; set; }
        #endregion

        #region Constructors
        public PlaneService(IUnitOfWork unitOfWork,
            IPlaneRepository planeRepository) : base(unitOfWork)
        {
            this.PlaneRepository = planeRepository;
        }
        #endregion

        #region Functions
        public IEnumerable<PlaneModel> FindAll()
        {
            var modelList = this.PlaneRepository.FindAll();
            var result = modelList.Select(model => this.CreateViewModel(model));

            return result;
        }

        public PlaneModel Find(int id)
        {
            var model = this.PlaneRepository.Find(id);
            var result = this.CreateViewModel(model);

            return result;
        }

        public int Insert(PlaneModel plane)
        {
            var ID = this.PlaneRepository.Insert(plane.GetModel());

            return ID;
        }

        public void Update(PlaneModel plane)
        {
            this.PlaneRepository.Update(plane.GetModel());
            var result = this.UnitOfWork.SaveChanges();
        }

        public int Delete(int planeid)
        {
            var result = this.PlaneRepository.Delete(planeid);

            return result;
        }
        #endregion

        #region Model Functions
        public PlaneModel CreateViewModel(Plane model)
        {
            PlaneModel viewModel = null;

            if (model != null)
            {
                viewModel = new PlaneModel();
                CommonMethods.CopyObjectProperties(model, viewModel);
                viewModel.Name = viewModel.Airline + viewModel.ID.ToString("D3");
            }

            return viewModel;
        }


        #endregion
    }
}