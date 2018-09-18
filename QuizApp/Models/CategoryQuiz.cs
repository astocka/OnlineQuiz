using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class CategoryQuiz
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int QuizId { get; set; }
        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; }
        [ForeignKey("QuizId")]
        public QuizModel Quiz { get; set; }
    }
}
