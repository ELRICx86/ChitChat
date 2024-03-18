using System.ComponentModel.DataAnnotations;

namespace chatapi.Dtos
{
    public class UserDto
    {
        [Required]
        [StringLength(15,MinimumLength =3, ErrorMessage ="Name must be at ")]
        public string Name { get; set; }
    }
}
