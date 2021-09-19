using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AQAPPLICATION.Data;
using AQAPPLICATION.Models;
using Microsoft.AspNetCore.Authorization;

namespace AQAPPLICATION.Controllers
{
    public class QueasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QueasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Queas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quea.ToListAsync());
        }

        // GET: Queas/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // PoST: Queas/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Quea.Where( q => q.Questions.Contains(SearchPhrase)).ToListAsync());
        }


        // GET: Queas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quea = await _context.Quea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quea == null)
            {
                return NotFound();
            }

            return View(quea);
        }

        // GET: Queas/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Queas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Questions,Answers")] Quea quea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(quea);
        }

        // GET: Queas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quea = await _context.Quea.FindAsync(id);
            if (quea == null)
            {
                return NotFound();
            }
            return View(quea);
        }

        // POST: Queas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Questions,Answers")] Quea quea)
        {
            if (id != quea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QueaExists(quea.Id))
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
            return View(quea);
        }

        // GET: Queas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quea = await _context.Quea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quea == null)
            {
                return NotFound();
            }

            return View(quea);
        }

        // POST: Queas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quea = await _context.Quea.FindAsync(id);
            _context.Quea.Remove(quea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QueaExists(int id)
        {
            return _context.Quea.Any(e => e.Id == id);
        }
    }
}
