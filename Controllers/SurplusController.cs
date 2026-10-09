using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodBridge.Data;
using FoodBridge.Models;

namespace FoodBridge.Controllers
{
    public class SurplusController : Controller
    {
        private readonly FoodBridgeDbContext _context;

        public SurplusController(FoodBridgeDbContext context)
        {
            _context = context;
        }

        // =========================================================================
        // REQUIREMENT 2.3: LISTINGS & FILTERING (GET: /Surplus/Listings)
        // =========================================================================
        public IActionResult Listings(int? categoryID, int? itemTypeID, int? suburbID)
        {
            // Populate filter dropdowns
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();
            ViewBag.Categories = new SelectList(categories, "CategoryID", "Name", categoryID);

            var suburbs = _context.Suburbs.OrderBy(s => s.Name).ToList();
            ViewBag.Suburbs = new SelectList(suburbs, "SuburbID", "Name", suburbID);

            if (categoryID.HasValue && categoryID.Value > 0)
            {
                var types = _context.ItemTypes
                    .Where(i => i.CategoryID == categoryID.Value)
                    .OrderBy(i => i.Name)
                    .ToList();
                ViewBag.ItemTypes = new SelectList(types, "ItemTypeID", "Name", itemTypeID);
            }
            else
            {
                ViewBag.ItemTypes = new SelectList(new List<SelectListItem>(), "Value", "Text");
            }

            // Session 19 - Task 4: Eager Loading using .Include()
            // We load Category, Suburb, and PostedByUser upfront with the main SQL query
            // so EF Core executes a single JOIN query and avoids the N+1 problem in the view.
            var query = _context.FoodItems
                .Include(f => f.Category)
                .Include(f => f.ItemType)
                .Include(f => f.Suburb)
                .Include(f => f.Storage)
                .Include(f => f.PostedByUser)
                .AsQueryable();

            // LINQ / Lambda filtering
            if (categoryID.HasValue && categoryID.Value > 0)
            {
                query = query.Where(f => f.CategoryId == categoryID.Value);
            }

            if (itemTypeID.HasValue && itemTypeID.Value > 0)
            {
                query = query.Where(f => f.ItemTypeId == itemTypeID.Value);
            }

            if (suburbID.HasValue && suburbID.Value > 0)
            {
                query = query.Where(f => f.SuburbId == suburbID.Value);
            }

            var listings = query.OrderByDescending(f => f.FoodItemId).ToList();
            return View(listings);
        }

        // =========================================================================
        // REQUIREMENT 2.4: CLAIM SURPLUS (GET & POST)
        // =========================================================================
        [HttpGet]
        public IActionResult Claim(int id)
        {
            // Eager load related details to show in claim view
            var item = _context.FoodItems
                .Include(f => f.Category)
                .Include(f => f.ItemType)
                .Include(f => f.Suburb)
                .Include(f => f.Storage)
                .Include(f => f.PostedByUser)
                .FirstOrDefault(f => f.FoodItemId == id);

            if (item == null || item.Status == "Claimed")
            {
                return RedirectToAction(nameof(Listings));
            }

            ViewBag.Users = new SelectList(
                _context.Users
                    .OrderBy(u => u.FirstName)
                    .Select(u => new { u.UserID, FullName = u.FirstName + " " + u.LastName })
                    .ToList(),
                "UserID",
                "FullName"
            );

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ClaimNow(int foodItemID, int claimedByUserID)
        {
            var item = _context.FoodItems.Find(foodItemID);

            if (item != null && item.Status == "Available")
            {
                item.Status = "Claimed";
                item.ClaimedByUserId = claimedByUserID;
                item.ClaimDate = DateTime.Now;

                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        // =========================================================================
        // REQUIREMENT 2.5: POST SURPLUS (GET & POST)
        // =========================================================================
        [HttpGet]
        public IActionResult Post()
        {
            PopulatePostDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(FoodItem item, IFormFile? imageFile)
        {
            // A.5 Requirement: Validate that uploaded image does not exceed 500 KB (512,000 bytes)
            if (imageFile != null && imageFile.Length > 512000)
            {
                ModelState.AddModelError("imageFile", "Image size must not exceed 500 KB per assignment instructions.");
                PopulatePostDropdowns();
                return View(item);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                // Convert stream to Base64 data URL string (Option B Binary representation)
                using var memoryStream = new MemoryStream();
                await imageFile.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                item.ImageData = $"data:{imageFile.ContentType};base64,{Convert.ToBase64String(fileBytes)}";
            }
            else
            {
                // Safe placeholder fallback
                item.ImageData = "https://images.unsplash.com/photo-1498837167922-ddd27525d352?w=600&auto=format&fit=crop&q=80";
            }

            item.Status = "Available";

            _context.FoodItems.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Listings));
        }

        // =========================================================================
        // JQUERY AJAX ENDPOINTS (Session 06)
        // =========================================================================
        [HttpGet]
        public IActionResult GetItemTypesByCategory(int categoryID)
        {
            var itemTypes = _context.ItemTypes
                .Where(i => i.CategoryID == categoryID)
                .OrderBy(i => i.Name)
                .Select(i => new
                {
                    itemTypeID = i.ItemTypeID,
                    name = i.Name
                })
                .ToList();

            return Json(itemTypes);
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

        private void PopulatePostDropdowns()
        {
            ViewBag.Users = new SelectList(
                _context.Users
                    .OrderBy(u => u.FirstName)
                    .Select(u => new { u.UserID, FullName = u.FirstName + " " + u.LastName })
                    .ToList(),
                "UserID",
                "FullName"
            );

            ViewBag.Categories = new SelectList(
                _context.Categories.OrderBy(c => c.Name).ToList(),
                "CategoryID",
                "Name"
            );

            ViewBag.Suburbs = new SelectList(
                _context.Suburbs.OrderBy(s => s.Name).ToList(),
                "SuburbID",
                "Name"
            );

            ViewBag.Storages = _context.Storages.ToList();
        }
    }
}