using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;
using BarManagement.DataAccess.Interfaces;

namespace BarManagement.DataAccess
{
    public class CocktailsRepository : Repository<EfModels.Cocktails, Models.Cocktails>, ICocktailsRepository
    {
        public CocktailsRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public List<Models.Cocktails> GetCocktails()
        {
            var result = _context.Cocktails.ToList();
            return _mapper.Map<List<Models.Cocktails>>(result);
        }

        public List<List<Models.Cocktails>> GetCocktailsSortedByCategory()
        {
            var allCocktails = GetCocktails();
            var sortCocktails = from cocktail in allCocktails
                                group cocktail by cocktail.CocktailCategory into list
                                select list.ToList();
            return sortCocktails.ToList();
        }
    }
}
