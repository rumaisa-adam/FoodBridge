using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodBridge.Data;
using FoodBridge.Models;

namespace FoodBridge.Controllers
{
    public class DonationsController : Controller
    {
        private readonly FoodBridgeDbContext _context;
        private const decimal TARGET_GOAL = 20000m; 

        public DonationsController(FoodBridgeDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Fund()
        {
            CalculateFundingProgress();
            PopulateUserDropdown();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fund(int userID, decimal amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("amount", "Donation amount must be greater than zero.");
            }

            if (ModelState.IsValid)
            {
                var donation = new Donations
                {
                    UserID = userID,
                    Amount = amount,
                    DonationDate = DateTime.Now
                };

                _context.Donations.Add(donation);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Thank you! Your donation of R{amount:N2} has been recorded.";
                return RedirectToAction(nameof(Fund));
            }

            CalculateFundingProgress();
            PopulateUserDropdown();
            return View();
        }


        [HttpGet]
        public IActionResult GetUserPhoneNumber(int userID)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == userID);
            if (user == null)
            {
                return NotFound();
            }

            return Json(new { phoneNumber = user.PhoneNumber });
        }

        private void CalculateFundingProgress()
        {
            decimal totalRaised = _context.Donations.Sum(d => (decimal?)d.Amount) ?? 0m;
            int percentage = (int)Math.Min((totalRaised / TARGET_GOAL) * 100m, 100m);

            ViewBag.TargetGoal = TARGET_GOAL;
            ViewBag.TotalRaised = totalRaised;
            ViewBag.Percentage = percentage;
            ViewBag.GoalReached = totalRaised >= TARGET_GOAL;
        }

        private void PopulateUserDropdown()
        {
            ViewBag.Users = new SelectList(
                _context.Users
                    .OrderBy(u => u.FirstName)
                    .Select(u => new { u.UserID, FullName = u.FirstName + " " + u.LastName })
                    .ToList(),
                "UserID",
                "FullName"
            );
        }
    }
}