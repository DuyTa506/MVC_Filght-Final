using EaseFlight.DAL.Entities;
using System.Collections.Generic;

namespace EaseFlight.DAL.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> FindAll();
        Account Find(int id);
        Account FindByUsername(string username);
        Account FindByEmail(string email);
        int Insert(Account account);
        void Update(Account account);
        int Delete(int accountId);
    }
}
