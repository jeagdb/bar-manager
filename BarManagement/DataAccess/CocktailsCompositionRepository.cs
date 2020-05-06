using BarManagement.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarManagement.DataAccess.EfModels;
using AutoMapper;

namespace BarManagement.DataAccess
{
    public class CocktailsCompositionRepository : Repository<EfModels.CocktailsComposition, Models.CocktailsComposition>, ICocktailsCompositionRepository
    {
        public CocktailsCompositionRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
