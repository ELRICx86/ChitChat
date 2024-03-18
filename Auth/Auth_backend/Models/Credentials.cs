using System.ComponentModel.DataAnnotations;

namespace FLiu__Auth.Models
{
    public class Credentials
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
