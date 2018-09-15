using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class QuizModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime TotalTime { get; set; }
        public int TotalQuestions { get; set; }
        public int PassingPercentage { get; set; }

        public QuizCategoryModel QuizCategory { get; set; }
        public ICollection<QuestionModel> Questions { get; set; }

    }
}
