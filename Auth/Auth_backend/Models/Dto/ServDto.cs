using FLiu__Auth.Models.Dto.NewFolder;

namespace FLiu__Auth.Models.Dto
{
    public class ServDto
    {
        public string StatusCode { get; set; }
        public bool status { get; set; }
        public string Message { get; set; }

        public Identity Identity { get; set; }
    }
}
