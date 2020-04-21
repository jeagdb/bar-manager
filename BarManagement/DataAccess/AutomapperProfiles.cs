using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace BarManagement.DataAccess
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Models.Cocktails, EfModels.Cocktails>();
            CreateMap<EfModels.Cocktails, Models.Cocktails>();

            CreateMap<Models.CocktailsComposition, EfModels.CocktailsComposition>();
            CreateMap<EfModels.CocktailsComposition, Models.CocktailsComposition>();

            CreateMap<Models.Drinks, EfModels.Drinks>();
            CreateMap<EfModels.Drinks, Models.Drinks>();

            CreateMap<Models.Stocks, EfModels.Stocks>();
            CreateMap<EfModels.Stocks, Models.Stocks>();

            CreateMap<Models.Transactions, EfModels.Transactions>();
            CreateMap<EfModels.Transactions, Models.Transactions>();
        }
    }
}
