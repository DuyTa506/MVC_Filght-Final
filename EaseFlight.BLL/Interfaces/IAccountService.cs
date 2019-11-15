using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<AccountModel> FindAll();
        AccountModel Find(int id);
        AccountModel FindByUsername(string username);
        AccountModel FindByEmail(string email);
        int Insert(AccountModel account);
        int Update(AccountModel account);
        AccountModel CheckUsernameExists(string username, string email);
        int Delete(int accountId);
    }
}
