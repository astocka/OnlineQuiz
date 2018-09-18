using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QuizApp.Models
{
    public class QuizModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [DisplayName("Total Time")]
        public int TotalTime { get; set; }
        [DisplayName("Total Questions")]
        public int TotalQuestions { get; set; }
        [DisplayName("Passing Percentage")]
        public decimal PassingPercentage { get; set; }

        //public int QuizCategoryId { get; set; }
        //[ForeignKey("QuizCategoryId")]

        public string CategoryName { get; set; }
        public ICollection<CategoryQuiz> CategoryQuiz { get; set; }

        public ICollection<QuestionModel> Questions { get; set; }

    }
}
