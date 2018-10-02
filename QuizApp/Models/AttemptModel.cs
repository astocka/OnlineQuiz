using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class AttemptModel
    {
        public int Id { get; set; }
        [Required]
        public string QuizCategory { get; set; }
        [Required]
        public string QuizTitle { get; set; }
        [Required]
        public string AttemptQuestion { get; set; }
        [Required]
        public string AttemptAnswer { get; set; }
        [Required]
        public string CorrectAnswer { get; set; }
        public int Subscore { get; set; }

        [DisplayName("User")]
        public string UserName { get; set; }

        public int? ResultNo { get; set; }
    }
}
