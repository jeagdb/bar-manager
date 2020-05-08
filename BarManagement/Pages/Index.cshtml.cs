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
        private readonly DataAccess.Interfaces.IStocksRepository _stocksRepository;
        private readonly DataAccess.Interfaces.ITransactionsRepository _transactionsRepository;
        public List<Models.Cocktails> Cocktails { get; set; }
        public List<Models.Stocks> Stocks { get; set; }

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
            ITransactionsRepository transactionsRepository)
        {
            _coktailsRepository = coktailsRepository;
            _stocksRepository = stocksRepository;
            _transactionsRepository = transactionsRepository;

            Cocktails = new List<Models.Cocktails>();

            alcoolList = new List<Models.Cocktails>();
            cocktailList = new List<Models.Cocktails>();
            virginCocktailList = new List<Models.Cocktails>();
            softList = new List<Models.Cocktails>();
        }

        public void OnGetAsync()
        {
            Cocktails = _coktailsRepository.GetCocktails();
            Stocks = _stocksRepository.GetStocks();
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

            if (!TryValidateModel(FormUnitSold, nameof(FormUnitSold)))
            {
                ModelState.AddModelError("Form", "Invalid Form");
                return Redirect("./");
            }
            string unitSoldName = FormUnitSold.NAME;
            int unitSoldQuantity = FormUnitSold.QUANTITY;
            Models.Cocktails cocktailSold = Cocktails.First(cocktail => cocktail.Name == unitSoldName);

            await _transactionsRepository.Insert(new Models.Transactions() {
                SellDate = DateTime.Now,
                Value = cocktailSold.PriceToSell * FormUnitSold.QUANTITY,
                CocktailId = cocktailSold.Id
            });
            return Redirect("./");
        }
    }
}