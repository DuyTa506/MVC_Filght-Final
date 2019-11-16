using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace EaseFlight.DAL.Repositories
{
    public class PlaneAirportRepository : BaseRepository, IPlaneAirportRepository
    {
        #region Constructors
        public PlaneAirportRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<PlaneAirport> FindAll()
        {
            var result = this.UnitOfWork.DBContext.PlaneAirports;

            return result;
        }

        public void Insert(PlaneAirport planeAirport)
        {
            this.UnitOfWork.DBContext.PlaneAirports.Add(planeAirport);
        }

        public PlaneAirport Find(int planeId, int airportId)
        {
            var result = this.UnitOfWork.DBContext.PlaneAirports.Find(planeId, airportId);

            return result;
        }

        public void Update(PlaneAirport planeAirport)
        {
            var currentPlaneAirport = this.Find(planeAirport.PlaneID, planeAirport.AirportID);

            if (currentPlaneAirport != null)
                CommonMethods.CopyObjectProperties(planeAirport, currentPlaneAirport);
        }

        public IEnumerable<PlaneAirport> FindByPlane(int planeId)
        {
            var result = this.UnitOfWork.DBContext.PlaneAirports.Where(planeAirport => planeAirport.PlaneID == planeId);

            return result;
        }

        public void Delete(int planeid, int planeAirportId)
        {
            var CurrentPlaneAirport = this.UnitOfWork.DBContext.PlaneAirports.Find(planeid,planeAirportId);
            this.UnitOfWork.DBContext.PlaneAirports.Remove(CurrentPlaneAirport);
        }
        #endregion
    }
}