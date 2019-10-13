using EaseFlight.DAL.Entities;

namespace EaseFlight.DAL.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        public EaseFlightEntities DBContext { get; private set; }
        #endregion

        #region Constructors
        public UnitOfWork()
        {
            this.DBContext = new EaseFlightEntities();
        }

        public int SaveChanges()
        {
            return this.DBContext.SaveChanges();
        }
        #endregion
    }
}
