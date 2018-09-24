using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class ResultModel
    {
        public int Id { get; set; }

        [Required]
        public string QuizCategory { get; set; }
        [Required]
        public string QuizTitle { get; set; }

        public int TotalScore { get; set; }

        public DateTime Date { get; set; }

        [DisplayName("User")]
        public string UserName { get; set; }

        public int? Attempt { get; set; }

    }
}
