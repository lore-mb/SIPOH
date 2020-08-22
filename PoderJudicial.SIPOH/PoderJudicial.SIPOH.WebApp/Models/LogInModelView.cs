using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class LogInModelView
    {
        [Required]
        public string Usuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}