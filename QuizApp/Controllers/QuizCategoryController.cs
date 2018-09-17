using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizApp.Context;
using QuizApp.Models;

namespace QuizApp.Controllers
{
    public class QuizCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public QuizCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: QuizCategory
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _context.QuizCategories.ToListAsync();

            //var categories = await _context.QuizCategories.ToListAsync();
            return View();
        }

        // GET: QuizCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizCategoryModel = await _context.QuizCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (quizCategoryModel == null)
            {
                return NotFound();
            }

            return View(quizCategoryModel);
        }

        // GET: QuizCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QuizCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName,ImagePath")] QuizCategoryModel quizCategoryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizCategoryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
            return View(quizCategoryModel);
        }

        // GET: QuizCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizCategoryModel = await _context.QuizCategories.SingleOrDefaultAsync(m => m.Id == id);
            if (quizCategoryModel == null)
            {
                return NotFound();
            }
            return View(quizCategoryModel);
        }

        // POST: QuizCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName,ImagePath")] QuizCategoryModel quizCategoryModel)
        {
            if (id != quizCategoryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizCategoryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizCategoryModelExists(quizCategoryModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Admin");
            }
            return View(quizCategoryModel);
        }

        // GET: QuizCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizCategoryModel = await _context.QuizCategories
                .SingleOrDefaultAsync(m => m.Id == id);
            if (quizCategoryModel == null)
            {
                return NotFound();
            }

            return View(quizCategoryModel);
        }

        // POST: QuizCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizCategoryModel = await _context.QuizCategories.SingleOrDefaultAsync(m => m.Id == id);
            _context.QuizCategories.Remove(quizCategoryModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin");
        }

        private bool QuizCategoryModelExists(int id)
        {
            return _context.QuizCategories.Any(e => e.Id == id);
        }
    }
}
