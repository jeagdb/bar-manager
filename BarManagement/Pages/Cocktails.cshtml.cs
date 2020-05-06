using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Composition;

namespace BarManagement.Pages
{
    public static class SessionExtensions
    {
        public static T GetComplexData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetComplexData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
    public class CocktailsModel : PageModel
    {
        private readonly DataAccess.Interfaces.IDrinksRepository _drinksRepository;
        private readonly DataAccess.Interfaces.ICocktailsRepository _cocktailsRepository;
        private readonly DataAccess.Interfaces.ICocktailsCompositionRepository _cocktailsCompositionRepository;

        public List<Models.Cocktails> Cocktails { get; set; }
        public List<Models.Drinks> Drinks { get; set; }
        public List<SelectListItem> DrinksOptions { get; set; }
        [BindProperty]
        public List<Models.CocktailsComposition> Compositions { get; set; }
        public CocktailsModel(DataAccess.Interfaces.ICocktailsRepository cocktailsRepository,
            DataAccess.Interfaces.IDrinksRepository drinksRepository,
            DataAccess.Interfaces.ICocktailsCompositionRepository cocktailsCompositionRepository)
        {
            _cocktailsRepository = cocktailsRepository;
            _drinksRepository = drinksRepository;
            _cocktailsCompositionRepository = cocktailsCompositionRepository;
            Compositions = new List<Models.CocktailsComposition>();
        }

        public class FormCocktailModel
        {
            [BindProperty]
            [Required]
            public string NAME { get; set; }
            [BindProperty]
            [Required]
            public string PRICE { get; set; }
        }

        [BindProperty]
        public FormCocktailModel FormCocktail { get; set; }

        public void OnGetAsync()
        {
            Drinks = _drinksRepository.GetDrinks();
            Cocktails = _cocktailsRepository.GetCocktails();
        }
        public async Task<IActionResult> OnPostCocktails()
        {
            ModelState.MarkAllFieldsAsSkipped();
            Drinks = _drinksRepository.GetDrinks();
            if (!TryValidateModel(FormCocktail, nameof(FormCocktail)))
            {
                return Page();
            }
            Models.Cocktails newCocktail = await _cocktailsRepository.Insert(new Models.Cocktails() { Name = FormCocktail.NAME, PriceToSell = Double.Parse(FormCocktail.PRICE) });
            int index = 0;
            bool containAlcool = false;
            foreach (Models.CocktailsComposition composition in Compositions)
            {
                var drinkId = Int32.Parse(Request.Form["drinkSelected_" + index ]);
                Models.Drinks drink = Drinks.First(drink => drink.Id == drinkId);
                if (drink.Category == "Alcool") { containAlcool = true;  }
                await _cocktailsCompositionRepository.Insert(new Models.CocktailsComposition() { DrinkId = drink.Id, CocktailId = newCocktail.Id, Quantity = composition.Quantity });
                index++;
            }
            if (containAlcool && Compositions.Count > 1)
            {
                newCocktail.CocktailCategory = "Cocktail";
            } else if (containAlcool)
            {
                newCocktail.CocktailCategory = "Alcool";
            }
            else if (Compositions.Count > 1)
            {
                newCocktail.CocktailCategory = "Cocktail sans alcool";
            }
            else
            {
                newCocktail.CocktailCategory = "Soft";
            }
            await _cocktailsRepository.Update(newCocktail);
            return Redirect("./Cocktails");
        }
        public void OnPostAdd()
        {
            Drinks = _drinksRepository.GetDrinks();
            var CopyDrinks = new List<Models.Drinks>(Drinks);
            DrinksOptions = CopyDrinks.Select(drink => new SelectListItem { Value = drink.Id.ToString(), Text = drink.Name }).ToList();
            Compositions.Add(new Models.CocktailsComposition());
        }
        public void OnPostRemove(int index)
        {
            Drinks = _drinksRepository.GetDrinks();
            var CopyDrinks = new List<Models.Drinks>(Drinks);
            DrinksOptions = CopyDrinks.Select(drink => new SelectListItem { Value = drink.Id.ToString(), Text = drink.Name }).ToList();
            Compositions.RemoveAt(index);
        }

        public async Task<IActionResult> OnPostUpdateCocktail(long id)
        {
            ModelState.MarkAllFieldsAsSkipped();
            Cocktails = _cocktailsRepository.GetCocktails();
            if (!TryValidateModel(FormCocktail, nameof(FormCocktail)))
            {
                return Page();
            }
            Models.Cocktails coktail = Cocktails.First(cocktail => cocktail.Id == id);
            var cocktail = await _cocktailsRepository.Update(new Models.Cocktails() { Name = FormCocktail.NAME, PriceToSell = Double.Parse(FormCocktail.PRICE)});
            return Redirect("./Cocktails");
        }

        public async Task<IActionResult> OnPostRemoveCocktail(long id)
        {
            var listCompo = _cocktailsCompositionRepository.getCompositionByCocktailId(id);
            listCompo.ForEach(composition => {
                removeComposition(composition.Id);
            });
            await _cocktailsRepository.Delete(id);
            return Redirect("./Cocktails");
        }

        public async void removeComposition(long id)
        {
            await _cocktailsCompositionRepository.Delete(id);
        }
    }
}