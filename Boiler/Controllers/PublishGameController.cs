using Microsoft.AspNetCore.Mvc;

using Boiler.Models;
using Boiler.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Boiler.DTOs;

namespace Boiler.Controllers
{
    [Authorize]
    public class PublishGameController : Controller
    {
        private readonly BoilerContext _context;
        public PublishGameController(BoilerContext boilerContext)
        {
            _context = boilerContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int userId = UserId.GetId();
            var balance = _context.Accounts.Where(x => x.Id == userId).FirstOrDefault()!.Balance;
            ViewData["balance"] = "Your balance: " + balance.ToString() + "$";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(PublishGameViewModel publishGameViewModel)
        {
            int userId = UserId.GetId();

            var user = _context.Accounts.Where(a => a.Id == userId).FirstOrDefault();
            ViewData["balance"] = "Your balance: " + user!.Balance.ToString() + "$";
            if (user!.Balance < 100)
            {
                ViewData["ErrorMessage"] = "You don't have enough money to publish a game.";
                return View();
            }

            user!.Balance -= 100;

            _context.Accounts.Update(user);
            await _context.SaveChangesAsync();


            Game game = new Game()
            {
                Name = publishGameViewModel.Name,
                Price = publishGameViewModel.Price,
            };

            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();

            if (publishGameViewModel.Categories != null)
            {
                foreach (var item in publishGameViewModel.Categories)
                {
                    GameCategory gameCategory = new GameCategory()
                    {
                        IdGame = game.Id,
                        IdCategory = item
                    };
                    await _context.GameCategories.AddAsync(gameCategory);
                    await _context.SaveChangesAsync();
                }
            }

            AccountGame accountGame = new AccountGame()
            {
                Relation = 2,
                IdAccount = userId,
                IdGame = game.Id,
            };
            
            await _context.AccountGames.AddAsync(accountGame);
            await _context.SaveChangesAsync();

            if (publishGameViewModel.AchievementsNames != null)
            {
                foreach (var item in publishGameViewModel.AchievementsNames)
                {
                    Achievement achievement = new Achievement() {
                        Name = item,
                        IdGame = game.Id,
                    };
                    await _context.Achievements.AddAsync(achievement);
                    await _context.SaveChangesAsync();
                }
            }


            if (game.Id != 0) return RedirectToAction("Index", "Home");

            ViewData["ErrorMessage"] = "Can't publish game.";
            return View();
        }
    }
}
