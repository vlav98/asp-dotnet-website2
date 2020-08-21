using System.ComponentModel.DataAnnotations;

namespace Mon2ndSite.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Pseudonyme")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
    }
 }