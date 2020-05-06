using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.DataAccess.Interfaces
{
    public interface ICocktailsCompositionRepository : IRepository<EfModels.CocktailsComposition, Models.CocktailsComposition>
    {
        List<Models.CocktailsComposition> getCompositionByCocktailId(long id);
    }
}
