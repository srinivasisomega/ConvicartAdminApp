using ConvicartAdminApp.Data;
using ConvicartAdminApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConvicartAdminApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PreferenceController : Controller
    {
        private readonly ConvicartWarehouseContext _context;
        public PreferenceController(ConvicartWarehouseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var preferences = await _context.Preferences.ToListAsync(); // Get all preferences from the database
            return View(preferences); // Pass the list to the view
        }
        [HttpGet]
        public IActionResult CreatePreference()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePreference(Preference preference, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageFile.CopyToAsync(memoryStream);
                        preference.PreferenceImage = memoryStream.ToArray(); // Save image as byte array
                    }
                }

                _context.Preferences.Add(preference);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // Redirect to the list of preferences
            }

            return View(preference);
        }
        [HttpGet]
        public async Task<IActionResult> EditPreference(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preference = await _context.Preferences.FindAsync(id);
            if (preference == null)
            {
                return NotFound();
            }

            return View(preference);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPreference(int id, Preference preference, IFormFile? imageFile)
        {
            if (id != preference.PreferenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            preference.PreferenceImage = memoryStream.ToArray(); // Update image if provided
                        }
                    }
                    else
                    {
                        var existingPreference = await _context.Preferences.AsNoTracking().FirstOrDefaultAsync(p => p.PreferenceId == id);
                        if (existingPreference != null)
                        {
                            preference.PreferenceImage = existingPreference.PreferenceImage; // Keep existing image if no new one is uploaded
                        }
                    }

                    _context.Update(preference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreferenceExists(preference.PreferenceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }

            return View(preference);
        }

        private bool PreferenceExists(int id)
        {
            return _context.Preferences.Any(e => e.PreferenceId == id);
        }
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> DeletePreference(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var preference = await _context.Preferences
                .FirstOrDefaultAsync(m => m.PreferenceId == id);

            if (preference == null)
            {
                return NotFound();
            }

            _context.Preferences.Remove(preference);
            await _context.SaveChangesAsync();

            // Redirect to the Index action in the Preference controller
            return RedirectToAction("Index", "Preference");
        }





    }
}
