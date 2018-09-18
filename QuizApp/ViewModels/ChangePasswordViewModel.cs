using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [DisplayName("New Password")]
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        [DisplayName("Repeat Password")]
        public string RepeatPassword { get; set; }
    }
}
