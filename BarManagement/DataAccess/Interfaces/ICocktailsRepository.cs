using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.DataAccess.Interfaces
{
    public interface ICocktailsRepository : IRepository<EfModels.Cocktails, Models.Cocktails>
    { 
        List<Models.Cocktails> GetCocktails();
        List<List<Models.Cocktails>> GetCocktailsSortedByCategory();
    }
}
