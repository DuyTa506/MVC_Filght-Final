using EaseFlight.Models.EntityModels;
using System.Collections.Generic;

namespace EaseFlight.BLL.Interfaces
{
    public interface ISeatClassService
    {
        IEnumerable<SeatClassModel> FindAll();
        SeatClassModel Find(int id);
    }
}