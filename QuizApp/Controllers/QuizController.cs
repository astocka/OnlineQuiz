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
    public class QuizController : Controller
    {
        private readonly AppDbContext _context;

        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Quiz
        public async Task<IActionResult> Index()
        {
            var quizzes = await _context.Quizzes.Include(c => c.Category).ToListAsync();
            return View(quizzes);
        }

        // GET: Quiz/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizModel = await _context.Quizzes.Include(q => q.Questions).ThenInclude(a => a.Answers)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (quizModel == null)
            {
                return NotFound();
            }

            return View(quizModel);
        }

        // GET: Quiz/Create
        public async Task<IActionResult> Create(int? categoryId)
        {
            if (categoryId == null)
            {
                return NotFound();
            }
            else
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                ViewBag.CategoryName = category.Name;
                ViewBag.CategoryId = categoryId;
                return View();
            }
        }

        // POST: Quiz/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,TotalTime,TotalQuestions,PassingPercentage,CategoryId")] QuizModel quizModel)
        {
            if (ModelState.IsValid)
            {
                _context.Quizzes.Include(c => c.Category);
                _context.Add(quizModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Admin");
            }
            return View(quizModel);
        }

        // GET: Quiz/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes.Include(c => c.Category).SingleOrDefaultAsync(m => m.Id == id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        // POST: Quiz/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,TotalTime,TotalQuestions,PassingPercentage,CategoryId")] QuizModel quizModel)
        {
            if (id != quizModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizModelExists(quizModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Quiz");
            }
            return View(quizModel);
        }

        // GET: Quiz/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizModel = await _context.Quizzes
                .SingleOrDefaultAsync(m => m.Id == id);
            if (quizModel == null)
            {
                return NotFound();
            }

            return View(quizModel);
        }

        // POST: Quiz/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizModel = await _context.Quizzes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Quizzes.Remove(quizModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Quiz");
        }

        private bool QuizModelExists(int id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Attempt(int? quizId, int? questionNumber)
        {
            var attemptQuiz = await _context.Quizzes.Include(c => c.Category).Include(q => q.Questions).ThenInclude(a => a.Answers)
                .FirstOrDefaultAsync(quiz => quiz.Id == quizId);

            var question = attemptQuiz.Questions.FirstOrDefault(q => q.QuestionNumber == questionNumber);

            if (attemptQuiz == null || question == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.QuizCategory = attemptQuiz.Category.Name;
                ViewBag.QuizTitle = attemptQuiz.Title;
                ViewBag.QuizCorrectAnswer = question.CorrectAnswer;
                ViewBag.QuizUserName = User.Identity.Name;
                ViewBag.Question = question;
                ViewBag.QuizId = quizId;
                ViewBag.QuestionNumber = questionNumber;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Attempt(int quizId, int questionNumber, [Bind("Id,QuizCategory,QuizTitle,AttemptQuestion,AttemptAnswer,CorrectAnswer,UserName")] AttemptModel attempt)
        {
            if (attempt == null)
            {
                return NotFound();
            }
            else
            {
                var attemptQuiz = await _context.Quizzes.Include(c => c.Category).Include(q => q.Questions).ThenInclude(a => a.Answers)
                    .FirstOrDefaultAsync(quiz => quiz.Id == quizId);

                var attemptQuestion = attemptQuiz.Questions.FirstOrDefault(q => q.QuestionNumber == questionNumber);

                if (attempt.AttemptAnswer == attemptQuestion.CorrectAnswer)
                {
                    attempt.Subscore = 1;
                }
                else
                {
                    attempt.Subscore = 0;
                }

                _context.Attempts.Add(attempt);
                await _context.SaveChangesAsync();

                var nextQuestion = attemptQuiz.Questions.FirstOrDefault(n => n.QuestionNumber == questionNumber + 1);

                if (nextQuestion == null)
                {
                    return Content("End of quiz");
                }
                else
                {
                    return RedirectToAction("Attempt", "Quiz", new { quizId = quizId, questionNumber = questionNumber + 1 });
                }

            }
        }

        [HttpGet]
        public async Task<IActionResult> UserResult(string userName)
        {
            if (userName == null)
            {
                return NotFound();
            }
            else
            {
                var results = await _context.Attempts.Where(u => u.UserName == userName).ToListAsync();
                return View(results);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Ranking()
        {
            var result = await _context.Attempts.OrderByDescending(s => s.Subscore).ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return View(result);
            }
        }
    }
}
