using EaseFlight.Models.EntityModels;

namespace EaseFlight.BLL.Interfaces
{
    public interface IAccountTypeService
    {
        AccountTypeModel FindByName(string name);
    }
}
