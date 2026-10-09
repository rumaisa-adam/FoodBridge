using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodBridge.Data;

namespace FoodBridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodBridgeDbContext _context;

        public HomeController(FoodBridgeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claimedMeals = _context.FoodItems
                .Include(f => f.ClaimedByUser)
                .Where(f => f.Status == "Claimed")
                .OrderByDescending(f => f.ClaimDate)
                .ToList();

            ViewBag.MealsShared = claimedMeals.Count;

            return View(claimedMeals);
        }
    }
}