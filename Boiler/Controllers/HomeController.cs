using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Boiler.Models;
using Boiler.ViewModels;
using Boiler.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Boiler.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BoilerContext _context;
        public HomeController(BoilerContext boilerContext)
        {
            _context = boilerContext;
        }

        [HttpGet]
        public IActionResult Index(string search, string message, string type)
        {
            int id_user = UserId.GetId();

            var results = _context.Accounts
                .Join(_context.AccountGames, accounts => accounts.Id, accountGames => accountGames.IdAccount,
                (accounts, accountGames) => new {accounts, accountGames})
                .Join(_context.Games, ag => ag.accountGames.IdGame, games => games.Id,
                (ag, games) => new {ag, games})
                .GroupBy(g => g.games.Name)
                .Select(
                    r => new Result {
                        Id = r.FirstOrDefault()!.games.Id,
                        Name = r.FirstOrDefault()!.games.Name,
                        Price = r.FirstOrDefault()!.games.Price,
                        Creator = r.FirstOrDefault()!.ag.accounts.Name,
                        Relation = r.Where(c => c.ag.accountGames.IdAccount == id_user).Select(c => c.ag.accountGames.Relation).FirstOrDefault(),
                    }
                )
                .Join(_context.GameCategories, games => games.Id, gameCategories => gameCategories.IdGame,
                (games, gameCategories) => new {games, gameCategories})
                .Join(_context.Categories, gc => gc.gameCategories.IdCategory, categories => categories.Id,
                (gc, categories) => new {gc, categories})
                .GroupBy(x=>x.gc.games.Name)
                .Select(r => new Result {
                    Id = r.FirstOrDefault()!.gc.games.Id,
                    Name = r.FirstOrDefault()!.gc.games.Name,
                    Creator = r.FirstOrDefault()!.gc.games.Creator,
                    Price = r.FirstOrDefault()!.gc.games.Price,
                    Categories = r.Where(c => c.gc.gameCategories.IdCategory == c.categories.Id).Select(c => c.categories.Name).ToList()!,
                    Relation = r.FirstOrDefault()!.gc.games.Relation,
                }).AsEnumerable();
            
            if(!String.IsNullOrEmpty(search))
            {
                results = results.Where(r => r.Name!.Contains(search) || r.Categories!.Any(c => c.Contains(search)));
            }

            var balance = _context.Accounts.Where(x => x.Id == id_user).FirstOrDefault()!.Balance;
            ViewData["balance"] = "Your balance: " + balance.ToString() + "$";
            if(!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(type)){
                ViewData[type] = message;
            }
            
            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AccountGameViewModel request){

            int user_id = UserId.GetId();

            AccountGame accountGames = new AccountGame();

            if (request.Wish != null)
            {
                accountGames.Relation = 0;
                accountGames.IdGame = request.Wish;
                Console.WriteLine("wish: " + request.Wish);
            }
            if (request.Buy != null)
            {
                Console.WriteLine("buy: " + request.Buy);

                var user = _context.Accounts
                                .Where(a => a.Id == user_id).FirstOrDefault();

                decimal price = (decimal)_context.Games
                                .Where(x => x.Id == request.Buy).FirstOrDefault()!.Price!;

                

                if (user!.Balance < price)
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "You don't have enough money to buy this game.",
                        type = "error"
                    });
                }

                user!.Balance -= price;
                
                _context.Accounts.Update(user);
                await _context.SaveChangesAsync();

                var buy = _context.AccountGames.Where(x => x.IdGame == request.Buy && x.IdAccount == user_id).FirstOrDefault();
                
                if (buy != null)
                {
                    buy.Relation = 1;
                    _context.AccountGames.Update(buy);
                    await _context.SaveChangesAsync();
                } else {
                    accountGames.Relation = 1;
                    accountGames.IdGame = request.Buy;
                    accountGames.IdAccount = user_id;
                    await _context.AccountGames.AddAsync(accountGames);
                    await _context.SaveChangesAsync();
                }

                var creator = _context.Accounts
                                .Where(c => c.Id == _context.AccountGames
                                .Where(a => a.Relation == 2 && a.IdGame == request.Buy).FirstOrDefault()!.IdAccount)
                                .FirstOrDefault();
                
                creator!.Balance += price;

                _context.Accounts.Update(creator);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home", new {message = "Game successfully purchased!", type = "success"});
            }
            if (request.UnWish != null)
            {
                Console.WriteLine("unwish: " + request.Buy);
                var delete = _context.AccountGames.Where(x => x.IdGame == request.UnWish && x.IdAccount == user_id).FirstOrDefault();
                if (delete != null)
                {
                    _context.AccountGames.Remove(delete);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            accountGames.IdAccount = user_id;

            await _context.AccountGames.AddAsync(accountGames);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.SetInt32("id_user", 0);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LogIn", "LogIn");
        }
    }
}
