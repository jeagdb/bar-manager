using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Pages
{
    public static class ModelStateExtensions
    {
        public static ModelStateDictionary MarkAllFieldsAsSkipped(this ModelStateDictionary modelState)
        {
            foreach (var state in modelState.Select(x => x.Value))
            {
                state.Errors.Clear();
                state.ValidationState = ModelValidationState.Skipped;
            }
            return modelState;
        }
    }

    public class IndexModel : PageModel
    {
        private readonly DataAccess.Interfaces.IStocksRepository _stocksRepository;
        private readonly DataAccess.Interfaces.IDrinksRepository _drinksRepository;
        public List<Models.Stocks> Stocks { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
        public List<SelectListItem> DrinksOptions { get; set; }


        public class FormDrinkModel
        {
            [BindProperty]
            public string NAME { get; set; }
            [BindProperty]
            public string BRAND { get; set; }
            [BindProperty]
            public string CATEGORY { get; set; }
        }
        public class FormStockModel
        {
            [BindProperty]
            public long DRINKID { get; set; }
            [BindProperty]
            public long QUANTITY { get; set; }
            [BindProperty]
            public double PRICE { get; set; }
    }

        [BindProperty]
        public FormDrinkModel FormDrink { get; set; }


        [BindProperty]
        public FormStockModel FormStock { get; set; }


        public IndexModel(DataAccess.Interfaces.IStocksRepository stocksRepository, DataAccess.Interfaces.IDrinksRepository drinksRepository)
        {
            _stocksRepository = stocksRepository;
            _drinksRepository = drinksRepository;
        }

        public void OnGetAsync()
        {
            Drinks = _drinksRepository.GetDrinks();
            Stocks = _stocksRepository.GetStocks();
            var CopyDrinks = new List<Models.Drinks>(Drinks);
            DrinksOptions = CopyDrinks.Select(drink => new SelectListItem { Value = drink.Id.ToString(), Text = drink.Name }).ToList();
        }

        public async Task<IActionResult> OnPostDrinks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            if (!TryValidateModel(FormDrink, nameof(FormDrink)))
            {
                return Page();
            }
            await _drinksRepository.Insert(new Models.Drinks() { Name = FormDrink.NAME, Brand = FormDrink.BRAND, Category = FormDrink.CATEGORY });
            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostStocks()
        {
            ModelState.MarkAllFieldsAsSkipped();
            Drinks = _drinksRepository.GetDrinks();
            if (!TryValidateModel(FormStock, nameof(FormStock)) || Drinks == null)
            {
                return Page();
            }
            Models.Drinks drink = Drinks[(int)FormStock.DRINKID];
            var stock = await _stocksRepository.Insert(new Models.Stocks() { DrinkId = drink.Id, Price = FormStock.PRICE, Quantity = FormStock.QUANTITY});
            stock.Drink = drink;
            return Redirect("./Index");
        }
    }
}