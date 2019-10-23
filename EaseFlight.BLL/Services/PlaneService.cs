﻿using EaseFlight.BLL.Interfaces;
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