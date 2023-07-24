using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinemath.Models
{
    public class WishList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        [Required(ErrorMessage = "Required")]
        public int UsersId { get; set; }
       // public User User { get; set; }

        public WishList() { }
    }
}
