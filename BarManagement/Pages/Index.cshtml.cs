using BarManagement.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DataAccess.Interfaces.ICocktailsRepository _coktailsRepository;
        private readonly DataAccess.Interfaces.ICocktailsCompositionRepository _cocktailsCompositionRepository;
        private readonly DataAccess.Interfaces.IStocksRepository _stocksRepository;
        private readonly DataAccess.Interfaces.IDrinksRepository _drinksRepository;
        private readonly DataAccess.Interfaces.ITransactionsRepository _transactionsRepository;
        public List<Models.Cocktails> Cocktails { get; set; }
        public List<Models.Stocks> Stocks { get; set; }
        public List<Models.Drinks> Drinks { get; set; }

        public List<Models.Cocktails> alcoolList { get; set; }
        public List<Models.Cocktails> cocktailList { get; set; }
        public List<Models.Cocktails> virginCocktailList { get; set; }
        public List<Models.Cocktails> softList { get; set; }

        public class FormUnitSoldModel
        {
            [BindProperty]
            [Required]
            public string NAME { get; set; }
            [BindProperty]
            [Required]
            public int QUANTITY { get; set; }
        }

        [BindProperty]
        public FormUnitSoldModel FormUnitSold { get; set; }

        public IndexModel(ICocktailsRepository coktailsRepository,
            IStocksRepository stocksRepository,
            ICocktailsCompositionRepository cocktailsCompositionRepository,
            IDrinksRepository drinksRepository,
            ITransactionsRepository transactionsRepository)
        {
            _coktailsRepository = coktailsRepository;
            _cocktailsCompositionRepository = cocktailsCompositionRepository;
            _stocksRepository = stocksRepository;
            _transactionsRepository = transactionsRepository;
            _drinksRepository = drinksRepository;

            Cocktails = _coktailsRepository.GetCocktails();
            Stocks = _stocksRepository.GetStocks();
            Drinks = _drinksRepository.GetDrinks();

            alcoolList = new List<Models.Cocktails>();
            cocktailList = new List<Models.Cocktails>();
            virginCocktailList = new List<Models.Cocktails>();
            softList = new List<Models.Cocktails>();
        }

        public void OnGetAsync()
        {
            foreach(Models.Cocktails cocktail in Cocktails)
            {
                switch (cocktail.CocktailCategory)
                {
                    case "Alcool":
                        alcoolList.Add(cocktail);
                        break;
                    case "Cocktail":
                        cocktailList.Add(cocktail);
                        break;
                    case "Cocktail sans alcool":
                        virginCocktailList.Add(cocktail);
                        break;
                    case "Soft":
                        softList.Add(cocktail);
                        break;
                }
            }
        }

        public async Task<IActionResult> OnPostUnitSold()
        {
            Cocktails = _coktailsRepository.GetCocktails();
            Stocks = _stocksRepository.GetStocks();
            Drinks = _drinksRepository.GetDrinks();
            if (!TryValidateModel(FormUnitSold, nameof(FormUnitSold)))
            {
                ModelState.AddModelError("Form", "Invalid Form");
                return Redirect("./");
            }
            string cocktailSoldName = FormUnitSold.NAME;
            int cocktailSoldQuantity = FormUnitSold.QUANTITY;
            Models.Cocktails cocktailSold = Cocktails.FirstOrDefault(cocktail => cocktail.Name == cocktailSoldName);

            List<Models.CocktailsComposition> cocktailSoldCompo = _cocktailsCompositionRepository.getCompositionByCocktailId(cocktailSold.Id);

            foreach (var compoUnit in cocktailSoldCompo)
            {
                var drinkId = compoUnit.DrinkId;
                var drinkQuantity = compoUnit.Quantity;
                Models.Drinks drinkUnit = Drinks.FirstOrDefault(drink => drink.Id == drinkId);
                Models.Stocks drinkUnitStock = Stocks.FirstOrDefault(stock => stock.DrinkId == drinkUnit.Id);

                if (cocktailSoldQuantity * drinkQuantity > drinkUnitStock.Quantity)
                {
                    ModelState.AddModelError("Quantity error", "No more available");
                    Redirect("./");
                }
                drinkUnitStock.Quantity -= cocktailSoldQuantity * drinkQuantity;
                await _stocksRepository.Update(drinkUnitStock);
            }

            await _transactionsRepository.Insert(new Models.Transactions() {
                SellDate = DateTime.Now,
                Value = cocktailSold.PriceToSell * FormUnitSold.QUANTITY,
                CocktailId = cocktailSold.Id
            });
            return Redirect("./");
        }
    }
}