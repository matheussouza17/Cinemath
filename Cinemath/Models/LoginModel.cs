using System.ComponentModel.DataAnnotations;

namespace Cinemath.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Required")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}
