using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class LogInModelView
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}