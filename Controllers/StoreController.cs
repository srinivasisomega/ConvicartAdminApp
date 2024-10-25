using Microsoft.AspNetCore.Mvc;
using ConvicartAdminApp.Data;
using ConvicartAdminApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.IO;
using ConvicartAdminApp.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ConvicartAdminApp.Controllers
{

    [Authorize(Roles = "Admin")] // Ensure only Admin can access
    public class StoreController : Controller
    {
        private readonly ConvicartWarehouseContext _context;

        public StoreController(ConvicartWarehouseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var stores = await _context.Stores.ToListAsync(); // Fetches all stores
            return View(stores); // Passes list to view
        }

        // GET: Admin/CreateStore
        public IActionResult CreateStore()
        {
            // Fetch preferences from the database
            var preferences = _context.Preferences.Select(p => new SelectListItem
            {
                Value = p.PreferenceId.ToString(),
                Text = p.PreferenceName
            }).ToList();

            // Pass the preferences list to the view using ViewBag or a ViewModel
            ViewBag.Preferences = preferences;

            return View();
        }


        // POST: Admin/CreateStore
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStore(StoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var store = new Store
                {
                    ProductName = model.ProductName,
                    ProductDescription = model.ProductDescription,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    Carbs = model.Carbs,
                    Proteins = model.Proteins,
                    Vitamins = model.Vitamins,
                    Minerals = model.Minerals,
                    CookTime = model.CookTime,
                    PrepTime = model.PrepTime,
                    Difficulty = model.Difficulty,
                    PreferenceId = model.PreferenceId,
                    imgUrl = model.imgUrl,
                    Rating = model.Rating
                };

                // Handling file upload
                if (model.ProductImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ProductImage.CopyToAsync(memoryStream);
                        store.ProductImage = memoryStream.ToArray();
                    }
                }

                _context.Stores.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); // Redirect after successful creation
            }
            ViewBag.Preferences = _context.Preferences.Select(p => new SelectListItem
            {
                Value = p.PreferenceId.ToString(),
                Text = p.PreferenceName
            }).ToList();
            return View(model); // If invalid, re-render the form
        }
        // GET: Admin/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            var model = new StoreViewModel
            {
                ProductName = store.ProductName,
                ProductDescription = store.ProductDescription,
                Price = store.Price,
                Quantity = store.Quantity,
                Carbs = store.Carbs,
                Proteins = store.Proteins,
                Vitamins = store.Vitamins,
                Minerals = store.Minerals,
                CookTime = store.CookTime,
                PrepTime = store.PrepTime,
                Difficulty = store.Difficulty,
                PreferenceId = store.PreferenceId,
                imgUrl = store.imgUrl,
                Rating = store.Rating
            };

            // Populate Preferences dropdown
            ViewBag.Preferences = _context.Preferences.Select(p => new SelectListItem
            {
                Value = p.PreferenceId.ToString(),
                Text = p.PreferenceName
            }).ToList();

            return View(model);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreViewModel model)
        {
            if (id == null || !_context.Stores.Any(s => s.ProductId == id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null)
                {
                    return NotFound();
                }

                store.ProductName = model.ProductName;
                store.ProductDescription = model.ProductDescription;
                store.Price = model.Price;
                store.Quantity = model.Quantity;
                store.Carbs = model.Carbs;
                store.Proteins = model.Proteins;
                store.Vitamins = model.Vitamins;
                store.Minerals = model.Minerals;
                store.CookTime = model.CookTime;
                store.PrepTime = model.PrepTime;
                store.Difficulty = model.Difficulty;
                store.PreferenceId = model.PreferenceId;
                store.imgUrl = model.imgUrl;
                store.Rating = model.Rating;

                // Handling file upload for ProductImage
                if (model.ProductImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ProductImage.CopyToAsync(memoryStream);
                        store.ProductImage = memoryStream.ToArray();
                    }
                }

                _context.Update(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown in case of errors
            ViewBag.Preferences = _context.Preferences.Select(p => new SelectListItem
            {
                Value = p.PreferenceId.ToString(),
                Text = p.PreferenceName
            }).ToList();

            return View(model); // Re-render the form if validation fails
        }


        // GET: Admin/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Stores
                .FirstOrDefaultAsync(m => m.ProductId == id);
            
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            if (store == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Store");
        }





    }

}
