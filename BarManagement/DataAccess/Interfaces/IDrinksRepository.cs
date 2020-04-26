using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.DataAccess.Interfaces
{
    public interface IDrinksRepository : IRepository<EfModels.Drinks, Models.Drinks>
    {
        Models.Drinks GetDrinkById(long id);
        List<Models.Drinks> GetDrinks();
    }
}
