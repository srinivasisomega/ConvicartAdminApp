using Microsoft.AspNetCore.Mvc;
using ConvicartAdminApp.Data;
using ConvicartAdminApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace ConvicartAdminApp.Controllers
{

    [Authorize(Roles = "Admin")]
    public class RecipeStepsController : Controller
    {
        private readonly ConvicartWarehouseContext _context;

        public RecipeStepsController(ConvicartWarehouseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int productId)
        {
            // Fetch recipe steps based on the productId
            var recipeSteps = await _context.RecipeSteps
                                            .Where(r => r.ProductId == productId)
                                            .ToListAsync();

            // If no steps are found, redirect to Create action
            if (recipeSteps == null || recipeSteps.Count == 0)
            {
                return RedirectToAction("Create", new { productId }); // Redirect to Create with the ProductId
            }

            ViewBag.ProductId = productId; // Pass product ID to the view for further actions
            return View(recipeSteps);
        }


        public IActionResult Create(int productId)
        {
            var model = new RecipeSteps { ProductId = productId }; // Assign ProductId from index
            return View(model);
        }

        // POST: RecipeSteps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeSteps model, IFormFile StepImageFile)
        {
            if (ModelState.IsValid)
            {
                if (StepImageFile != null && StepImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await StepImageFile.CopyToAsync(memoryStream);
                        model.Stepimage = memoryStream.ToArray(); // Store image as byte array
                    }
                }

                _context.RecipeSteps.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Store"); // Redirect to Store Index or appropriate page
            }
            return View(model);
        }

        // GET: RecipeSteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeStep = await _context.RecipeSteps.FindAsync(id);
            if (recipeStep == null)
            {
                return NotFound();
            }

            return View(recipeStep);
        }

        // POST: RecipeSteps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeSteps model, IFormFile StepImageFile)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var recipeStep = await _context.RecipeSteps.FindAsync(id);

                    recipeStep.StepDescription = model.StepDescription;
                    recipeStep.StepNumber = model.StepNumber;

                    if (StepImageFile != null && StepImageFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await StepImageFile.CopyToAsync(memoryStream);
                            recipeStep.Stepimage = memoryStream.ToArray();
                        }
                    }

                    _context.Update(recipeStep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeStepExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Store");
            }
            return View(model);
        }

        // GET: RecipeSteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeStep = await _context.RecipeSteps
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipeStep == null)
            {
                return NotFound();
            }

            return View(recipeStep);
        }

        // POST: RecipeSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeStep = await _context.RecipeSteps.FindAsync(id);
            _context.RecipeSteps.Remove(recipeStep);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Store");
        }

        private bool RecipeStepExists(int id)
        {
            return _context.RecipeSteps.Any(e => e.Id == id);
        }
    }

}
