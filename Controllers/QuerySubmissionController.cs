using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ConvicartAdminApp.Data;
using ConvicartAdminApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ConvicartAdminApp.Controllers
{
    public class QuerySubmissionController : Controller
    {
        private readonly ConvicartWarehouseContext _context;

        public QuerySubmissionController(ConvicartWarehouseContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.QuerySubmissions.ToListAsync());
        }

        // GET: QuerySubmission/Details/5
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var querySubmission = await _context.QuerySubmissions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (querySubmission == null)
            {
                return NotFound();
            }

            return View(querySubmission);
        }

        // GET: QuerySubmission/Create
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuerySubmission/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> Create([Bind("Id,Name,Mobile,Email,Description,Status")] QuerySubmission querySubmission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(querySubmission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(querySubmission);
        }

        // GET: QuerySubmission/Edit/5
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var querySubmission = await _context.QuerySubmissions.FindAsync(id);
            if (querySubmission == null)
            {
                return NotFound();
            }
            return View(querySubmission);
        }

        // POST: QuerySubmission/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Mobile,Email,Description,Status")] QuerySubmission querySubmission)
        {
            if (id != querySubmission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(querySubmission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuerySubmissionExists(querySubmission.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(querySubmission);
        }

        // GET: QuerySubmission/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var querySubmission = await _context.QuerySubmissions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (querySubmission == null)
            {
                return NotFound();
            }

            return View(querySubmission);
        }

        // POST: QuerySubmission/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var querySubmission = await _context.QuerySubmissions.FindAsync(id);
            if (querySubmission != null)
            {
                _context.QuerySubmissions.Remove(querySubmission);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuerySubmissionExists(int id)
        {
            return _context.QuerySubmissions.Any(e => e.Id == id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> MarkAsResolving(int id)
        {
            var querySubmission = await _context.QuerySubmissions.FindAsync(id);
            if (querySubmission == null)
            {
                return NotFound();
            }

            querySubmission.Status = QueryStatus.ResolvingQuery;
            _context.Update(querySubmission);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, CustomerCareExec")]
        public async Task<IActionResult> MarkAsResolved(int id)
        {
            var querySubmission = await _context.QuerySubmissions.FindAsync(id);
            if (querySubmission == null)
            {
                return NotFound();
            }

            querySubmission.Status = QueryStatus.Resolved;
            _context.Update(querySubmission);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
