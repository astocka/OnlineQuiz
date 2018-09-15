using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        [Required]
        public string AnswerOption { get; set; }

        public QuestionModel Question { get; set; }
    }
}
