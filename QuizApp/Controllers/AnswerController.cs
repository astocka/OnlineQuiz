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
    public class AnswerController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Answer
        public async Task<IActionResult> Index(int? questionId)
        {
            var appDbContext = _context.Answers.Include(a => a.Question).Where(q => q.QuestionId == questionId);
            var question = await _context.Questions.Include(qz => qz.Quiz).FirstOrDefaultAsync(q => q.Id == questionId);
            ViewBag.Question = question.Question;
            ViewBag.CorrectAnswer = question.CorrectAnswer;
            ViewBag.QuizTitle = question.Quiz.Title;
            return View(await appDbContext.ToListAsync());
        }

        //// GET: Answer/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var answerModel = await _context.Answers
        //        .Include(a => a.Question)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (answerModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(answerModel);
        //}

        // GET: Answer/Create
        public IActionResult Create()
        {

            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Question"); // CorrectAnswer
            return View();

        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AnswerOption,QuestionId")] AnswerModel answerModel)
        {
            if (ModelState.IsValid)
            {

                _context.Add(answerModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Question");
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Question", answerModel.QuestionId);
            return View(answerModel);
        }

        // GET: Answer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerModel = await _context.Answers.Include(q => q.Question).SingleOrDefaultAsync(m => m.Id == id);
            if (answerModel == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Question", answerModel.QuestionId);
            return View(answerModel);
        }

        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AnswerOption,QuestionId")] AnswerModel answerModel)
        {
            if (id != answerModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answerModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerModelExists(answerModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Question");
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Question", answerModel.QuestionId);
            return View(answerModel);
        }

        // GET: Answer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answerModel = await _context.Answers
                .Include(a => a.Question)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answerModel == null)
            {
                return NotFound();
            }

            return View(answerModel);
        }

        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answerModel = await _context.Answers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Answers.Remove(answerModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Question");
        }

        private bool AnswerModelExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
