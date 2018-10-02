using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Context;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizApp.Controllers
{
    public class ResultController : Controller
    {
        private readonly AppDbContext _context;

        public ResultController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: /<controller>/
        //public async Task<IActionResult> Index(int? quizId)
        //{
        //    var attemptQuiz = await _context.Quizzes.Include(c => c.Category).Include(q => q.Questions).ThenInclude(a => a.Answers)
        //        .FirstOrDefaultAsync(quiz => quiz.Id == quizId);

        //    return View();
        //}
    }
}
