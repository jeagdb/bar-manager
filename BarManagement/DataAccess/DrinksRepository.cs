using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarManagement.DataAccess.EfModels;
using BarManagement.DataAccess.Interfaces;

namespace BarManagement.DataAccess
{
    public class DrinksRepository : Repository<EfModels.Drinks, Models.Drinks>, IDrinksRepository
    {
        public DrinksRepository(barDBContext context, IMapper mapper)
            : base(context, mapper)
        {
        }
        public Models.Drinks GetDrinkById(long id)
        {
            var result = _context.Drinks.Where(x => x.Id == id).FirstOrDefault();
            return _mapper.Map<Models.Drinks>(result);
        }

        public List<Models.Drinks> GetDrinks()
        {
            var result = _context.Drinks.ToList();
            return _mapper.Map<List<Models.Drinks>>(result);
        }
    }
}
