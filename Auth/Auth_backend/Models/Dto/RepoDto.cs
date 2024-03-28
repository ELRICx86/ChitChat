using FLiu__Auth.Models.Dto.NewFolder;

namespace FLiu__Auth.Models.Dto
{
    public class RepoDto
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public User User { get; set; }
        public Identity Identity { get; set; }
    }
}
