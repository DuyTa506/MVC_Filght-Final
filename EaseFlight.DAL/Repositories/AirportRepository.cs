using EaseFlight.Common.Utilities;
using EaseFlight.DAL.Entities;
using EaseFlight.DAL.Interfaces;
using EaseFlight.DAL.UnitOfWorks;
using System.Collections.Generic;

namespace EaseFlight.DAL.Repositories
{
    public class AirportRepository : BaseRepository, IAirportRepository
    {
        #region Constructors
        public AirportRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        #endregion

        #region Functions
        public IEnumerable<Airport> FindAll()
        {
            var result = this.UnitOfWork.DBContext.Airports;

            return result;
        }

        public Airport Find(int id)
        {
            var result = this.UnitOfWork.DBContext.Airports.Find(id);

            return result;
        }

        public int Insert(Airport airport)
        {
            this.UnitOfWork.DBContext.Airports.Add(airport);

            return airport.ID;
        }

        public void Update(Airport airport)
        {
            var currentAirport = this.UnitOfWork.DBContext.Airports.Find(airport.ID);

            if (currentAirport != null)
            {
                CommonMethods.CopyObjectProperties(airport,currentAirport);
            }
        }

        public void Delete(int airportId)
        {
            var airport = this.UnitOfWork.DBContext.Airports.Find(airportId);

            if (airport.PlaneAirports.Count == 0)
            {
                this.UnitOfWork.DBContext.Airports.Remove(airport);
                this.UnitOfWork.SaveChanges();
            }
        }
        #endregion
    }
}