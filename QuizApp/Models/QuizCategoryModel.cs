using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    public class QuizCategoryModel
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }

        public ICollection<QuizModel> Quiz { get; set; }
    }
}
