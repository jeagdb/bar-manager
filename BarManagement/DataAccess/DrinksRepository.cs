using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;

namespace BarManagement.DataAccess
{
    public class DrinksRepository : Repository<Drinks, Models.Drinks>
    {
        public DrinksRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
    }
}
