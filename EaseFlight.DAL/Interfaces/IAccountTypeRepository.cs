using EaseFlight.DAL.Entities;

namespace EaseFlight.DAL.Interfaces
{
    public interface IAccountTypeRepository
    {
        AccountType FindByName(string name);
    }
}
