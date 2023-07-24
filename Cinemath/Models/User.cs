using System.ComponentModel.DataAnnotations;

namespace Cinemath.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must contain at least 8 characters, including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password{get; set;}
        
        public User() { }

        public bool ValidPassword(string password)
        {
            return Password == password;
        }
    }
}
