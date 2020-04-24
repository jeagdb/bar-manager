using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;

namespace BarManagement.DataAccess
{
    public class CocktailsRepository : Repository<Cocktails, Models.Cocktails>
    {
        public CocktailsRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
