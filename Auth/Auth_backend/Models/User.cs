using System.ComponentModel.DataAnnotations;

namespace FLiu__Auth.Models
{
    public class User
    {
        [Required]
        public string Email { get; set; }
        [Required] 
        public byte[] PasswordSalt { get; set; }
        [Required] 
        public byte[] PasswordHash { get; set; }
    }
}
