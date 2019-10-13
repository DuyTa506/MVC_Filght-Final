using EaseFlight.DAL.Entities;

namespace EaseFlight.DAL.UnitOfWorks
{
    public interface IUnitOfWork
    {
        EaseFlightEntities DBContext { get; }
        int SaveChanges();
    }
}
