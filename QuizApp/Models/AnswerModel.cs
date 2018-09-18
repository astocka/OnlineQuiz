using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Answer Option")]
        public string AnswerOption { get; set; }

        //public int QuestionId { get; set; }
        //[ForeignKey("QuestionId")]
        public QuestionModel Question { get; set; }
    }
}
