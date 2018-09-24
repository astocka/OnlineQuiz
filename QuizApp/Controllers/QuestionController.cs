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
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Question
        public async Task<IActionResult> Index()
        {
            return View(await _context.Questions.Include(q => q.Quiz).ThenInclude(c => c.Category).ToListAsync());
        }

        // GET: Question/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionModel = await _context.Questions
                .SingleOrDefaultAsync(m => m.Id == id);
            if (questionModel == null)
            {
                return NotFound();
            }

            return View(questionModel);
        }

        // GET: Question/Create
        public async Task<IActionResult> Create(int? quizId, int questionNumber)
        {
            if (quizId == null)
            {
                return NotFound();
            }
            else
            {
                var quiz = await _context.Quizzes.FirstOrDefaultAsync(c => c.Id == quizId);
                ViewBag.QuizTitle = quiz.Title;
                ViewBag.QuizId = quizId;
                if (quiz.TotalQuestions >= questionNumber)
                {
                    ViewBag.QuestionNumber = questionNumber;
                    return View();
                }
                else
                {
                    return Content("You cannot add more questions than declared in quiz");
                }
            }
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,CorrectAnswer,QuizId,QuestionNumber")] QuestionModel questionModel)
        {
            if (ModelState.IsValid)
            {

                _context.Questions.Include(q => q.Quiz).ThenInclude(c => c.Category);
                _context.Add(questionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
            return View(questionModel);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionModel = await _context.Questions.Include(q => q.Quiz).SingleOrDefaultAsync(m => m.Id == id);
            if (questionModel == null)
            {
                return NotFound();
            }
            return View(questionModel);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,CorrectAnswer,QuizTitle,QuizId")] QuestionModel questionModel)
        {
            if (id != questionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(questionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionModelExists(questionModel.Id))
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
            return View(questionModel);
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionModel = await _context.Questions
                .SingleOrDefaultAsync(m => m.Id == id);
            if (questionModel == null)
            {
                return NotFound();
            }

            return View(questionModel);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var questionModel = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);
            _context.Questions.Remove(questionModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionModelExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
