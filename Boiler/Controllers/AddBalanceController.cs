using Microsoft.AspNetCore.Mvc;

using Boiler.Models;
using Boiler.DTOs;

using Microsoft.AspNetCore.Authorization;

namespace Boiler.Controllers
{
    [Authorize]
    public class AddBalanceController : Controller
    {
        private readonly BoilerContext _context;
        public AddBalanceController(BoilerContext boilerContext)
        {
            _context = boilerContext;
        }

        [HttpGet]
        public IActionResult Index(string message, string type){
            Console.WriteLine("Get");
            int userId = UserId.GetId();

            var balance = _context.Accounts.Where(x => x.Id == userId).FirstOrDefault()!.Balance;
            ViewData["balance"] = "Your balance: " + balance.ToString() + "$";
            if(!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(type)){
                ViewData[type] = message;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(decimal balance){
            Console.WriteLine("Post");
            int userId = UserId.GetId();

            var user = _context.Accounts.Where(a => a.Id == userId).FirstOrDefault();
            user!.Balance += balance;

            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();
            // return View();
            return RedirectToAction("Index", "AddBalance", new 
            {
                message = "Added balance succesfully!",
                type = "success"
            });
        }
    }
}