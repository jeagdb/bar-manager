using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BarManagement.Pages
{
    public class TransactionsModel : PageModel
    {
        private readonly DataAccess.Interfaces.ITransactionsRepository _transactionsRepository;
        public List<Models.Transactions> Transactions { get; set; }
        public List<SelectListItem> TransactionsOptions { get; set; }

        public TransactionsModel(DataAccess.Interfaces.ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public void OnGetAsync()
        {
            Transactions = _transactionsRepository.GetTransactions();
            var CopyTransactions = new List<Models.Transactions>(Transactions);
            TransactionsOptions = CopyTransactions.Select(transaction => new SelectListItem { 
                Value = transaction.Id.ToString(), Text = transaction.Value.ToString() }).ToList();
        }
    }
}